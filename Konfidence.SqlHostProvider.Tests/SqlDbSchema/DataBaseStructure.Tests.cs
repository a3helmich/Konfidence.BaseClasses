using System.Linq;
using FluentAssertions;
using Konfidence.SqlHostProvider.SqlDbSchema;
using Konfidence.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.SqlHostProvider.Tests.SqlDbSchema
{
    /// <summary>
    ///This is a test class for DatabaseStructureTest and is intended
    ///to contain all DatabaseStructureTest Unit Tests
    ///</summary>
    [TestClass]
    public class DatabaseStructureTest
    {
        [TestInitialize]
        public void initialize()
        {
            SqlTestToolExtensions.CopySqlSettingsToActiveConfiguration();
        }

        [TestMethod, TestCategory("DatabaseStructure")]
        public void BuildStructureTest()
        {
            // arrange
            var target = new DatabaseStructure("TestClassGenerator"); // TODO: Initialize to an appropriate value

            // act
            target.BuildStructure();

            // assert
            target.TableList.Should().HaveCount(6); // TestClassGenerator heeft nu 6 tabellen
        }

        [TestMethod, TestCategory("DatabaseStructure")]
        public void BuildStructureWithDifferentConnectionNameTest()
        {
            // arrange
            var target = new DatabaseStructure("SchemaDatabaseDevelopment"); // TODO: Initialize to an appropriate value

            // act
            target.BuildStructure();

            // assert
            target.TableList.Should().HaveCount(6); // TestClassGenerator heeft nu 6 tabellen

            target.TableList.First(x => x.Name == "Test6").PrimaryKey.Should().Be("Test6Id");
        }

        [TestMethod, TestCategory("DatabaseStructure")]
        public void BuildStructureWithBlockedHackersConnectionName()
        {
            // arrange
            var target = new DatabaseStructure("BlockedHackers"); // TODO: Initialize to an appropriate value

            // act
            target.BuildStructure();

            // assert
            target.TableList.First(x => x.Name == "Blocked").PrimaryKey.Should().Be("BlockedId");
        }
    }
}
