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

namespace Konfidence.SqlHostProvider.IntegrationTest.SqlDbSchema
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
        public void BuildStructure_HMailServerDatabase_GeneratesStructure()
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

            DatabaseStructure target = new(client);

            // act
            target.BuildStructure();

            // assert
            target.Tables.Should().HaveCount(34); // hmailserver contains 34 tables
        }

        [TestMethod]
        public void BuildStructure_TestClassGeneratorDatabase_GeneratesStructure()
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

            DatabaseStructure target = new(client);

            // act
            target.BuildStructure();

            // assert
            target.Tables.Should().HaveCount(8); // TestClassGenerator has 7 tables
        }

        [TestMethod]
        public void BuildStructure_SchemaDatabaseDevelopmentConnection_GeneratesStructure()
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

            DatabaseStructure target = new(client);

            // act
            target.BuildStructure();

            // assert
            target.Tables.Should().HaveCount(8); // TestClassGenerator heeft nu 6 tabellen

            target.Tables.First(x => x.Name == "Test6").PrimaryKey.Should().Be("Test6Id");
        }

        [TestMethod]
        public void BuildStructure_BlockedHackersConnection_SetsPrimaryKey()
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

            DatabaseStructure target = new(client);

            // act
            target.BuildStructure();

            // assert
            target.Tables.First(x => x.Name == "Blocked").PrimaryKey.Should().Be("BlockedId");
        }

        [TestMethod]
        public void BuildStructure_DbMenuConnection_SetsHasGuidId()
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

            DatabaseStructure target = new(client);

            // act
            target.BuildStructure();

            // assert
            target.Tables.First(x => x.Name == "TestInt").HasGuidId.Should().BeTrue();
        }

        [TestMethod]
        public void GetJoinedFieldNames_MultipleColumns_ReturnsConcatenatedColumnNames()
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

            DatabaseStructure target = new(client);

            target.BuildStructure();

            ITableDataItem table = target.Tables.First(x => x.Name == "Test5");
            List<string> columnNameList = ["naam", "Omschrijving"];

            // act
            string columnString = table.ColumnDataItems.GetJoinedFieldNames(columnNameList);

            // assert
            columnString.Should().Be("NaamOmschrijving");
        }

        [TestMethod]
        public void GetJoinedUnderscoreFieldNames_MultipleColumns_ReturnsUnderscoreConcatenatedColumnNames()
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

            DatabaseStructure target = new(client);

            target.BuildStructure();

            ITableDataItem table = target.Tables.First(x => x.Name == "Test5");
            List<string> columnNameList = ["naam", "Omschrijving"];

            // act
            string columnString = table.ColumnDataItems.GetJoinedUnderscoreFieldNames(columnNameList);

            // assert
            columnString.Should().Be("Naam_Omschrijving".ToUpperInvariant());
        }

        [TestMethod]
        public void GetFieldNamesAsArguments_MultipleColumns_ReturnsCommaSeparatedColumnNames()
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

            DatabaseStructure target = new(client);

            target.BuildStructure();

            ITableDataItem table = target.Tables.First(x => x.Name == "Test5");
            List<string> columnNameList = ["naam", "Omschrijving"];

            // act
            string columnString = table.ColumnDataItems.GetFieldNamesAsArguments(columnNameList);

            // assert
            columnString.Should().Be("Naam, Omschrijving");
        }

        [TestMethod]
        public void GetFieldNamesAsParameters_MultipleColumns_ReturnsTypedParameterList()
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

            DatabaseStructure target = new(client);

            target.BuildStructure();

            ITableDataItem table = target.Tables.First(x => x.Name == "Test5");
            List<string> columnNameList = ["naam", "Omschrijving"];

            // act
            string columnString = table.ColumnDataItems.GetFieldNamesAsParameters(columnNameList);

            // assert
            columnString.Should().Be("string naam, string omschrijving");
        }

        [TestMethod]
        public void GetFirstColumnName_MultipleColumns_ReturnsFirstName()
        {
            // arrange
            List<string> columnNameList = ["naam", "Omschrijving"];

            // act
            string columnString = columnNameList.Any() ? columnNameList.First() : string.Empty;

            // assert
            columnString.Should().Be("naam");
        }

        [TestMethod]
        public void GetLastColumnName_MultipleColumns_ReturnsLastName()
        {
            // arrange
            List<string> columnNameList = ["naam", "Omschrijving"];

            // act
            string columnString = columnNameList.Any() ? columnNameList.Last() : string.Empty;

            // assert
            columnString.Should().Be("Omschrijving");
        }

        [TestMethod]
        public void TableExists_ExistingTable_ReturnsTrue()
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

            DatabaseStructure target = new(client);

            target.BuildStructure();

            // act
            bool tableExists = client.TableExists("Test1");

            // assert
            tableExists.Should().BeTrue();
        }

        [TestMethod]
        public void TableExists_NonExistentTable_ReturnsFalse()
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

            DatabaseStructure target = new(client);

            target.BuildStructure();

            // act
            bool tableExists = client.TableExists("Test666");

            // assert
            tableExists.Should().BeFalse();
        }

        [TestMethod]
        public void ConfigureDependencyInjection_Always_RegistersDatabaseStructureForDefaultDb()
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
        public void ConfigureDependencyInjection_WithDbMenuArgument_RegistersDatabaseStructureForDbMenu(string param, string delim, string value)
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
        public void ConfigureDependencyInjection_WithConfigFileFolderArgument_SetsConfigFileFolder()
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
        public void ConfigureDependencyInjection_Always_SetsUseEnvironmentSettingToTrue()
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
