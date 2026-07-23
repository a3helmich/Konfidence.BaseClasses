using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using FluentAssertions;
using Konfidence.DatabaseInterface;
using Konfidence.SqlDataAccess;
using Konfidence.SqlHostProvider.SqlAccess;
using Konfidence.SqlHostProvider.SqlConnectionManagement;
using Konfidence.TestTools;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Konfidence.SqlHostProvider.UnitTest.SqlAccess;

[TestClass]
public class SqlClientRepositoryTests
{
    [TestMethod]
    public void BuildConnectionString_Always_MapsServerAndDatabaseToDataSourceAndInitialCatalog()
    {
        // Arrange
        TestContext context = CreateContext();

        // Act
        string connectionString = SqlClientRepository.BuildConnectionString(context.Connection);

        // Assert
        SqlConnectionStringBuilder builder = new(connectionString);

        builder.DataSource.Should().Be("konfidence2");
        builder.InitialCatalog.Should().Be("TestClassGenerator");
    }

    [TestMethod]
    public void BuildConnectionString_WithCredentials_UsesSqlAuthentication()
    {
        // Arrange
        TestContext context = CreateContext();
        context.Connection.UserName = "someuser";
        context.Connection.Password = "somepassword";

        // Act
        string connectionString = SqlClientRepository.BuildConnectionString(context.Connection);

        // Assert
        SqlConnectionStringBuilder builder = new(connectionString);

        builder.UserID.Should().Be("someuser");
        builder.Password.Should().Be("somepassword");
        builder.PersistSecurityInfo.Should().BeTrue();
        builder.IntegratedSecurity.Should().BeFalse();
    }

    [TestMethod]
    public void BuildConnectionString_WithoutCredentials_UsesIntegratedSecurity()
    {
        // Arrange
        TestContext context = CreateContext();

        // Act
        string connectionString = SqlClientRepository.BuildConnectionString(context.Connection);

        // Assert
        SqlConnectionStringBuilder builder = new(connectionString);

        builder.IntegratedSecurity.Should().BeTrue();
        builder.UserID.Should().BeEmpty();
        builder.Password.Should().BeEmpty();
    }

    [TestMethod]
    public void GetDatabase_WithNoMatchingConnection_FallsBackToAppConfigDefaultDatabase()
    {
        // Arrange
        SqlTestToolExtensions.CopySqlSettingsToActiveConfiguration();

        ClientConfig clientConfig = new(new ConfigurationBuilder().Build())
        {
            DefaultDatabase = "NonExistentConnectionName"
        };

        SqlClientRepository repository = new(clientConfig);

        // Act
        SqlDatabase database = repository.GetDatabase();

        // Assert
        DbConnection connection = database.CreateConnection();

        connection.ConnectionString.Should().Contain("TestClassGenerator");
    }

    [TestMethod]
    public void GetDatabase_WithNoMatchingConnectionAndNoAppConfigDefault_Throws()
    {
        // Arrange
        SqlTestToolExtensions.CopySqlSettingsToActiveConfiguration();
        ConnectionManagement.SetActiveConnection("NonExistentConnectionName");

        ClientConfig clientConfig = new(new ConfigurationBuilder().Build())
        {
            DefaultDatabase = "AlsoNonExistentConnectionName"
        };

        SqlClientRepository repository = new(clientConfig);

        try
        {
            // Act
            Action action = () => repository.GetDatabase();

            // Assert
            action.Should().Throw<InvalidOperationException>().WithMessage("No connection could be resolved*");
        }
        finally
        {
            SqlTestToolExtensions.CopySqlSettingsToActiveConfiguration();
        }
    }

    [TestMethod]
    public void ExecuteDeleteStoredProcedure_WithIdZero_ReturnsWithoutTouchingTheDatabase()
    {
        // Arrange
        ClientConfig clientConfig = new(new ConfigurationBuilder().Build());
        SqlClientRepository repository = new(clientConfig);
        FakeBaseDataItem dataItem = new();

        // Act
        Action action = () => repository.ExecuteDeleteStoredProcedure(dataItem);

        // Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    public void SetParameterData_Always_DoesNotClearTheCallerSuppliedList()
    {
        // Arrange
        Mock<ISpParameterData> parameterMock = new();
        parameterMock.Setup(x => x.ParameterName).Returns("Id");
        parameterMock.Setup(x => x.DbType).Returns(DbType.Int32);
        parameterMock.Setup(x => x.Value).Returns(1);

        List<ISpParameterData> parameterObjectList = [parameterMock.Object];

        SqlDatabase database = SqlDatabaseFactory.Create("Data Source=.;Initial Catalog=test;Integrated Security=True");

        using DbCommand dbCommand = database.GetStoredProcCommand("SomeProcedure");

        // Act
        SqlClientRepository.SetParameterData(parameterObjectList, database, dbCommand);

        // Assert
        // Before the fix, SetParameterData() cleared the caller-supplied list as an undocumented
        // side effect, so a list a caller intended to reuse across multiple calls would silently
        // end up empty.
        parameterObjectList.Should().HaveCount(1);
    }

    private sealed class FakeBaseDataItem : IBaseDataItem
    {
        public string GuidIdField { get; set; } = string.Empty;
        public string AutoIdField { get; set; } = string.Empty;
        public System.Collections.Generic.Dictionary<string, ISpParameterData> AutoUpdateFieldDictionary { get; } = [];
        public string GetStoredProcedure { get; set; } = string.Empty;
        public string DeleteStoredProcedure { get; set; } = string.Empty;
        public string SaveStoredProcedure { get; set; } = string.Empty;
        public string GetByGuidStoredProcedure { get; set; } = string.Empty;
        public void InitializeDataItem() { }
        public System.Collections.Generic.List<ISpParameterData> SetItemData() => [];
        public void Save() { }
        public void Delete() { }
        public int GetId() => 0;
        public void SetId(int id) { }
        public void GetKey(IDataReader dataReader) { }
        public void GetData(IDataReader dataReader) { }
        public System.Collections.Generic.List<ISpParameterData> GetParameterObjects() => [];
        public bool IsNew => true;
    }

    private sealed class TestContext
    {
        public TestContext(
            ConfigConnectionString Connection
        )
        {
            this.Connection = Connection;
        }

        public ConfigConnectionString Connection { get; }
    }

    private static TestContext CreateContext()
    {
        ConfigConnectionString connection = new()
        {
            Server = "konfidence2",
            Database = "TestClassGenerator",
            ConnectionName = "TestClassGenerator"
        };

        return new TestContext(connection);
    }
}
