﻿using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Konfidence.SqlHostProvider.SqlDbSchema;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.Smo.Tests.SqlDbSchema
{
    /// <summary>
    ///This is a test class for DatabaseStructureTest and is intended
    ///to contain all DatabaseStructureTest Unit Tests
    ///</summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class DatabaseStructureTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        [TestMethod, TestCategory("DatabaseStructure")]
        public void BuildStructureTest()
        {
            var target = new DatabaseStructure("TestClassGenerator"); // TODO: Initialize to an appropriate value

            target.BuildStructure();

            target.TableList.Should().HaveCount(5); // TestClassGenerator heeft nu 5 tabellen
        }

        [TestMethod, TestCategory("DatabaseStructure")]
        public void BuildStructureWithDifferentConnectionNameTest()
        {
            var target = new DatabaseStructure("SchemaDatabaseDevelopment"); // TODO: Initialize to an appropriate value

            target.BuildStructure();

            target.TableList.Should().HaveCount(5); // TestClassGenerator heeft nu 5 tabellen
        }
    }
}
