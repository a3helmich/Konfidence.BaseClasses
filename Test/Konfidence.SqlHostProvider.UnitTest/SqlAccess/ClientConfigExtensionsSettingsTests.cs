using System.Linq;
using FluentAssertions;
using Konfidence.SqlHostProvider.SqlAccess;
using Konfidence.SqlHostProvider.SqlConnectionManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ConfigurationBuilder = Microsoft.Extensions.Configuration.ConfigurationBuilder;

namespace Konfidence.SqlHostProvider.UnitTest.SqlAccess;

/// <summary>
/// SetSqlApplicationSettings decides what to publish back to the host application's configuration.
/// With the writer and the security-file locator injected, those decisions are testable without
/// rewriting the test host's app.config on disk - which is what previously confined this logic to
/// the integration suite.
/// </summary>
[TestClass]
public class ClientConfigExtensionsSettingsTests
{
    [TestMethod]
    public void SetSqlApplicationSettings_WithoutDefaultDatabase_WritesNothing()
    {
        // Arrange
        TestContext context = CreateContext();

        // Act
        context.ClientConfig.SetSqlApplicationSettings(context.WriterMock.Object, context.SecurityFileLocatorMock.Object);

        // Assert
        context.WriterMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void SetSqlApplicationSettings_WithMatchingConnection_PublishesBothTheConnectionAndTheDefault()
    {
        // Arrange
        TestContext context = CreateContext();

        context.ClientConfig.DefaultDatabase = "TestConnection";
        context.ClientConfig.Connections =
        [
            new ConfigConnectionString { ConnectionName = "TestConnection", Database = "TestDatabase", Server = "test-server" }
        ];

        // Act
        context.ClientConfig.SetSqlApplicationSettings(context.WriterMock.Object, context.SecurityFileLocatorMock.Object);

        // Assert
        context.WriterMock.Verify(x => x.SetConnectionString("TestConnection", "TestDatabase", "test-server"), Times.Once);
        context.WriterMock.Verify(x => x.SetDefaultDatabase("TestConnection"), Times.Once);
    }

    [TestMethod]
    public void SetSqlApplicationSettings_WithoutMatchingConnection_StillPublishesTheDefaultDatabase()
    {
        // Arrange
        // The default database is published even when no connection carries that name, so a host
        // configured only through app.config keeps working.
        TestContext context = CreateContext();

        context.ClientConfig.DefaultDatabase = "NoSuchConnection";

        // Act
        context.ClientConfig.SetSqlApplicationSettings(context.WriterMock.Object, context.SecurityFileLocatorMock.Object);

        // Assert
        context.WriterMock.Verify(x => x.SetDefaultDatabase("NoSuchConnection"), Times.Once);
        context.WriterMock.Verify(x => x.SetConnectionString(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [TestMethod]
    public void SetSqlApplicationSettings_WithoutUseEnvironmentSetting_DoesNotLookForASecurityFile()
    {
        // Arrange
        TestContext context = CreateContext();

        context.ClientConfig.DefaultDatabase = "TestConnection";
        context.ClientConfig.UseEnvironmentSetting = false;

        // Act
        context.ClientConfig.SetSqlApplicationSettings(context.WriterMock.Object, context.SecurityFileLocatorMock.Object);

        // Assert
        context.SecurityFileLocatorMock.Verify(x => x.TryGetSecurityFilePath(out It.Ref<string>.IsAny), Times.Never);
    }

    [TestMethod]
    public void SetSqlApplicationSettings_WithCredentialsAlreadyPresent_DoesNotLookForASecurityFile()
    {
        // Arrange
        // Credentials already on the connection mean there is nothing to fill in, so the security
        // file is not consulted even with UseEnvironmentSetting switched on.
        TestContext context = CreateContext();

        context.ClientConfig.DefaultDatabase = "TestConnection";
        context.ClientConfig.UseEnvironmentSetting = true;
        context.ClientConfig.Connections =
        [
            new ConfigConnectionString { ConnectionName = "TestConnection", Server = "test-server", UserName = "existing-user", Password = "existing-password" }
        ];

        // Act
        context.ClientConfig.SetSqlApplicationSettings(context.WriterMock.Object, context.SecurityFileLocatorMock.Object);

        // Assert
        context.SecurityFileLocatorMock.Verify(x => x.TryGetSecurityFilePath(out It.Ref<string>.IsAny), Times.Never);
    }

    [TestMethod]
    public void SetSqlApplicationSettings_WithMissingCredentials_LooksForASecurityFile()
    {
        // Arrange
        TestContext context = CreateContext();

        context.ClientConfig.DefaultDatabase = "TestConnection";
        context.ClientConfig.UseEnvironmentSetting = true;
        context.ClientConfig.Connections =
        [
            new ConfigConnectionString { ConnectionName = "TestConnection", Server = "test-server" }
        ];

        // Act
        context.ClientConfig.SetSqlApplicationSettings(context.WriterMock.Object, context.SecurityFileLocatorMock.Object);

        // Assert
        context.SecurityFileLocatorMock.Verify(x => x.TryGetSecurityFilePath(out It.Ref<string>.IsAny), Times.Once);
    }

    [TestMethod]
    public void SetSqlApplicationSettings_WithNoConnectionAtAll_LooksForASecurityFile()
    {
        // Arrange
        // The other half of the same condition: no matching connection also counts as "credentials
        // missing", which a test with a credential-less connection alone would not prove.
        TestContext context = CreateContext();

        context.ClientConfig.DefaultDatabase = "NoSuchConnection";
        context.ClientConfig.UseEnvironmentSetting = true;

        // Act
        context.ClientConfig.SetSqlApplicationSettings(context.WriterMock.Object, context.SecurityFileLocatorMock.Object);

        // Assert
        context.SecurityFileLocatorMock.Verify(x => x.TryGetSecurityFilePath(out It.Ref<string>.IsAny), Times.Once);
    }

    private sealed class TestContext
    {
        public TestContext(
            ClientConfig ClientConfig,
            Mock<IApplicationConfigurationWriter> WriterMock,
            Mock<ISqlSecurityFileLocator> SecurityFileLocatorMock
        )
        {
            this.ClientConfig = ClientConfig;
            this.WriterMock = WriterMock;
            this.SecurityFileLocatorMock = SecurityFileLocatorMock;
        }

        public ClientConfig ClientConfig { get; }

        public Mock<IApplicationConfigurationWriter> WriterMock { get; }

        public Mock<ISqlSecurityFileLocator> SecurityFileLocatorMock { get; }
    }

    private static TestContext CreateContext()
    {
        ClientConfig clientConfig = new(new ConfigurationBuilder().Build())
        {
            Connections = []
        };

        Mock<IApplicationConfigurationWriter> writerMock = new();

        Mock<ISqlSecurityFileLocator> securityFileLocatorMock = new();

        string noPath = string.Empty;
        securityFileLocatorMock.Setup(x => x.TryGetSecurityFilePath(out noPath)).Returns(false);

        return new TestContext(clientConfig, writerMock, securityFileLocatorMock);
    }
}
