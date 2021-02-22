using System;
using System.Threading.Tasks;
using FluentAssertions;
using Konfidence.SqlHostProvider.Exceptions;
using Konfidence.SqlHostProvider.SqlServerManagement;
using Konfidence.TestTools;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.SqlServer.Management.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.SqlHostProvider.Tests.SqlServerManagement
{
    [TestClass]
    public class SqlServerCheckTests
    {
        [TestInitialize]
        public void initialize()
        {
            SqlTestToolExtensions.CopySqlSettingsToActiveConfiguration();
        }

        [TestMethod, TestCategory("SqlServer")]
        public void When_VerifyDatabaseServer_With_invalid_database_Should_return_DoesNotExist()
        {
            // Arrange
            var databaseProviderFactory = new DatabaseProviderFactory();

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
            var databaseProviderFactory = new DatabaseProviderFactory();

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
            var foundDatabase = SqlServerInstance.TryFindDatabase(serverName, database, userName, password);

            // Assert
            foundDatabase.Should().BeTrue();
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
