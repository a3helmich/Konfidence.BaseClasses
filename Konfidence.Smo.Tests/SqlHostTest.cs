using System;
using System.Data.Common;
using Konfidence.BaseData;
using Konfidence.Smo.SqlDbSchema;
using Konfidence.Smo.SqlServerManagement;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.Smo.Tests
{
    [TestClass]
    public class SqlHostTest
    {
        [TestMethod, TestCategory("SqlServer")]
        [ExpectedException(typeof(SqlHostException))]
        public void SqlServerNotFound()
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

                throw;
            }
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
