using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Extensions.DependencyInjection;
using Konfidence.BaseData;
using Konfidence.DatabaseInterface;

namespace TestClasses
{
    public partial class Dl
    {
        public class Test7ExludeDataItem : BaseDataItem
        {
            // field definitions
            private const string TEST7EXLUDEID = "Test7ExludeId";
            private const string TESTDATA = "TestData";
            private const string SYSINSERTTIME = "SysInsertTime";
            private const string SYSUPDATETIME = "SysUpdateTime";
            private const string SYSLOCK = "SysLock";

            // stored procedures
            private const string TEST7EXLUDE_GETROW = "gen_Test7Exlude_GetRow";
            private const string TEST7EXLUDE_SAVEROW = "gen_Test7Exlude_SaveRow";
            private const string TEST7EXLUDE_DELETEROW = "gen_Test7Exlude_DeleteRow";
            private const string TEST7EXLUDE_GETLIST = "gen_Test7Exlude_GetList";

            // property storage
            private string _TestData = string.Empty;
            private DateTime _SysInsertTime = DateTime.MinValue;
            private DateTime _SysUpdateTime = DateTime.MinValue;
            private string _SysLock = string.Empty;

            private static IBaseClient _client;

            // generated properties
            public int Test7ExludeId => GetId();

            public string TestData { get => _TestData; set => _TestData = value; }

            public DateTime SysInsertTime => _SysInsertTime;

            public DateTime SysUpdateTime => _SysUpdateTime;

            public string SysLock { get => _SysLock; set => _SysLock = value; }

            static Test7ExludeDataItem()
            {
                _client = _serviceProvider.GetService<IBaseClient>();
            }

            public Test7ExludeDataItem()
            {
                Client = _client;
            }

            public Test7ExludeDataItem(int test7exludeid) : this()
            {
                GetItem(test7exludeid);
            }

            public override void InitializeDataItem()
            {
                AutoIdField = TEST7EXLUDEID;

                AddAutoUpdateField(SYSINSERTTIME, DbType.DateTime);
                AddAutoUpdateField(SYSUPDATETIME, DbType.DateTime);

                GetStoredProcedure = TEST7EXLUDE_GETROW;
                DeleteStoredProcedure = TEST7EXLUDE_DELETEROW;
                SaveStoredProcedure = TEST7EXLUDE_SAVEROW;

                base.InitializeDataItem();
            }

            protected override void GetAutoUpdateData()
            {
                this.GetAutoUpdateField(SYSINSERTTIME, ref _SysInsertTime);
                this.GetAutoUpdateField(SYSUPDATETIME, ref _SysUpdateTime);
            }

            public override void GetData(IDataReader dataReader)
            {
                dataReader.GetField(TESTDATA, out _TestData);
                dataReader.GetField(SYSINSERTTIME, out _SysInsertTime);
                dataReader.GetField(SYSUPDATETIME, out _SysUpdateTime);
                dataReader.GetField(SYSLOCK, out _SysLock);
            }

            protected override void SetData()
            {
                base.SetData();

                this.SetField(TESTDATA, _TestData);
                this.SetField(SYSLOCK, _SysLock);
            }

            public static List<Test7ExludeDataItem> GetList()
            {
                var test7exludeList = new List<Test7ExludeDataItem>();

                _client.BuildItemList(test7exludeList, TEST7EXLUDE_GETLIST);

                return test7exludeList;
            }
        }
    }
}
