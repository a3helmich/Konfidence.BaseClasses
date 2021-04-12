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
        public class Test6DataItem : BaseDataItem
        {
            // field definitions
            private const string TEST6ID = "Test6Id";
            private const string TESTDATA = "TestData";
            private const string SYSINSERTTIME = "SysInsertTime";
            private const string SYSUPDATETIME = "SysUpdateTime";
            private const string SYSLOCK = "SysLock";

            // stored procedures
            private const string TEST6_GETROW = "gen_Test6_GetRow";
            private const string TEST6_SAVEROW = "gen_Test6_SaveRow";
            private const string TEST6_DELETEROW = "gen_Test6_DeleteRow";
            private const string TEST6_GETLIST = "gen_Test6_GetList";

            // property storage
            private string _TestData = string.Empty;
            private DateTime _SysInsertTime = DateTime.MinValue;
            private DateTime _SysUpdateTime = DateTime.MinValue;
            private string _SysLock = string.Empty;

            private static IBaseClient _client;

            // generated properties
            public int Test6Id => GetId();

            public string TestData { get => _TestData; set => _TestData = value; }

            public DateTime SysInsertTime => _SysInsertTime;

            public DateTime SysUpdateTime => _SysUpdateTime;

            public string SysLock { get => _SysLock; set => _SysLock = value; }

            static Test6DataItem()
            {
                _client = _serviceProvider.GetService<IBaseClient>();
            }

            public Test6DataItem()
            {
                Client = _client;
            }

            public Test6DataItem(int test6id) : this()
            {
                GetItem(test6id);
            }

            public override void InitializeDataItem()
            {
                AutoIdField = TEST6ID;

                AddAutoUpdateField(SYSINSERTTIME, DbType.DateTime);
                AddAutoUpdateField(SYSUPDATETIME, DbType.DateTime);

                GetStoredProcedure = TEST6_GETROW;
                DeleteStoredProcedure = TEST6_DELETEROW;
                SaveStoredProcedure = TEST6_SAVEROW;

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

            public static List<Test6DataItem> GetList()
            {
                var test6List = new List<Test6DataItem>();

                _client.BuildItemList(test6List, TEST6_GETLIST);

                return test6List;
            }
        }
    }
}
