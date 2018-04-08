﻿using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
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
        public void VerifyDatabaseServer_WhenInvalidDatabase_ShouldReturnDoesNotExist()
        {
            // Arrange
            var databaseProviderFactory = new DatabaseProviderFactory();

            var database = databaseProviderFactory.Create("TestDatabase");

            // Act 
            Action action = () => SqlServerCheck.VerifyDatabaseServer(database); 

            // Assert
            action.Should().Throw<SqlHostException>().WithMessage("Database TestDatabase does not exist");
        }

        [TestMethod, TestCategory("SqlServer")]
        public void VerifyDatabaseServer_WhenValidDatabase_ShouldReturnOk()
        {
            // Arrange
            var databaseProviderFactory = new DatabaseProviderFactory();

            var database = databaseProviderFactory.Create("Newsletter");

            // Act 
            var result = SqlServerCheck.VerifyDatabaseServer(database); 

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void SqlServerExists()
        {
            var target = new DatabaseStructure("Newsletter"); // TODO: Initialize to an appropriate value

            target.BuildStructure();

            Assert.AreEqual(25, target.TableList.Count); // newsletter heeft nu 25 tabellen

            //var tableDataItemList = new TableDataItemList("Newsletter");   // should exist
        }
    }
}