using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Konfidence.SqlHostProvider.Exceptions;
using Konfidence.SqlHostProvider.SqlServerManagement;
using Konfidence.TestTools;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.SqlHostProvider.Tests.SqlServerManagement
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class SqlServerCheckTests
    {
        [TestInitialize]
        public void initialize()
        {
            SqlTestToolExtensions.CopySqlSettingsToActiveConfiguration();
        }

        [TestMethod, TestCategory("SqlServer")]
        public void VerifyDatabaseServer_WhenInvalidDatabase_ShouldReturnDoesNotExist()
        {
            // Arrange
            var databaseProviderFactory = new DatabaseProviderFactory();

            var database = databaseProviderFactory.Create("TestDatabase");

            // Act 
            Action action = () => SqlServerCheck.VerifyDatabaseServer(database, 10000); 

            // Assert
            action.Should().Throw<SqlClientException>().WithMessage("Database TestDatabase does not exist");
        }

        [TestMethod, TestCategory("SqlServer")]
        public void VerifyDatabaseServer_WhenValidDatabase_ShouldReturnOk()
        {
            // Arrange
            var databaseProviderFactory = new DatabaseProviderFactory();

            var database = databaseProviderFactory.Create("TestClassGenerator");

            // Act 
            var result = SqlServerCheck.VerifyDatabaseServer(database, 10000);

            // Assert
            Assert.IsTrue(result);
        }
    }
}
