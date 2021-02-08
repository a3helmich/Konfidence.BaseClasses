using System.Collections.Generic;
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

        [TestMethod]
        public void When_GetFields_executed_on_table_Should_return_a_string_with_all_ColumnNames_concatenated()
        {
            // arrange
            var target = new DatabaseStructure("SchemaDatabaseDevelopment"); // TODO: Initialize to an appropriate value

            target.BuildStructure();

            var table = target.TableList.First(x => x.Name == "Test5");
            var columnNameList = new List<string> { "naam", "Omschrijving" };

            // act
            var columnString = table.ColumnDataItems.GetFieldNames(columnNameList);

            // assert
            columnString.Should().Be("NaamOmschrijving");
        }

        [TestMethod]
        public void When_GetUnderscoreFields_executed_on_table_Should_return_a_string_with_all_ColumnNames_concatenated()
        {
            // arrange
            var target = new DatabaseStructure("SchemaDatabaseDevelopment"); // TODO: Initialize to an appropriate value

            target.BuildStructure();

            var table = target.TableList.First(x => x.Name == "Test5");
            var columnNameList = new List<string> { "naam", "Omschrijving" };

            // act
            var columnString = table.ColumnDataItems.GetUnderscoreFieldNames(columnNameList);

            // assert
            columnString.Should().Be("Naam_Omschrijving".ToUpperInvariant());
        }

        [TestMethod]
        public void When_GetCommaFields_executed_on_table_Should_return_a_string_with_all_ColumnNames_concatenated()
        {
            // arrange
            var target = new DatabaseStructure("SchemaDatabaseDevelopment"); // TODO: Initialize to an appropriate value

            target.BuildStructure();

            var table = target.TableList.First(x => x.Name == "Test5");
            var columnNameList = new List<string> { "naam", "Omschrijving" };

            // act
            var columnString = table.ColumnDataItems.GetCommaFieldNames(columnNameList);

            // assert
            columnString.Should().Be("Naam, Omschrijving");
        }

        [TestMethod]
        public void When_GetTypedCommaFields_executed_on_table_Should_return_a_string_with_all_ColumnNames_concatenated()
        {
            // arrange
            var target = new DatabaseStructure("SchemaDatabaseDevelopment"); // TODO: Initialize to an appropriate value

            target.BuildStructure();

            var table = target.TableList.First(x => x.Name == "Test5");
            var columnNameList = new List<string> { "naam", "Omschrijving" };

            // act
            var columnString = table.ColumnDataItems.GetTypedCommaFieldNames(columnNameList);

            // assert
            columnString.Should().Be("string naam, string omschrijving");
        }

        [TestMethod]
        public void When_GetFirstField_executed_on_table_Should_return_a_string_with_all_ColumnNames_concatenated()
        {
            // arrange
            var columnNameList = new List<string> { "naam", "Omschrijving" };

            // act
            var columnString = columnNameList.Any() ? columnNameList.First() : string.Empty;

            // assert
            columnString.Should().Be("naam");
        }

        [TestMethod]
        public void When_GetLastField_executed_on_table_Should_return_a_string_with_all_ColumnNames_concatenated()
        {
            // arrange
            var columnNameList = new List<string> { "naam", "Omschrijving" };

            // act
            var columnString = columnNameList.Any() ? columnNameList.Last() : string.Empty;

            // assert
            columnString.Should().Be("Omschrijving");
        }
    }
}
