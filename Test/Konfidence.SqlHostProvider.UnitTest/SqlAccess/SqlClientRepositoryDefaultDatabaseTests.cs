using System;
using FluentAssertions;
using Konfidence.SqlDataAccess;
using Konfidence.SqlHostProvider.SqlAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ConfigurationBuilder = Microsoft.Extensions.Configuration.ConfigurationBuilder;

namespace Konfidence.SqlHostProvider.UnitTest.SqlAccess;

/// <summary>
/// GetDatabase() resolves through IClientConfig first and only falls back to
/// IDefaultDatabaseProvider when no connection matches. With the provider injected, that fallback
/// no longer needs app.config to be manipulated, so all three outcomes are testable without touching
/// process-wide configuration or a database.
/// </summary>
[TestClass]
public class SqlClientRepositoryDefaultDatabaseTests
{
    [TestMethod]
    public void GetDatabase_WithMatchingClientConfigConnection_DoesNotConsultTheFallbackProvider()
    {
        // Arrange
        // IClientConfig is the source of truth: when it can answer, the legacy app.config lookup
        // must not run at all.
        TestContext context = CreateContext();

        context.ClientConfig.DefaultDatabase = "TestConnection";
        context.ClientConfig.Connections =
        [
            new ConfigConnectionString { ConnectionName = "TestConnection", Server = "some-server", Database = "SomeDatabase" }
        ];

        // Act
        SqlDatabase database = context.Repository.GetDatabase();

        // Assert
        database.Should().NotBeNull();
        context.DefaultDatabaseProviderMock.Verify(x => x.TryGetDefaultConnectionString(out It.Ref<string>.IsAny), Times.Never);
    }

    [TestMethod]
    public void GetDatabase_WithoutMatchingConnection_UsesTheFallbackProvidersConnectionString()
    {
        // Arrange
        TestContext context = CreateContext();

        context.ClientConfig.DefaultDatabase = "NoSuchConnection";

        string fallbackConnectionString = "Data Source=fallback-server;Initial Catalog=FallbackDatabase;Integrated Security=True";

        context.DefaultDatabaseProviderMock
            .Setup(x => x.TryGetDefaultConnectionString(out fallbackConnectionString))
            .Returns(true);

        // Act
        SqlDatabase database = context.Repository.GetDatabase();

        // Assert
        database.CreateConnection().ConnectionString.Should().Contain("FallbackDatabase");
    }

    [TestMethod]
    public void GetDatabase_WithoutMatchingConnectionAndNoFallback_Throws()
    {
        // Arrange
        TestContext context = CreateContext();

        context.ClientConfig.DefaultDatabase = "NoSuchConnection";

        string noConnectionString = string.Empty;

        context.DefaultDatabaseProviderMock
            .Setup(x => x.TryGetDefaultConnectionString(out noConnectionString))
            .Returns(false);

        // Act
        Action action = () => context.Repository.GetDatabase();

        // Assert
        action.Should().Throw<InvalidOperationException>().WithMessage("No connection could be resolved*");
    }

    private sealed class TestContext
    {
        public TestContext(
            SqlClientRepository Repository,
            ClientConfig ClientConfig,
            Mock<IDefaultDatabaseProvider> DefaultDatabaseProviderMock
        )
        {
            this.Repository = Repository;
            this.ClientConfig = ClientConfig;
            this.DefaultDatabaseProviderMock = DefaultDatabaseProviderMock;
        }

        public SqlClientRepository Repository { get; }

        public ClientConfig ClientConfig { get; }

        public Mock<IDefaultDatabaseProvider> DefaultDatabaseProviderMock { get; }
    }

    private static TestContext CreateContext()
    {
        ClientConfig clientConfig = new(new ConfigurationBuilder().Build());

        Mock<IDefaultDatabaseProvider> defaultDatabaseProviderMock = new();

        SqlClientRepository repository = new(clientConfig, defaultDatabaseProviderMock.Object);

        return new TestContext(repository, clientConfig, defaultDatabaseProviderMock);
    }
}
