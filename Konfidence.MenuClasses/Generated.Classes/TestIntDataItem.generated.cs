using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Extensions.DependencyInjection;
using Konfidence.BaseData;
using Konfidence.DatabaseInterface;

namespace DbMenuClasses
{
    public partial class Dl
    {
        public partial class TestIntDataItem : BaseDataItem
        {
            // field definitions
            private const string ID = "Id";
            private const string TESTINTID = "TestIntId";
            private const string TESTTINYINT = "testTinyInt";
            private const string TESTBIGINT = "testBigInt";
            private const string TESTINT = "testInt";
            private const string TESTNTEXT = "testNtext";
            private const string SYSINSERTTIME = "SysInsertTime";
            private const string SYSUPDATETIME = "SysUpdateTime";
            private const string SYSLOCK = "SysLock";

            // stored procedures
            private const string TESTINT_GETROW = "gen_TestInt_GetRow";
            private const string TESTINT_GETROWBYGUID = "gen_TestInt_GetRowByGuid";
            private const string TESTINT_SAVEROW = "gen_TestInt_SaveRow";
            private const string TESTINT_DELETEROW = "gen_TestInt_DeleteRow";
            private const string TESTINT_GETLIST = "gen_TestInt_GetList";

            // property storage
            private Guid _TestIntId = Guid.Empty;
            private byte _testTinyInt = 0;
            private long _testBigInt = 0;
            private int _testInt = 0;
            private string _testNtext = string.Empty;
            private DateTime _SysInsertTime = DateTime.MinValue;
            private DateTime _SysUpdateTime = DateTime.MinValue;
            private string _SysLock = string.Empty;

            private static IBaseClient _client;

            // generated properties

            public Guid TestIntId => _TestIntId;

            public byte testTinyInt { get => _testTinyInt; set => _testTinyInt = value; }

            public long testBigInt { get => _testBigInt; set => _testBigInt = value; }

            public int testInt { get => _testInt; set => _testInt = value; }

            public string testNtext { get => _testNtext; set => _testNtext = value; }

            public DateTime SysInsertTime => _SysInsertTime;

            public DateTime SysUpdateTime => _SysUpdateTime;

            public string SysLock { get => _SysLock; set => _SysLock = value; }

            static TestIntDataItem()
            {
                _client = _serviceProvider.GetService<IBaseClient>();
            }

            public TestIntDataItem()
            {
                Client = _client;
            }

            public TestIntDataItem(int id) : this()
            {
                GetItem(id);
            }

            public TestIntDataItem(Guid testintId) : this()
            {
                GetItem(testintId);
            }

            public override void InitializeDataItem()
            {
                AutoIdField = ID;
                GuidIdField = TESTINTID;

                AddAutoUpdateField(TESTINTID, DbType.Guid);
                AddAutoUpdateField(SYSINSERTTIME, DbType.DateTime);
                AddAutoUpdateField(SYSUPDATETIME, DbType.DateTime);

                GetStoredProcedure = TESTINT_GETROW;
                DeleteStoredProcedure = TESTINT_DELETEROW;
                SaveStoredProcedure = TESTINT_SAVEROW;
                GetByGuidStoredProcedure = TESTINT_GETROWBYGUID;

                base.InitializeDataItem();
            }

            protected override void GetAutoUpdateData()
            {
                this.GetAutoUpdateField(TESTINTID, ref _TestIntId);
                this.GetAutoUpdateField(SYSINSERTTIME, ref _SysInsertTime);
                this.GetAutoUpdateField(SYSUPDATETIME, ref _SysUpdateTime);
            }

            public override void GetData(IDataReader dataReader)
            {
                dataReader.GetField(TESTINTID, out _TestIntId);
                dataReader.GetField(TESTTINYINT, out _testTinyInt);
                dataReader.GetField(TESTBIGINT, out _testBigInt);
                dataReader.GetField(TESTINT, out _testInt);
                dataReader.GetField(TESTNTEXT, out _testNtext);
                dataReader.GetField(SYSINSERTTIME, out _SysInsertTime);
                dataReader.GetField(SYSUPDATETIME, out _SysUpdateTime);
                dataReader.GetField(SYSLOCK, out _SysLock);
            }

            protected override void SetData()
            {
                base.SetData();

                this.SetField(TESTTINYINT, _testTinyInt);
                this.SetField(TESTBIGINT, _testBigInt);
                this.SetField(TESTINT, _testInt);
                this.SetField(TESTNTEXT, _testNtext);
                this.SetField(SYSLOCK, _SysLock);
            }

            public static List<TestIntDataItem> GetList()
            {
                var testintList = new List<TestIntDataItem>();

                _client.BuildItemList(testintList, TESTINT_GETLIST);

                return testintList;
            }
        }
    }
}
