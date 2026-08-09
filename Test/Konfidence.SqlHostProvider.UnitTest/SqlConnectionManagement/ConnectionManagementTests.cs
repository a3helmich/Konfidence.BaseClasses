using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Konfidence.Base;
using Konfidence.SqlHostProvider.SqlAccess;
using Konfidence.SqlHostProvider.SqlConnectionManagement;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConfigurationBuilder = Microsoft.Extensions.Configuration.ConfigurationBuilder;

namespace Konfidence.SqlHostProvider.UnitTest.SqlConnectionManagement;

/// <summary>
/// Exercises CopySqlSecurityToClientConfig through its two seams: a fixture file path, and a stubbed
/// ISqlSecurityFileLocator. The production locator reads "ClientConfigLocation" via
/// TryGetEnvironmentVariable, which resolves User scope before Process scope - so a test process can
/// neither redirect nor clear it, and none of these branches were reachable before those seams
/// existed. Nothing here touches a database.
/// </summary>
[TestClass]
public class ConnectionManagementTests
{
    [TestMethod]
    public void CopySqlSecurityToClientConfig_WithMissingFile_LeavesConnectionsUntouched()
    {
        // Arrange
        string fileName = Path.Combine(Path.GetTempPath(), $"NoSuchSecurityFile_{Guid.NewGuid():N}.json");

        ClientConfig clientConfig = CreateClientConfig("konfidence-test-server");

        // Act
        ConnectionManagement.CopySqlSecurityToClientConfig(clientConfig, fileName);

        // Assert
        clientConfig.Connections.Single().UserName.Should().BeEmpty();
        clientConfig.Connections.Single().Password.Should().BeEmpty();
    }

    [TestMethod]
    public void CopySqlSecurityToClientConfig_WithUnparsableFile_LeavesConnectionsUntouched()
    {
        // Arrange
        ClientConfig clientConfig = CreateClientConfig("konfidence-test-server");

        RunWithSecurityFile("{ not valid json", fileName =>
        {
            // Act
            ConnectionManagement.CopySqlSecurityToClientConfig(clientConfig, fileName);
        });

        // Assert
        clientConfig.Connections.Single().UserName.Should().BeEmpty();
    }

    [TestMethod]
    public void CopySqlSecurityToClientConfig_WithoutDataConfigurationSection_LeavesConnectionsUntouched()
    {
        // Arrange
        // A syntactically valid settings file that simply carries no DataConfiguration - the second
        // arm of the three-part guard.
        ClientConfig clientConfig = CreateClientConfig("konfidence-test-server");

        RunWithSecurityFile(new ClientSettings().Serialize(), fileName =>
        {
            // Act
            ConnectionManagement.CopySqlSecurityToClientConfig(clientConfig, fileName);
        });

        // Assert
        clientConfig.Connections.Single().UserName.Should().BeEmpty();
    }

    [TestMethod]
    public void CopySqlSecurityToClientConfig_WithNoConnectionsInFile_LeavesConnectionsUntouched()
    {
        // Arrange
        // DataConfiguration present but empty - the third arm of the guard, distinct from both a
        // parse failure and a missing section.
        ClientConfig clientConfig = CreateClientConfig("konfidence-test-server");

        ClientSettings clientSettings = new()
        {
            DataConfiguration = new DataConfiguration { Connections = [] }
        };

        RunWithSecurityFile(clientSettings.Serialize(), fileName =>
        {
            // Act
            ConnectionManagement.CopySqlSecurityToClientConfig(clientConfig, fileName);
        });

        // Assert
        clientConfig.Connections.Single().UserName.Should().BeEmpty();
    }

    [TestMethod]
    public void CopySqlSecurityToClientConfig_WithMatchingServer_CopiesUserNameAndPassword()
    {
        // Arrange
        ClientConfig clientConfig = CreateClientConfig("konfidence-test-server");

        RunWithSecurityFile(CreateSecuritySettings("konfidence-test-server", "sa-user", "sa-password").Serialize(), fileName =>
        {
            // Act
            ConnectionManagement.CopySqlSecurityToClientConfig(clientConfig, fileName);
        });

        // Assert
        ConfigConnectionString connection = clientConfig.Connections.Single();

        connection.UserName.Should().Be("sa-user");
        connection.Password.Should().Be("sa-password");
    }

    [TestMethod]
    public void CopySqlSecurityToClientConfig_WithNonMatchingServer_LeavesConnectionsUntouched()
    {
        // Arrange
        // Matching is by server name, so credentials for a different server must not leak across.
        ClientConfig clientConfig = CreateClientConfig("konfidence-test-server");

        RunWithSecurityFile(CreateSecuritySettings("some-other-server", "sa-user", "sa-password").Serialize(), fileName =>
        {
            // Act
            ConnectionManagement.CopySqlSecurityToClientConfig(clientConfig, fileName);
        });

        // Assert
        ConfigConnectionString connection = clientConfig.Connections.Single();

        connection.UserName.Should().BeEmpty();
        connection.Password.Should().BeEmpty();
    }

    [TestMethod]
    public void CopySqlSecurityToClientConfig_WithSeveralConnectionsOnTheSameServer_UpdatesEveryOneOfThem()
    {
        // Arrange
        // The copy runs over every client-config connection whose server matches, not just the
        // first - a single-connection fixture cannot tell those two implementations apart.
        ClientConfig clientConfig = new(new ConfigurationBuilder().Build())
        {
            Connections =
            [
                new ConfigConnectionString { ConnectionName = "First", Server = "konfidence-test-server" },
                new ConfigConnectionString { ConnectionName = "Second", Server = "konfidence-test-server" },
                new ConfigConnectionString { ConnectionName = "Elsewhere", Server = "some-other-server" }
            ]
        };

        RunWithSecurityFile(CreateSecuritySettings("konfidence-test-server", "sa-user", "sa-password").Serialize(), fileName =>
        {
            // Act
            ConnectionManagement.CopySqlSecurityToClientConfig(clientConfig, fileName);
        });

        // Assert
        clientConfig.Connections.Where(x => x.Server == "konfidence-test-server")
            .Should().OnlyContain(x => x.UserName == "sa-user" && x.Password == "sa-password");

        clientConfig.Connections.Single(x => x.Server == "some-other-server").UserName.Should().BeEmpty();
    }

    [TestMethod]
    public void CopySqlSecurityToClientConfig_WhenTheLocatorFindsNoFile_LeavesConnectionsUntouched()
    {
        // Arrange
        // With the lookup behind ISqlSecurityFileLocator, the "no security file configured" path is
        // reachable at last - through the environment variable directly it never was, because
        // TryGetEnvironmentVariable resolves User scope before Process.
        ClientConfig clientConfig = CreateClientConfig("konfidence-test-server");

        Mock<ISqlSecurityFileLocator> securityFileLocatorMock = new();

        string noPath = string.Empty;
        securityFileLocatorMock.Setup(x => x.TryGetSecurityFilePath(out noPath)).Returns(false);

        // Act
        ConnectionManagement.CopySqlSecurityToClientConfig(clientConfig, securityFileLocatorMock.Object);

        // Assert
        clientConfig.Connections.Single().UserName.Should().BeEmpty();
    }

    [TestMethod]
    public void CopySqlSecurityToClientConfig_WhenTheLocatorFindsAFile_CopiesFromIt()
    {
        // Arrange
        ClientConfig clientConfig = CreateClientConfig("konfidence-test-server");

        RunWithSecurityFile(CreateSecuritySettings("konfidence-test-server", "sa-user", "sa-password").Serialize(), fileName =>
        {
            Mock<ISqlSecurityFileLocator> securityFileLocatorMock = new();
            securityFileLocatorMock.Setup(x => x.TryGetSecurityFilePath(out fileName)).Returns(true);

            // Act
            ConnectionManagement.CopySqlSecurityToClientConfig(clientConfig, securityFileLocatorMock.Object);
        });

        // Assert
        clientConfig.Connections.Single().UserName.Should().Be("sa-user");
    }

    private static ClientConfig CreateClientConfig(string server)
    {
        return new ClientConfig(new ConfigurationBuilder().Build())
        {
            Connections = [new ConfigConnectionString { ConnectionName = "TestConnection", Server = server }]
        };
    }

    private static ClientSettings CreateSecuritySettings(string server, string userName, string password)
    {
        return new ClientSettings
        {
            DataConfiguration = new DataConfiguration
            {
                Connections =
                [
                    new ConfigConnectionString { ConnectionName = "TestConnection", Server = server, UserName = userName, Password = password }
                ]
            }
        };
    }

    private static void RunWithSecurityFile(string fileContent, Action<string> useFile)
    {
        string fileName = Path.Combine(Path.GetTempPath(), $"SqlSecurityTest_{Guid.NewGuid():N}.json");

        File.WriteAllText(fileName, fileContent);

        try
        {
            useFile(fileName);
        }
        finally
        {
            File.Delete(fileName);
        }
    }
}
