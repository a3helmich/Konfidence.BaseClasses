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

namespace Konfidence.SqlHostProvider.IntegrationTest.SqlDbSchema;

[TestClass, TestCategory("DatabaseStructure")]
public class DatabaseStructureTest
{
    [ClassInitialize]
    public static void ClassInitialize(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext _)
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
        // Arrange
        TestContext context = CreateContext("hMailServer");

        // Act
        context.Target.BuildStructure();

        // Assert
        context.Target.Tables.Should().HaveCount(34); // hmailserver contains 34 tables
    }

    [TestMethod]
    public void BuildStructure_TestClassGeneratorDatabase_GeneratesStructure()
    {
        // Arrange
        TestContext context = CreateContext("TestClassGenerator");

        // Act
        context.Target.BuildStructure();

        // Assert
        context.Target.Tables.Should().HaveCount(8); // TestClassGenerator has 7 tables
    }

    [TestMethod]
    public void BuildStructure_SchemaDatabaseDevelopmentConnection_GeneratesStructure()
    {
        // Arrange
        TestContext context = CreateContext("SchemaDatabaseDevelopment");

        // Act
        context.Target.BuildStructure();

        // Assert
        context.Target.Tables.Should().HaveCount(8); // TestClassGenerator heeft nu 6 tabellen

        context.Target.Tables.First(x => x.Name == "Test6").PrimaryKey.Should().Be("Test6Id");
    }

    [TestMethod]
    public void BuildStructure_BlockedHackersConnection_SetsPrimaryKey()
    {
        // Arrange
        TestContext context = CreateContext("BlockedHackers");

        // Act
        context.Target.BuildStructure();

        // Assert
        context.Target.Tables.First(x => x.Name == "Blocked").PrimaryKey.Should().Be("BlockedId");
    }

    [TestMethod]
    public void BuildStructure_DbMenuConnection_SetsHasGuidId()
    {
        // Arrange
        TestContext context = CreateContext("TestClassGenerator");

        // Act
        context.Target.BuildStructure();

        // Assert
        context.Target.Tables.First(x => x.Name == "TestInt").HasGuidId.Should().BeTrue();
    }

    [TestMethod]
    public void GetJoinedFieldNames_MultipleColumns_ReturnsConcatenatedColumnNames()
    {
        // Arrange
        TestContext context = CreateContext("SchemaDatabaseDevelopment");
        context.Target.BuildStructure();

        ITableDataItem table = context.Target.Tables.First(x => x.Name == "Test5");
        List<string> columnNameList = ["naam", "Omschrijving"];

        // Act
        string columnString = table.ColumnDataItems.GetJoinedFieldNames(columnNameList);

        // Assert
        columnString.Should().Be("NaamOmschrijving");
    }

    [TestMethod]
    public void GetJoinedUnderscoreFieldNames_MultipleColumns_ReturnsUnderscoreConcatenatedColumnNames()
    {
        // Arrange
        TestContext context = CreateContext("SchemaDatabaseDevelopment");
        context.Target.BuildStructure();

        ITableDataItem table = context.Target.Tables.First(x => x.Name == "Test5");
        List<string> columnNameList = ["naam", "Omschrijving"];

        // Act
        string columnString = table.ColumnDataItems.GetJoinedUnderscoreFieldNames(columnNameList);

        // Assert
        columnString.Should().Be("Naam_Omschrijving".ToUpperInvariant());
    }

    [TestMethod]
    public void GetFieldNamesAsArguments_MultipleColumns_ReturnsCommaSeparatedColumnNames()
    {
        // Arrange
        TestContext context = CreateContext("SchemaDatabaseDevelopment");
        context.Target.BuildStructure();

        ITableDataItem table = context.Target.Tables.First(x => x.Name == "Test5");
        List<string> columnNameList = ["naam", "Omschrijving"];

        // Act
        string columnString = table.ColumnDataItems.GetFieldNamesAsArguments(columnNameList);

        // Assert
        columnString.Should().Be("Naam, Omschrijving");
    }

    [TestMethod]
    public void GetFieldNamesAsParameters_MultipleColumns_ReturnsTypedParameterList()
    {
        // Arrange
        TestContext context = CreateContext("SchemaDatabaseDevelopment");
        context.Target.BuildStructure();

        ITableDataItem table = context.Target.Tables.First(x => x.Name == "Test5");
        List<string> columnNameList = ["naam", "Omschrijving"];

        // Act
        string columnString = table.ColumnDataItems.GetFieldNamesAsParameters(columnNameList);

        // Assert
        columnString.Should().Be("string naam, string omschrijving");
    }

    [TestMethod]
    public void GetFirstColumnName_MultipleColumns_ReturnsFirstName()
    {
        // Arrange
        List<string> columnNameList = ["naam", "Omschrijving"];

        // Act
        string columnString = columnNameList.Any() ? columnNameList.First() : string.Empty;

        // Assert
        columnString.Should().Be("naam");
    }

    [TestMethod]
    public void GetLastColumnName_MultipleColumns_ReturnsLastName()
    {
        // Arrange
        List<string> columnNameList = ["naam", "Omschrijving"];

        // Act
        string columnString = columnNameList.Any() ? columnNameList.Last() : string.Empty;

        // Assert
        columnString.Should().Be("Omschrijving");
    }

    [TestMethod]
    public void TableExists_ExistingTable_ReturnsTrue()
    {
        // Arrange
        TestContext context = CreateContext("TestClassGenerator");
        context.Target.BuildStructure();

        // Act
        bool tableExists = context.Client.TableExists("Test1");

        // Assert
        tableExists.Should().BeTrue();
    }

    [TestMethod]
    public void TableExists_NonExistentTable_ReturnsFalse()
    {
        // Arrange
        TestContext context = CreateContext("TestClassGenerator");
        context.Target.BuildStructure();

        // Act
        bool tableExists = context.Client.TableExists("Test666");

        // Assert
        tableExists.Should().BeFalse();
    }

    [TestMethod]
    public void ConfigureDependencyInjection_Always_RegistersDatabaseStructureForDefaultDb()
    {
        // Arrange
        IServiceProvider dependencyProvider = DependencyInjectionFactory.ConfigureDependencyInjection();

        // Act
        IDatabaseStructure? target = dependencyProvider.GetService<IDatabaseStructure>();

        // Assert
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
        // Arrange
        IServiceProvider dependencyProvider = DependencyInjectionFactory.ConfigureDependencyInjection($"{param}{delim}{value}");

        // Act
        IDatabaseStructure? target = dependencyProvider.GetService<IDatabaseStructure>();

        // Assert
        target.Should().NotBeNull();
        target.Should().BeOfType<DatabaseStructure>();
    }

    [TestMethod]
    public void ConfigureDependencyInjection_WithConfigFileFolderArgument_SetsConfigFileFolder()
    {
        // Arrange
        IServiceProvider dependencyProvider = DependencyInjectionFactory.ConfigureDependencyInjection(@"--ConfigFileFolder=some\location\");

        // Act
        IClientConfig? target = dependencyProvider.GetService<IClientConfig>();

        // Assert
        target.Should().NotBeNull();
        target.Should().BeOfType<ClientConfig>();
        target?.ConfigFileFolder.Should().Be(@"some\location\");
    }

    [TestMethod]
    public void ConfigureDependencyInjection_Always_SetsUseEnvironmentSettingToTrue()
    {
        // Arrange
        IServiceProvider dependencyProvider = DependencyInjectionFactory.ConfigureDependencyInjection();

        // Act
        IClientConfig? target = dependencyProvider.GetService<IClientConfig>();

        // Assert
        target.Should().NotBeNull();
        target.Should().BeOfType<ClientConfig>();
        target?.UseEnvironmentSetting.Should().BeTrue();
    }

    private sealed class TestContext
    {
        public TestContext(
            SqlClient Client,
            DatabaseStructure Target
        )
        {
            this.Client = Client;
            this.Target = Target;
        }

        public SqlClient Client { get; }

        public DatabaseStructure Target { get; }
    }

    private static TestContext CreateContext(string defaultDatabase)
    {
        IServiceProvider di = DependencyInjectionFactory.ConfigureDependencyInjection();
        IClientConfig? clientConfig = di.GetService<IClientConfig>();

        if (!clientConfig.IsAssigned())
        {
            throw new Exception("clientconfig not returned by dependency injection");
        }

        clientConfig.DefaultDatabase = defaultDatabase;

        SqlClient client = new(new SqlClientRepository(clientConfig));
        DatabaseStructure target = new(client);

        return new TestContext(client, target);
    }
}
