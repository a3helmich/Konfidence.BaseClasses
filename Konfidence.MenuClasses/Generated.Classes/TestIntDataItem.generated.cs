using System;
using System.Data;
using System.Collections.Generic;
using Konfidence.BaseData.Sp;
using System;
using Konfidence.Base;
using Konfidence.BaseData;
using Konfidence.DataBaseInterface;
using Konfidence.SqlHostProvider.SqlAccess;

namespace DbMenuClasses
{
    public partial class Bl
    {
        public partial class TestIntDataItem : BaseDataItem
        {
            // field definitions
            internal const string ID = "Id";
            internal const string TESTINTID = "TestIntId";
            internal const string TESTTINYINT = "testTinyInt";
            internal const string TESTBIGINT = "testBigInt";
            internal const string TESTINT = "testInt";
            internal const string TESTNTEXT = "testNtext";
            internal const string SYSINSERTTIME = "SysInsertTime";
            internal const string SYSUPDATETIME = "SysUpdateTime";
            internal const string SYSLOCK = "SysLock";

            // stored procedures
            private const string TESTINT_GETROW = "gen_TestInt_GetRow";
            private const string TESTINT_GETROWBYGUID = "gen_TestInt_GetRowByGuid";
            private const string TESTINT_SAVEROW = "gen_TestInt_SaveRow";
            private const string TESTINT_DELETEROW = "gen_TestInt_DeleteRow";
            internal const string TESTINT_GETLIST = "gen_TestInt_GetList";

            // property storage
            private Guid _TestIntId = Guid.NewGuid();
            private byte _testTinyInt = 0;
            private long _testBigInt = 0;
            private int _testInt = 0;
            private string _testNtext = string.Empty;
            private DateTime _SysInsertTime = DateTime.MinValue;
            private DateTime _SysUpdateTime = DateTime.MinValue;
            private string _SysLock = string.Empty;

            private static IBaseClient _client;

            #region generated properties

            public Guid TestIntId
            {
                get { return _TestIntId; }
            }

            public byte testTinyInt
            {
                get { return _testTinyInt; }
                set { _testTinyInt = value; }
            }

            public long testBigInt
            {
                get { return _testBigInt; }
                set { _testBigInt = value; }
            }

            public int testInt
            {
                get { return _testInt; }
                set { _testInt = value; }
            }

            public string testNtext
            {
                get { return _testNtext; }
                set { _testNtext = value; }
            }

            public DateTime SysInsertTime
            {
                get { return _SysInsertTime; }
            }

            public DateTime SysUpdateTime
            {
                get { return _SysUpdateTime; }
            }

            public string SysLock
            {
                get { return _SysLock; }
                set { _SysLock = value; }
            }
            #endregion generated properties

            static TestIntDataItem()
            {
                _client = new SqlClient(string.Empty);
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
                GetItem(TESTINT_GETROWBYGUID, testintId);
            }

            protected override IBaseClient ClientBind()
            {
                return base.ClientBind<SqlClient>();
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

                base.InitializeDataItem();

            }

            protected override void GetAutoUpdateData()
            {
                GetAutoUpdateField(TESTINTID, out _TestIntId);
                GetAutoUpdateField(SYSINSERTTIME, out _SysInsertTime);
                GetAutoUpdateField(SYSUPDATETIME, out _SysUpdateTime);
            }

            public override void GetData(IDataReader dataReader)
            {
                GetField(TESTINTID, dataReader, out _TestIntId);
                GetField(TESTTINYINT, dataReader, out _testTinyInt);
                GetField(TESTBIGINT, dataReader, out _testBigInt);
                GetField(TESTINT, dataReader, out _testInt);
                GetField(TESTNTEXT, dataReader, out _testNtext);
                GetField(SYSINSERTTIME, dataReader, out _SysInsertTime);
                GetField(SYSUPDATETIME, dataReader, out _SysUpdateTime);
                GetField(SYSLOCK, dataReader, out _SysLock);
            }

            protected override void SetData()
            {
                base.SetData();

                SetField(TESTTINYINT, _testTinyInt);
                SetField(TESTBIGINT, _testBigInt);
                SetField(TESTINT, _testInt);
                SetField(TESTNTEXT, _testNtext);
                SetField(SYSLOCK, _SysLock);
            }

            public static List<TestIntDataItem> GetList()
            {
                var testintList = new List<TestIntDataItem>();

                _client.BuildItemList(testintList, TestIntDataItem.TESTINT_GETLIST);

                return testintList;
            }
        }
    }
}
