using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Konfidence.SqlHostProvider.SqlAccess;
using Konfidence.SqlHostProvider.SqlDbSchema;
using Konfidence.TestTools;
using Microsoft.Extensions.DependencyInjection;
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
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            File.Copy(@"ClientSettings.json", "CopyClientSettings.json", overwrite: true);
        }

        [TestInitialize]
        public void initialize()
        {
            SqlTestToolExtensions.CopySqlSettingsToActiveConfiguration();
        }

        [TestCleanup]
        public void Cleanup()
        {
            File.Copy(@"CopyClientSettings.json", "ClientSettings.json", overwrite: true);
        }

        [TestMethod, TestCategory("DatabaseStructure")]
        public void BuildStructureTest()
        {
            // arrange
            var target = new DatabaseStructure("TestClassGenerator"); // TODO: Initialize to an appropriate value

            // act
            target.BuildStructure();

            // assert
            target.Tables.Should().HaveCount(6); // TestClassGenerator heeft nu 6 tabellen
        }

        [TestMethod, TestCategory("DatabaseStructure")]
        public void BuildStructureWithDifferentConnectionNameTest()
        {
            // arrange
            var target = new DatabaseStructure("SchemaDatabaseDevelopment"); // TODO: Initialize to an appropriate value

            // act
            target.BuildStructure();

            // assert
            target.Tables.Should().HaveCount(6); // TestClassGenerator heeft nu 6 tabellen

            target.Tables.First(x => x.Name == "Test6").PrimaryKey.Should().Be("Test6Id");
        }

        [TestMethod, TestCategory("DatabaseStructure")]
        public void BuildStructureWithBlockedHackersConnectionName()
        {
            // arrange
            var target = new DatabaseStructure("BlockedHackers"); // TODO: Initialize to an appropriate value

            // act
            target.BuildStructure();

            // assert
            target.Tables.First(x => x.Name == "Blocked").PrimaryKey.Should().Be("BlockedId");
        }


        [TestMethod, TestCategory("DatabaseStructure")]
        public void BuildStructureWithDBMenuConnectionName()
        {
            // arrange
            IDatabaseStructure target = new DatabaseStructure("DBMenu"); // TODO: Initialize to an appropriate value

            // act
            target.BuildStructure();

            // assert
            target.Tables.First(x => x.Name == "TestInt").HasGuidId.Should().BeTrue();
        }


        [TestMethod]
        public void When_GetFields_executed_on_table_Should_return_a_string_with_all_ColumnNames_concatenated()
        {
            // arrange
            IDatabaseStructure target = new DatabaseStructure("SchemaDatabaseDevelopment"); // TODO: Initialize to an appropriate value

            target.BuildStructure();

            var table = target.Tables.First(x => x.Name == "Test5");
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
            IDatabaseStructure target = new DatabaseStructure("SchemaDatabaseDevelopment"); // TODO: Initialize to an appropriate value

            target.BuildStructure();

            var table = target.Tables.First(x => x.Name == "Test5");
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
            IDatabaseStructure target = new DatabaseStructure("SchemaDatabaseDevelopment"); // TODO: Initialize to an appropriate value

            target.BuildStructure();

            var table = target.Tables.First(x => x.Name == "Test5");
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
            IDatabaseStructure target = new DatabaseStructure("SchemaDatabaseDevelopment"); // TODO: Initialize to an appropriate value

            target.BuildStructure();

            var table = target.Tables.First(x => x.Name == "Test5");
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

        [TestMethod]
        public void When_TableExists_is_executed_and_table_exists_Should_return_true()
        {
            // arrange
            const string connectionName = "TestClassGenerator";

            var client = new SqlClient(connectionName);

            IDatabaseStructure target = new DatabaseStructure(connectionName); // TODO: Initialize to an appropriate value

            target.BuildStructure();

            // act
            var tableExists = client.TableExists("Test1");

            // assert
            tableExists.Should().BeTrue();
        }

        [TestMethod]
        public void When_TableExists_is_executed_and_table_does_notexists_Should_return_false()
        {
            // arrange
            const string connectionName = "TestClassGenerator";

            var client = new SqlClient(connectionName);

            IDatabaseStructure target = new DatabaseStructure(connectionName); // TODO: Initialize to an appropriate value

            target.BuildStructure();

            // act
            var tableExists = client.TableExists("Test666");

            // assert
            tableExists.Should().BeFalse();
        }

        [TestMethod]
        public void When_DependecyInjection_is_used_should_return_DatabaseStructure_Of_defaultDb()
        {
            // arrange
            var dependencyProvider = DependencyInjectionFactory.ConfigureDependencyInjection();

            // act
            var target = dependencyProvider.GetService<IDatabaseStructure>();

            // assert
            target.Should().NotBeNull();
            target.Should().BeOfType<DatabaseStructure>();
            target?.SelectedConnectionName.Should().Be("TestClassGenerator");
        }

        [TestMethod]
        public void When_DependecyInjection_is_used_With_DbMenu_Should_return_DatabaseStructure_of_DbMenu()
        {
            // arrange
            File.Copy(@"TestConfigurations\DbMenuClientSettings.json", "ClientSettings.json", overwrite: true);

            var dependencyProvider = DependencyInjectionFactory.ConfigureDependencyInjection();

            // act
            var target = dependencyProvider.GetService<IDatabaseStructure>();

            // assert
            target.Should().NotBeNull();
            target.Should().BeOfType<DatabaseStructure>();
            target?.SelectedConnectionName.Should().Be("DbMenu");
        }
    }
}
