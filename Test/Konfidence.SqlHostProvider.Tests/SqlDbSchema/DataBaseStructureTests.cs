using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Konfidence.Base;
using Konfidence.SqlHostProvider.SqlAccess;
using Konfidence.SqlHostProvider.SqlDbSchema;
using Konfidence.TestTools;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.SqlHostProvider.Tests.SqlDbSchema
{
    [TestClass, TestCategory("DatabaseStructure")]
    public class DatabaseStructureTest
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext _)
        {
            SqlTestToolExtensions.CopySqlSettingsToActiveConfiguration();

            SqlTestToolExtensions.CopySqlSecurityToActiveConfiguration("TestClassGenerator");
            SqlTestToolExtensions.CopySqlSecurityToActiveConfiguration("SchemaDatabaseDevelopment");
            SqlTestToolExtensions.CopySqlSecurityToActiveConfiguration("BlockedHackers");
            SqlTestToolExtensions.CopySqlSecurityToActiveConfiguration("DbMenu");
            SqlTestToolExtensions.CopySqlSecurityToActiveConfiguration("hMailServer");
        }

        [TestMethod]
        public void When_BuildStructure_of_hMailServer_Should_generate_structure()
        {
            // arrange
            IServiceProvider di = DependencyInjectionFactory.ConfigureDependencyInjection();
            IClientConfig? clientConfig = di.GetService<IClientConfig>();

            if (!clientConfig.IsAssigned())
            {
                throw new Exception("clientconfig not returned by dependency injection");
            }

            clientConfig.DefaultDatabase = "hMailServer";

            SqlClient client = new(new SqlClientRepository(clientConfig));

            IDatabaseStructure target = new DatabaseStructure(client);

            // act
            target.BuildStructure();

            // assert
            target.Tables.Should().HaveCount(34); // hmailserver contains 34 tables
        }

        [TestMethod]
        public void BuildStructureTest()
        {
            // arrange
            IServiceProvider di = DependencyInjectionFactory.ConfigureDependencyInjection();
            IClientConfig? clientConfig = di.GetService<IClientConfig>();

            if (!clientConfig.IsAssigned())
            {
                throw new Exception("clientconfig not returned by dependency injection");
            }

            clientConfig.DefaultDatabase = "TestClassGenerator";

            SqlClient client = new(new SqlClientRepository(clientConfig));

            IDatabaseStructure target = new DatabaseStructure(client);

            // act
            target.BuildStructure();

            // assert
            target.Tables.Should().HaveCount(8); // TestClassGenerator has 7 tables
        }

        [TestMethod]
        public void BuildStructureWithDifferentConnectionNameTest()
        {
            // arrange
            IServiceProvider di = DependencyInjectionFactory.ConfigureDependencyInjection();
            IClientConfig? clientConfig = di.GetService<IClientConfig>();

            if (!clientConfig.IsAssigned())
            {
                throw new Exception("clientconfig not returned by dependency injection");
            }

            clientConfig.DefaultDatabase = "SchemaDatabaseDevelopment";

            SqlClient client = new(new SqlClientRepository(clientConfig));

            IDatabaseStructure target = new DatabaseStructure(client);

            // act
            target.BuildStructure();

            // assert
            target.Tables.Should().HaveCount(8); // TestClassGenerator heeft nu 6 tabellen

            target.Tables.First(x => x.Name == "Test6").PrimaryKey.Should().Be("Test6Id");
        }

        [TestMethod]
        public void BuildStructureWithBlockedHackersConnectionName()
        {
            // arrange
            IServiceProvider di = DependencyInjectionFactory.ConfigureDependencyInjection();
            IClientConfig? clientConfig = di.GetService<IClientConfig>();

            if (!clientConfig.IsAssigned())
            {
                throw new Exception("clientconfig not returned by dependency injection");
            }

            clientConfig.DefaultDatabase = "BlockedHackers";

            SqlClient client = new(new SqlClientRepository(clientConfig));

            IDatabaseStructure target = new DatabaseStructure(client);

            // act
            target.BuildStructure();

            // assert
            target.Tables.First(x => x.Name == "Blocked").PrimaryKey.Should().Be("BlockedId");
        }

        [TestMethod]
        public void BuildStructureWithDBMenuConnectionName()
        {
            // arrange
            IServiceProvider di = DependencyInjectionFactory.ConfigureDependencyInjection();
            IClientConfig? clientConfig = di.GetService<IClientConfig>();

            if (!clientConfig.IsAssigned())
            {
                throw new Exception("clientconfig not returned by dependency injection");
            }

            clientConfig.DefaultDatabase = "TestClassGenerator";

            SqlClient client = new(new SqlClientRepository(clientConfig));

            IDatabaseStructure target = new DatabaseStructure(client);

            // act
            target.BuildStructure();

            // assert
            target.Tables.First(x => x.Name == "TestInt").HasGuidId.Should().BeTrue();
        }

        [TestMethod]
        public void When_GetFields_executed_on_table_Should_return_a_string_with_all_ColumnNames_concatenated()
        {
            // arrange
            IServiceProvider di = DependencyInjectionFactory.ConfigureDependencyInjection();
            IClientConfig? clientConfig = di.GetService<IClientConfig>();

            if (!clientConfig.IsAssigned())
            {
                throw new Exception("clientconfig not returned by dependency injection");
            }

            clientConfig.DefaultDatabase = "SchemaDatabaseDevelopment";

            SqlClient client = new(new SqlClientRepository(clientConfig));

            IDatabaseStructure target = new DatabaseStructure(client);

            target.BuildStructure();

            ITableDataItem table = target.Tables.First(x => x.Name == "Test5");
            List<string> columnNameList = ["naam", "Omschrijving"];

            // act
            string columnString = table.ColumnDataItems.GetJoinedFieldNames(columnNameList);

            // assert
            columnString.Should().Be("NaamOmschrijving");
        }

        [TestMethod]
        public void When_GetUnderscoreFields_executed_on_table_Should_return_a_string_with_all_ColumnNames_concatenated()
        {
            // arrange
            IServiceProvider di = DependencyInjectionFactory.ConfigureDependencyInjection();
            IClientConfig? clientConfig = di.GetService<IClientConfig>();

            if (!clientConfig.IsAssigned())
            {
                throw new Exception("clientconfig not returned by dependency injection");
            }

            clientConfig.DefaultDatabase = "SchemaDatabaseDevelopment";

            SqlClient client = new(new SqlClientRepository(clientConfig));

            IDatabaseStructure target = new DatabaseStructure(client);

            target.BuildStructure();

            ITableDataItem table = target.Tables.First(x => x.Name == "Test5");
            List<string> columnNameList = ["naam", "Omschrijving"];

            // act
            string columnString = table.ColumnDataItems.GetJoinedUnderscoreFieldNames(columnNameList);

            // assert
            columnString.Should().Be("Naam_Omschrijving".ToUpperInvariant());
        }

        [TestMethod]
        public void When_GetCommaFields_executed_on_table_Should_return_a_string_with_all_ColumnNames_concatenated()
        {
            // arrange
            IServiceProvider di = DependencyInjectionFactory.ConfigureDependencyInjection();
            IClientConfig? clientConfig = di.GetService<IClientConfig>();

            if (!clientConfig.IsAssigned())
            {
                throw new Exception("clientconfig not returned by dependency injection");
            }

            clientConfig.DefaultDatabase = "SchemaDatabaseDevelopment";

            SqlClient client = new(new SqlClientRepository(clientConfig));

            IDatabaseStructure target = new DatabaseStructure(client);

            target.BuildStructure();

            ITableDataItem table = target.Tables.First(x => x.Name == "Test5");
            List<string> columnNameList = ["naam", "Omschrijving"];

            // act
            string columnString = table.ColumnDataItems.GetFieldNamesAsArguments(columnNameList);

            // assert
            columnString.Should().Be("Naam, Omschrijving");
        }

        [TestMethod]
        public void When_GetTypedCommaFields_executed_on_table_Should_return_a_string_with_all_ColumnNames_concatenated()
        {
            // arrange
            IServiceProvider di = DependencyInjectionFactory.ConfigureDependencyInjection();
            IClientConfig? clientConfig = di.GetService<IClientConfig>();

            if (!clientConfig.IsAssigned())
            {
                throw new Exception("clientconfig not returned by dependency injection");
            }

            clientConfig.DefaultDatabase = "SchemaDatabaseDevelopment";

            SqlClient client = new(new SqlClientRepository(clientConfig));

            IDatabaseStructure target = new DatabaseStructure(client);

            target.BuildStructure();

            ITableDataItem table = target.Tables.First(x => x.Name == "Test5");
            List<string> columnNameList = ["naam", "Omschrijving"];

            // act
            string columnString = table.ColumnDataItems.GetFieldNamesAsParameters(columnNameList);

            // assert
            columnString.Should().Be("string naam, string omschrijving");
        }

        [TestMethod]
        public void When_GetFirstField_executed_on_table_Should_return_a_string_with_all_ColumnNames_concatenated()
        {
            // arrange
            List<string> columnNameList = ["naam", "Omschrijving"];

            // act
            string columnString = columnNameList.Any() ? columnNameList.First() : string.Empty;

            // assert
            columnString.Should().Be("naam");
        }

        [TestMethod]
        public void When_GetLastField_executed_on_table_Should_return_a_string_with_all_ColumnNames_concatenated()
        {
            // arrange
            List<string> columnNameList = ["naam", "Omschrijving"];

            // act
            string columnString = columnNameList.Any() ? columnNameList.Last() : string.Empty;

            // assert
            columnString.Should().Be("Omschrijving");
        }

        [TestMethod]
        public void When_TableExists_is_executed_and_table_exists_Should_return_true()
        {
            // arrange
            IServiceProvider di = DependencyInjectionFactory.ConfigureDependencyInjection();
            IClientConfig? clientConfig = di.GetService<IClientConfig>();

            if (!clientConfig.IsAssigned())
            {
                throw new Exception("clientconfig not returned by dependency injection");
            }

            clientConfig.DefaultDatabase = "TestClassGenerator";

            SqlClient client = new(new SqlClientRepository(clientConfig));

            IDatabaseStructure target = new DatabaseStructure(client);

            target.BuildStructure();

            // act
            bool tableExists = client.TableExists("Test1");

            // assert
            tableExists.Should().BeTrue();
        }

        [TestMethod]
        public void When_TableExists_is_executed_and_table_does_notexists_Should_return_false()
        {
            // arrange
            IServiceProvider di = DependencyInjectionFactory.ConfigureDependencyInjection();
            IClientConfig? clientConfig = di.GetService<IClientConfig>();

            if (!clientConfig.IsAssigned())
            {
                throw new Exception("clientconfig not returned by dependency injection");
            }

            clientConfig.DefaultDatabase = "TestClassGenerator";

            SqlClient client = new(new SqlClientRepository(clientConfig));

            IDatabaseStructure target = new DatabaseStructure(client);

            target.BuildStructure();

            // act
            bool tableExists = client.TableExists("Test666");

            // assert
            tableExists.Should().BeFalse();
        }

        [TestMethod]
        public void When_DependecyInjection_is_used_should_return_DatabaseStructure_Of_defaultDb()
        {
            // arrange
            IServiceProvider dependencyProvider = DependencyInjectionFactory.ConfigureDependencyInjection();

            // act
            IDatabaseStructure? target = dependencyProvider.GetService<IDatabaseStructure>();

            // assert
            target.Should().NotBeNull();
            target.Should().BeOfType<DatabaseStructure>();
        }

        [TestMethod]
        [DataRow("--defaultdatabase", "=", "DbMenu")]
        [DataRow("--DefaultDatabase", "=", "DbMenu")]
        [DataRow("--DefaultDatabase", " = ", "DbMenu")]
        [DataRow("--DefaultDatabase", " =", "DbMenu")]
        [DataRow("--DefaultDatabase", "= ", "DbMenu")]
        [DataRow("--DefaultDatabase", ":", "DbMenu")]
        [DataRow("--DefaultDatabase", " : ", "DbMenu")]
        [DataRow("--DefaultDatabase", " ", "DbMenu")]
        [DataRow("--DefaultDatabase", "   ", "DbMenu")]
        public void When_DependecyInjection_is_used_With_DbMenu_Should_return_DatabaseStructure_of_DbMenu(string param, string delim, string value)
        {
            // arrange
            IServiceProvider dependencyProvider = DependencyInjectionFactory.ConfigureDependencyInjection($"{param}{delim}{value}");

            // act
            IDatabaseStructure? target = dependencyProvider.GetService<IDatabaseStructure>();

            // assert
            target.Should().NotBeNull();
            target.Should().BeOfType<DatabaseStructure>();
        }

        [TestMethod]
        public void When_DependecyInjection_is_used_should_return_commandlinearguments()
        {
            // arrange
            IServiceProvider dependencyProvider = DependencyInjectionFactory.ConfigureDependencyInjection(@"--ConfigFileFolder=some\location\");

            // act
            IClientConfig? target = dependencyProvider.GetService<IClientConfig>();

            // assert
            target.Should().NotBeNull();
            target.Should().BeOfType<ClientConfig>();
            target?.ConfigFileFolder.Should().Be(@"some\location\");
        }

        [TestMethod]
        public void When_DependecyInjection_is_used_should_have_UseEnvironmentSetting_set_to_true()
        {
            // arrange
            IServiceProvider dependencyProvider = DependencyInjectionFactory.ConfigureDependencyInjection();

            // act
            IClientConfig? target = dependencyProvider.GetService<IClientConfig>();

            // assert
            target.Should().NotBeNull();
            target.Should().BeOfType<ClientConfig>();
            target?.UseEnvironmentSetting.Should().BeTrue();
        }
    }
}
