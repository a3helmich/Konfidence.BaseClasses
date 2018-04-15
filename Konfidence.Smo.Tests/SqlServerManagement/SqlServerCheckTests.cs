using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Konfidence.SqlHostProvider.Exceptions;
using Konfidence.SqlHostProvider.SqlServerManagement;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.Smo.Tests.SqlServerManagement
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class SqlServerCheckTests
    {
        [TestMethod, TestCategory("SqlServer")]
        public void VerifyDatabaseServer_WhenInvalidDatabase_ShouldReturnDoesNotExist()
        {
            // Arrange
            var databaseProviderFactory = new DatabaseProviderFactory();

            var database = databaseProviderFactory.Create("TestDatabase");

            // Act 
            Action action = () => SqlServerCheck.VerifyDatabaseServer(database); 

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
            var result = SqlServerCheck.VerifyDatabaseServer(database);

            // Assert
            Assert.IsTrue(result);
        }
    }
}
