using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Konfidence.SqlHostProvider.SqlDbSchema;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestExtensionMethods;

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
        [TestInitialize]
        public void initialize()
        {
            TestExtensions.CopySqlSettingsToActiveConfiguration();
        }

        [TestMethod, TestCategory("DatabaseStructure")]
        public void BuildStructureTest()
        {
            // arrange
            var target = new DatabaseStructure("TestClassGenerator"); // TODO: Initialize to an appropriate value

            // act
            target.BuildStructure();

            // assert
            target.TableList.Should().HaveCount(5); // TestClassGenerator heeft nu 5 tabellen
        }

        [TestMethod, TestCategory("DatabaseStructure")]
        public void BuildStructureWithDifferentConnectionNameTest()
        {
            // arrange
            var target = new DatabaseStructure("SchemaDatabaseDevelopment"); // TODO: Initialize to an appropriate value

            // act
            target.BuildStructure();

            // assert
            target.TableList.Should().HaveCount(5); // TestClassGenerator heeft nu 5 tabellen
        }
    }
}
