using System.Data;
using System;
using Konfidence.Base;
using Konfidence.BaseData;
using Konfidence.BaseDataInterfaces;
using Konfidence.SqlHostProvider.SqlAccess;

namespace DbMenuClasses
{
    public partial class Bl
    {
        public partial class TestIntDataItem : BaseDataItem
        {
            // field definitions
            internal const string TESTTINYINT = "testTinyInt";
            internal const string TESTID = "TestId";
            internal const string TESTINT = "testInt";
            internal const string SYSINSERTTIME = "SysInsertTime";
            internal const string SYSUPDATETIME = "SysUpdateTime";
            internal const string TESTNTEXT = "testNtext";
            internal const string TESTBIGINT = "testBigInt";
            internal const string SYSLOCK = "SysLock";

            // stored procedures
            private const string TESTINT_GETROW = "gen_TestInt_GetRow";
            private const string TESTINT_SAVEROW = "gen_TestInt_SaveRow";
            private const string TESTINT_DELETEROW = "gen_TestInt_DeleteRow";

            // property storage
            private byte _testTinyInt = 0;
            private int _testInt = 0;
            private DateTime _SysInsertTime = DateTime.MinValue;
            private DateTime _SysUpdateTime = DateTime.MinValue;
            private string _testNtext;
            private long _testBigInt = 0;
            private string _SysLock = string.Empty;

            #region generated properties
            // id storage
            public int TestId
            {
                get { return Id; }
            }

            public byte testTinyInt
            {
                get { return _testTinyInt; }
                set { _testTinyInt = value; }
            }

            public int testInt
            {
                get { return _testInt; }
                set { _testInt = value; }
            }

            public DateTime SysInsertTime
            {
                get { return _SysInsertTime; }
            }

            public DateTime SysUpdateTime
            {
                get { return _SysUpdateTime; }
            }

            public string testNtext
            {
                get { return _testNtext; }
                set { _testNtext = value; }
            }

            public long testBigInt
            {
                get { return _testBigInt; }
                set { _testBigInt = value; }
            }

            public string SysLock
            {
                get { return _SysLock; }
                set { _SysLock = value; }
            }
            #endregion generated properties

            public TestIntDataItem()
            {
            }

            public TestIntDataItem(int testid) : this()
            {
                GetItem(TESTINT_GETROW, testid);
            }

            protected override IBaseClient ClientBind()
            {
                return base.ClientBind<SqlClient>();
            }

            public override void InitializeDataItem()
            {
                AutoIdField = TESTID;

                AddAutoUpdateField(SYSINSERTTIME, DbType.DateTime);
                AddAutoUpdateField(SYSUPDATETIME, DbType.DateTime);

                LoadStoredProcedure = TESTINT_GETROW;
                DeleteStoredProcedure = TESTINT_DELETEROW;
                SaveStoredProcedure = TESTINT_SAVEROW;

                base.InitializeDataItem();

            }

            protected override void GetAutoUpdateData()
            {
                GetAutoUpdateField(SYSINSERTTIME, out _SysInsertTime);
                GetAutoUpdateField(SYSUPDATETIME, out _SysUpdateTime);
            }

            public override void GetData()
            {
                GetField(TESTTINYINT, out _testTinyInt);
                GetField(TESTINT, out _testInt);
                GetField(SYSINSERTTIME, out _SysInsertTime);
                GetField(SYSUPDATETIME, out _SysUpdateTime);
                GetField(TESTNTEXT, out _testNtext);
                GetField(TESTBIGINT, out _testBigInt);
                GetField(SYSLOCK, out _SysLock);
            }

            protected override void SetData()
            {
                base.SetData();

                SetField(TESTTINYINT, _testTinyInt);
                SetField(TESTINT, _testInt);
                SetField(TESTNTEXT, _testNtext);
                SetField(TESTBIGINT, _testBigInt);
                SetField(SYSLOCK, _SysLock);
            }
        }
    }
}
