using System;
using System.Threading.Tasks;
using FluentAssertions;
using Konfidence.Base;
using Konfidence.SqlHostProvider.Exceptions;
using Konfidence.SqlHostProvider.SqlAccess;
using Konfidence.SqlHostProvider.SqlConnectionManagement;
using Konfidence.SqlHostProvider.SqlServerManagement;
using Konfidence.TestTools;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.SqlServer.Management.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.SqlHostProvider.Tests.SqlServerManagement
{
    [TestClass]
    public class SqlServerCheckTests
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext _)
        {
            SqlTestToolExtensions.CopySqlSettingsToActiveConfiguration();

            SqlTestToolExtensions.CopySqlSecurityToClientConfig("TestClassGenerator");
            SqlTestToolExtensions.CopySqlSecurityToClientConfig("TestDatabase");
        }

        [TestMethod, TestCategory("SqlServer")]
        public void When_VerifyDatabaseServer_With_invalid_database_Should_return_DoesNotExist()
        {
            // Arrange
            var di = DependencyInjectionFactory.ConfigureDependencyInjection();
            var clientConfig = di.GetService<IClientConfig>();

            if (!clientConfig.IsAssigned())
            {
                throw new Exception("clientconfig not returned by dependency injection");
            }

            clientConfig.DefaultDatabase = "TestClassGenerator";

            var connection = clientConfig.GetConfigConnection();

            if (!connection.IsAssigned())
            {
                throw new Exception("connection not returned by configuration");
            }

            var config = ConnectionManagement.SetDatabaseSecurityInMemory(connection.UserName, connection.Password, connection.ConnectionName);

            var databaseProviderFactory = new DatabaseProviderFactory(config.GetSection);

            var database = databaseProviderFactory.Create("TestDatabase");

            // Act 
            Action action = () => SqlServerCheck.VerifyDatabaseServer(database, 1000); 

            // Assert
            action.Should().Throw<SqlClientException>().WithMessage("Database TestDatabase does not exist");
        }

        [TestMethod, TestCategory("SqlServer")]
        public void When_VerifyDatabaseServer_With_valid_database_Should_return_Ok()
        {
            // Arrange
            var di = DependencyInjectionFactory.ConfigureDependencyInjection();
            var clientConfig = di.GetService<IClientConfig>();

            if (!clientConfig.IsAssigned())
            {
                throw new Exception("clientconfig not returned by dependency injection");
            }

            clientConfig.DefaultDatabase = "TestClassGenerator";

            var connection = clientConfig.GetConfigConnection();

            if (!connection.IsAssigned())
            {
                throw new Exception("connection not returned by configuration");
            }

            var config = ConnectionManagement.SetDatabaseSecurityInMemory(connection.UserName, connection.Password, connection.ConnectionName);

            var databaseProviderFactory = new DatabaseProviderFactory(config.GetSection);

            var database = databaseProviderFactory.Create("TestClassGenerator");

            // Act 
            var result = SqlServerCheck.VerifyDatabaseServer(database, 1000);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod, TestCategory("SqlServer")]
        public void When_TryFindDatabase_With_valid_database_and_no_username_or_password_Should_return_Ok()
        {
            // Arrange
            var serverName = "konfidence2";
            var database = "TestClassGenerator";
            var userName = string.Empty;
            var password = string.Empty;

            // Act 
            Action action = () => SqlServerInstance.TryFindDatabase(serverName, database, userName, password);

            // Assert
            action.Should().Throw<SqlClientException>().WithMessage("no password or username provided");
        }

        [TestMethod, TestCategory("SqlServer")]
        public void When_VerifyDatabaseServer_With_inValid_sqlserver_Should_Return_database_server_not_found()
        {
            // Arrange
            var sqlServerName = "does_not_exists";
            var userName = "nobody";
            var password = "none";

            // Act 
            Func<Task> action = async () => await SqlServerInstance.VerifyDatabaseServer(sqlServerName, userName, password);

            // Assert
            action.Should().Throw<ConnectionFailureException>().WithMessage("Failed to connect to server does_not_exists.");
        }

        [TestMethod, TestCategory("SqlServer")]
        public void When_VerifyDatabaseServer_With_inValid_sqlserver_and_no_username_and_no_password_Should_Return_database_server_not_found()
        {
            // Arrange
            var sqlServerName = "does_not_exists";
            var userName = string.Empty;
            var password = string.Empty;

            // Act 
            Func<Task> action = async () => await SqlServerInstance.VerifyDatabaseServer(sqlServerName, userName, password);

            // Assert
            action.Should().Throw<ConnectionFailureException>().WithMessage("Failed to connect to server does_not_exists.");
        }
    }
}
