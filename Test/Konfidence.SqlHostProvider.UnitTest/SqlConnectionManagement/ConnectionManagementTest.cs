using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using FluentAssertions;
using Konfidence.Base;
using Konfidence.SqlDataAccess;
using Konfidence.SqlHostProvider.SqlAccess;
using Konfidence.TestTools;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConfigurationBuilder = Microsoft.Extensions.Configuration.ConfigurationBuilder;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace Konfidence.SqlHostProvider.UnitTest.SqlConnectionManagement
{
    [TestClass]
    public class ConnectionManagementTest
    {
        [TestMethod]
        public void When_ConfigureSettings_read_with_multiple_connections_Should_set_them_all_in_ClientConfig()
        {
            // arrange
            IServiceProvider di = DependencyInjectionFactory.ConfigureDependencyInjection();

            // act
            IClientConfig? clientConfig = di.GetService<IClientConfig>();

            // assert
            if (!clientConfig.IsAssigned())
            {
                throw new Exception("mayor fail");
            }

            clientConfig.Connections.Should().HaveCountGreaterThan(1);
            clientConfig.Connections.Where(x => !x.Password.IsAssigned()).Should().HaveCount(0);
        }

        [TestMethod]
        public void SetConnectionStringPart_With_no_existing_part_Should_add_the_part()
        {
            // Arrange
            List<string> connectionStringParts = ["Server=konfidence2"];

            // Act
            Konfidence.SqlHostProvider.SqlConnectionManagement.ConnectionManagement.SetConnectionStringPart(connectionStringParts, "Database", "MyDatabase");

            // Assert
            connectionStringParts.Should().Contain("Database=MyDatabase");
        }

        [TestMethod]
        public void SetConnectionStringPart_With_existing_part_Should_replace_the_part()
        {
            // Arrange
            List<string> connectionStringParts = ["Server=konfidence2", "Database=OldDatabase"];

            // Act
            Konfidence.SqlHostProvider.SqlConnectionManagement.ConnectionManagement.SetConnectionStringPart(connectionStringParts, "Database", "NewDatabase");

            // Assert
            connectionStringParts.Should().ContainSingle(x => x.StartsWith("Database=", StringComparison.OrdinalIgnoreCase));
            connectionStringParts.Should().Contain("Database=NewDatabase");
            connectionStringParts.Should().NotContain("Database=OldDatabase");
        }

        [TestMethod]
        public void SetConnectionStringPart_Is_case_insensitive_when_finding_the_existing_part()
        {
            // Arrange
            List<string> connectionStringParts = ["database=OldDatabase"];

            // Act
            Konfidence.SqlHostProvider.SqlConnectionManagement.ConnectionManagement.SetConnectionStringPart(connectionStringParts, "Database", "NewDatabase");

            // Assert
            connectionStringParts.Should().ContainSingle();
            connectionStringParts.Should().Contain("Database=NewDatabase");
        }

        [TestMethod]
        public void SetConnectionStringPart_With_unassigned_value_Should_leave_parts_unchanged()
        {
            // Arrange
            List<string> connectionStringParts = ["Server=konfidence2"];

            // Act
            Konfidence.SqlHostProvider.SqlConnectionManagement.ConnectionManagement.SetConnectionStringPart(connectionStringParts, "Database", string.Empty);

            // Assert
            connectionStringParts.Should().BeEquivalentTo(["Server=konfidence2"]);
        }

        [TestMethod]
        public void CopySqlSecurityToClientConfig_With_no_matching_server_Should_leave_connection_unchanged()
        {
            // Arrange
            // Note: deliberately does not touch the "ClientConfigLocation" environment variable — it can be set
            // at User/Machine scope on a dev machine, which TryGetEnvironmentVariable checks before Process scope,
            // so a process-only override can't reliably fake "unset" or "points at our fixture" here. A server
            // name that can't plausibly appear in any real security file keeps this test hermetic either way.
            ClientConfig clientConfig = new(new ConfigurationBuilder().Build())
            {
                Connections = [new ConfigConnectionString { Server = "unit-test-nonexistent-server", UserName = string.Empty, Password = string.Empty }]
            };

            // Act
            Konfidence.SqlHostProvider.SqlConnectionManagement.ConnectionManagement.CopySqlSecurityToClientConfig(clientConfig);

            // Assert
            clientConfig.Connections.Single().UserName.Should().BeEmpty();
        }

        [TestMethod]
        public void CopySqlSecurityToClientConfig_With_matching_server_Should_copy_UserName_and_Password()
        {
            // Arrange
            if (!"ClientConfigLocation".TryGetEnvironmentVariable(out string fileName) || !File.Exists(fileName))
            {
                Assert.Inconclusive("ClientConfigLocation is not configured in this environment; the matching-server path can't be exercised here.");
            }

            if (!File.ReadAllText(fileName).Deserialize(out ClientSettings? clientSettings) || !clientSettings.DataConfiguration.IsAssigned() || !clientSettings.DataConfiguration.Connections.Any())
            {
                Assert.Inconclusive("ClientConfigLocation's file has no connections to match against.");
            }

            ConfigConnectionString realConnection = clientSettings!.DataConfiguration!.Connections.First();

            ClientConfig clientConfig = new(new ConfigurationBuilder().Build())
            {
                Connections = [new ConfigConnectionString { Server = realConnection.Server, UserName = string.Empty, Password = string.Empty }]
            };

            // Act
            Konfidence.SqlHostProvider.SqlConnectionManagement.ConnectionManagement.CopySqlSecurityToClientConfig(clientConfig);

            // Assert
            ConfigConnectionString connection = clientConfig.Connections.Single();
            connection.UserName.Should().Be(realConnection.UserName);
            connection.Password.Should().Be(realConnection.Password);
        }

        [TestMethod]
        public void SetActiveConnection_Should_update_the_active_dataConfiguration_defaultDatabase()
        {
            // Arrange
            SqlTestToolExtensions.CopySqlSettingsToActiveConfiguration();

            try
            {
                // Act
                Konfidence.SqlHostProvider.SqlConnectionManagement.ConnectionManagement.SetActiveConnection("BlockedHackers");

                // Assert
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                DatabaseSettings? databaseSettings = config.Sections["dataConfiguration"] as DatabaseSettings;

                databaseSettings.Should().NotBeNull();
                databaseSettings!.DefaultDatabase.Should().Be("BlockedHackers");
            }
            finally
            {
                SqlTestToolExtensions.CopySqlSettingsToActiveConfiguration();
            }
        }

        [TestMethod]
        public void SetApplicationDatabase_Should_update_the_matching_connectionString()
        {
            // Arrange
            SqlTestToolExtensions.CopySqlSettingsToActiveConfiguration();

            try
            {
                // Act
                Konfidence.SqlHostProvider.SqlConnectionManagement.ConnectionManagement.SetApplicationDatabase("NewDatabaseName", "NewServerName", "TShirt");

                // Assert
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                ConnectionStringSettings? connectionStringSettings = config.ConnectionStrings.ConnectionStrings["TShirt"];

                connectionStringSettings.Should().NotBeNull();
                connectionStringSettings!.ConnectionString.Should().Contain("Database=NewDatabaseName");
                connectionStringSettings.ConnectionString.Should().Contain("Server=NewServerName");
            }
            finally
            {
                SqlTestToolExtensions.CopySqlSettingsToActiveConfiguration();
            }
        }

        [TestMethod]
        public void SetApplicationDatabase_With_no_matching_connectionName_Should_return_without_touching_configuration()
        {
            // Arrange
            SqlTestToolExtensions.CopySqlSettingsToActiveConfiguration();

            // Act
            Action action = () => Konfidence.SqlHostProvider.SqlConnectionManagement.ConnectionManagement.SetApplicationDatabase("NewDatabaseName", "NewServerName", "NonExistentConnectionName");

            // Assert
            action.Should().NotThrow();
        }
    }
}
