using System;
using System.Collections.Generic;
using System.Data;
using FluentAssertions;
using Konfidence.DatabaseInterface;
using Konfidence.SqlHostProvider.SqlAccess;
using Konfidence.TestTools;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.SqlHostProvider.IntegrationTest.SqlAccess;

/// <summary>
/// Covers the stored-procedure execution paths of <see cref="SqlClientRepository"/> against the real
/// TestClassGenerator database. These are the save/get/get-by/get-list/delete round trips, plus the
/// parameter marshalling either side of them - none of which the other test fixtures reach, because
/// they all stop at connection-string building and schema queries.
/// </summary>
[TestClass]
public class SqlClientRepositoryCrudTests
{
    [TestMethod]
    public void ExecuteSaveStoredProcedure_ForNewItem_AssignsIdAndFillsAutoUpdateFields()
    {
        // Arrange
        // The save path is the only one that runs SetParameterData(IBaseDataItem, ...) and
        // GetParameterData(...) - the InputOutput parameter marshalling that writes the generated
        // identity and the database-side columns back onto the item.
        TestContext context = CreateContext();
        TestIntDataItem dataItem = CreateDataItem(testInt: 4242);

        try
        {
            // Act
            context.Repository.ExecuteSaveStoredProcedure(dataItem);

            // Assert
            dataItem.GetId().Should().BeGreaterThan(0);
            dataItem.IsNew.Should().BeFalse();

            dataItem.AutoUpdateFieldDictionary["TestIntId"].Value.Should().BeOfType<Guid>()
                .Which.Should().NotBe(Guid.Empty);
            dataItem.AutoUpdateFieldDictionary["SysInsertTime"].Value.Should().BeOfType<DateTime>();
        }
        finally
        {
            DeleteRow(context, dataItem);
        }
    }

    [TestMethod]
    public void ExecuteGetStoredProcedure_ForSavedItem_ReadsTheRowBack()
    {
        // Arrange
        TestContext context = CreateContext();
        TestIntDataItem saved = CreateDataItem(testInt: 4243);

        context.Repository.ExecuteSaveStoredProcedure(saved);

        try
        {
            TestIntDataItem fetched = new();
            fetched.SetId(saved.GetId());

            // Act
            context.Repository.ExecuteGetStoredProcedure(fetched);

            // Assert
            fetched.GetId().Should().Be(saved.GetId());
            fetched.TestIntValue.Should().Be(4243);
        }
        finally
        {
            DeleteRow(context, saved);
        }
    }

    [TestMethod]
    public void ExecuteGetStoredProcedure_ForMissingRow_LeavesTheItemUntouched()
    {
        // Arrange
        // The reader's "no rows" path skips GetKey/GetData entirely, so the item keeps whatever it
        // already held rather than being partially overwritten.
        TestContext context = CreateContext();
        TestIntDataItem dataItem = new();
        dataItem.SetId(int.MaxValue);

        // Act
        context.Repository.ExecuteGetStoredProcedure(dataItem);

        // Assert
        dataItem.TestIntValue.Should().Be(0);
        dataItem.ReadCount.Should().Be(0);
    }

    [TestMethod]
    public void ExecuteGetByStoredProcedure_UsingTheGuidProcedure_ReadsTheRowBack()
    {
        // Arrange
        TestContext context = CreateContext();
        TestIntDataItem saved = CreateDataItem(testInt: 4244);

        context.Repository.ExecuteSaveStoredProcedure(saved);

        try
        {
            Guid rowGuid = (Guid)saved.AutoUpdateFieldDictionary["TestIntId"].Value!;

            TestIntDataItem fetched = new();
            fetched.SetGuidParameter(rowGuid);

            // Act
            context.Repository.ExecuteGetByStoredProcedure(fetched, "gen_TestInt_GetRowByGuid");

            // Assert
            fetched.GetId().Should().Be(saved.GetId());
            fetched.TestIntValue.Should().Be(4244);
        }
        finally
        {
            DeleteRow(context, saved);
        }
    }

    [TestMethod]
    public void ExecuteGetListStoredProcedure_WithoutParameters_ReturnsEveryRow()
    {
        // Arrange
        // The parameterless overload delegates to the four-argument one with an empty parameter
        // list, so it is a distinct entry point from the one the other list test uses.
        TestContext context = CreateContext();
        TestIntDataItem saved = CreateDataItem(testInt: 4245);

        context.Repository.ExecuteSaveStoredProcedure(saved);

        try
        {
            List<TestIntDataItem> items = [];

            // Act
            context.Repository.ExecuteGetListStoredProcedure(items, "gen_TestInt_GetList", context.BaseClient);

            // Assert
            items.Should().NotBeEmpty();
            items.Should().Contain(x => x.GetId() == saved.GetId());
        }
        finally
        {
            DeleteRow(context, saved);
        }
    }

    [TestMethod]
    public void ExecuteGetListStoredProcedure_WithExplicitParameterList_ReturnsEveryRow()
    {
        // Arrange
        TestContext context = CreateContext();
        TestIntDataItem saved = CreateDataItem(testInt: 4246);

        context.Repository.ExecuteSaveStoredProcedure(saved);

        try
        {
            List<TestIntDataItem> items = [];

            // Act
            context.Repository.ExecuteGetListStoredProcedure(items, "gen_TestInt_GetList", new List<ISpParameterData>(), context.BaseClient);

            // Assert
            items.Should().Contain(x => x.GetId() == saved.GetId());
        }
        finally
        {
            DeleteRow(context, saved);
        }
    }

    [TestMethod]
    public void ExecuteDeleteStoredProcedure_WithRealId_RemovesTheRow()
    {
        // Arrange
        // The id-zero guard already had a test; this covers the other side of it, where the delete
        // actually reaches the database.
        TestContext context = CreateContext();
        TestIntDataItem saved = CreateDataItem(testInt: 4247);

        context.Repository.ExecuteSaveStoredProcedure(saved);

        int savedId = saved.GetId();

        // Act
        context.Repository.ExecuteDeleteStoredProcedure(saved);

        // Assert
        TestIntDataItem fetched = new();
        fetched.SetId(savedId);

        context.Repository.ExecuteGetStoredProcedure(fetched);

        fetched.ReadCount.Should().Be(0);
    }

    [TestMethod]
    public void ExecuteSaveStoredProcedure_ForExistingItem_UpdatesRatherThanInserting()
    {
        // Arrange
        // The stored procedure branches on "@Id > 0", so re-saving an item that already carries an
        // id has to update in place and keep the same identity rather than inserting a second row.
        TestContext context = CreateContext();
        TestIntDataItem saved = CreateDataItem(testInt: 4248);

        context.Repository.ExecuteSaveStoredProcedure(saved);

        int originalId = saved.GetId();

        try
        {
            saved.TestIntValue = 9999;

            // Act
            context.Repository.ExecuteSaveStoredProcedure(saved);

            // Assert
            saved.GetId().Should().Be(originalId);

            TestIntDataItem fetched = new();
            fetched.SetId(originalId);

            context.Repository.ExecuteGetStoredProcedure(fetched);

            fetched.TestIntValue.Should().Be(9999);
        }
        finally
        {
            DeleteRow(context, saved);
        }
    }

    private static TestIntDataItem CreateDataItem(int testInt)
    {
        TestIntDataItem dataItem = new()
        {
            TestIntValue = testInt
        };

        return dataItem;
    }

    private static void DeleteRow(TestContext context, TestIntDataItem dataItem)
    {
        if (dataItem.GetId() == 0)
        {
            return;
        }

        context.Repository.ExecuteDeleteStoredProcedure(dataItem);
    }

    /// <summary>
    /// A hand-written stand-in for the generated TestInt data item, mapped onto the same
    /// gen_TestInt_* procedures. Writing it out here keeps this fixture independent of the generated
    /// TestClasses project and its static, DI-resolved client.
    /// </summary>
    private sealed class TestIntDataItem : IBaseDataItem
    {
        private int _id;
        private readonly List<ISpParameterData> _spParameterData = [];

        public TestIntDataItem()
        {
            AutoIdField = "Id";
            GuidIdField = "TestIntId";

            GetStoredProcedure = "gen_TestInt_GetRow";
            SaveStoredProcedure = "gen_TestInt_SaveRow";
            DeleteStoredProcedure = "gen_TestInt_DeleteRow";
            GetByGuidStoredProcedure = "gen_TestInt_GetRowByGuid";

            // Columns the database fills in: they travel as InputOutput parameters and come back
            // through GetParameterData().
            AutoUpdateFieldDictionary.Add("TestIntId", new TestSpParameter("TestIntId", DbType.Guid));
            AutoUpdateFieldDictionary.Add("SysInsertTime", new TestSpParameter("SysInsertTime", DbType.DateTime));
            AutoUpdateFieldDictionary.Add("SysUpdateTime", new TestSpParameter("SysUpdateTime", DbType.DateTime));
        }

        public string GuidIdField { get; set; }

        public string AutoIdField { get; set; }

        public Dictionary<string, ISpParameterData> AutoUpdateFieldDictionary { get; } = [];

        public string GetStoredProcedure { get; set; }

        public string DeleteStoredProcedure { get; set; }

        public string SaveStoredProcedure { get; set; }

        public string GetByGuidStoredProcedure { get; set; }

        public int TestIntValue { get; set; }

        public int ReadCount { get; private set; }

        public bool IsNew => _id == 0;

        public void InitializeDataItem()
        {
        }

        public List<ISpParameterData> SetItemData()
        {
            return
            [
                new TestSpParameter("testTinyInt", DbType.Byte) { Value = (byte)1 },
                new TestSpParameter("testBigInt", DbType.Int64) { Value = 1L },
                new TestSpParameter("testInt", DbType.Int32) { Value = TestIntValue },
                new TestSpParameter("testNtext", DbType.String) { Value = "integration test row" },
                new TestSpParameter("SysLock", DbType.String) { Value = string.Empty }
            ];
        }

        public void SetGuidParameter(Guid guidId)
        {
            _spParameterData.Add(new TestSpParameter(GuidIdField, DbType.Guid) { Value = guidId });
        }

        public void Save()
        {
        }

        public void Delete()
        {
        }

        public int GetId()
        {
            return _id;
        }

        public void SetId(int id)
        {
            _id = id;

            _spParameterData.Add(new TestSpParameter(AutoIdField, DbType.Int32) { Value = id });
        }

        public void GetKey(IDataReader dataReader)
        {
            _id = (int)dataReader["Id"];
        }

        public void GetData(IDataReader dataReader)
        {
            ReadCount++;

            TestIntValue = (int)dataReader["testInt"];
        }

        public List<ISpParameterData> GetParameterObjects()
        {
            List<ISpParameterData> parameterObjects = [.. _spParameterData];

            _spParameterData.Clear();

            return parameterObjects;
        }
    }

    private sealed class TestSpParameter : ISpParameterData
    {
        public TestSpParameter(string parameterName, DbType dbType)
        {
            ParameterName = parameterName;
            DbType = dbType;
        }

        public string ParameterName { get; set; }

        public DbType DbType { get; set; }

        public object? Value { get; set; }
    }

    private sealed class TestContext
    {
        public TestContext(
            SqlClientRepository Repository,
            IBaseClient BaseClient
        )
        {
            this.Repository = Repository;
            this.BaseClient = BaseClient;
        }

        public SqlClientRepository Repository { get; }

        public IBaseClient BaseClient { get; }
    }

    private static TestContext CreateContext()
    {
        SqlTestToolExtensions.CopySqlSettingsToActiveConfiguration();
        SqlTestToolExtensions.CopySqlSecurityToActiveConfiguration("TestClassGenerator");

        IServiceProvider serviceProvider = DependencyInjectionFactory.ConfigureDependencyInjection("--DefaultDatabase=TestClassGenerator");

        IClientConfig clientConfig = serviceProvider.GetRequiredService<IClientConfig>();
        IBaseClient baseClient = serviceProvider.GetRequiredService<IBaseClient>();

        return new TestContext(new SqlClientRepository(clientConfig), baseClient);
    }
}
