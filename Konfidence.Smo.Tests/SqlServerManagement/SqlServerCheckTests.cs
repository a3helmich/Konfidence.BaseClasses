using System;
using System.Diagnostics.CodeAnalysis;
using Konfidence.BaseData;
using Konfidence.Smo.SqlDbSchema;
using Konfidence.Smo.SqlServerManagement;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.Smo.Tests.SqlServerManagement
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class SqlServerCheckTests
    {
        [TestMethod, TestCategory("SqlServer")]
        [ExpectedException(typeof(SqlHostException))]
        public void VerifyDatabaseServer_WhenInvalidDatabase_ShouldReturnDoesNotExist()
        {
            // Arrange
            var databaseProviderFactory = new DatabaseProviderFactory();

            var database = databaseProviderFactory.Create("TestDatabase");

            // Act 
            try
            {
                SqlServerCheck.VerifyDatabaseServer(database); // "TestDatabase" does not exist, and throws exception SqlHostException
            }
            catch (Exception e)
            {
                // Assert
                Assert.AreEqual("Database TestDatabase does not exist", e.Message);

                throw; // [ExpectedException(typeof(SqlHostException))] 
            }
        }

        [TestMethod, TestCategory("SqlServer")]
        public void VerifyDatabaseServer_WhenValidDatabase_ShouldReturnOk()
        {
            // Arrange
            var databaseProviderFactory = new DatabaseProviderFactory();

            var database = databaseProviderFactory.Create("Newsletter");

            // Act 
            var result = SqlServerCheck.VerifyDatabaseServer(database); // "TestDatabase" does not exist, and throws exception SqlHostException

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void SqlServerExists()
        {
            DatabaseStructure target = new DatabaseStructure("Newsletter"); // TODO: Initialize to an appropriate value

            target.BuildStructure();

            Assert.AreEqual(25, target.TableList.Count); // newsletter heeft nu 25 tabellen

            //var tableDataItemList = new TableDataItemList("Newsletter");   // should exist
        }
    }
}
