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
        public class Test1DataItem : BaseDataItem
        {
            // field definitions
            private const string ID = "Id";
            private const string TEST1ID = "Test1Id";
            private const string NAAM = "naam";
            private const string OMSCHRIJVING = "omschrijving";
            private const string SYSINSERTTIME = "SysInsertTime";
            private const string SYSUPDATETIME = "SysUpdateTime";
            private const string SYSLOCK = "SysLock";
            private const string YEAR = "Year";

            // stored procedures
            private const string TEST1_GETROW = "gen_Test1_GetRow";
            private const string TEST1_GETROWBYGUID = "gen_Test1_GetRowByGuid";
            private const string TEST1_SAVEROW = "gen_Test1_SaveRow";
            private const string TEST1_DELETEROW = "gen_Test1_DeleteRow";
            private const string TEST1_GETLIST = "gen_Test1_GetList";

            // property storage
            private Guid _Test1Id = Guid.Empty;
            private string _naam = string.Empty;
            private string _omschrijving = string.Empty;
            private DateTime _SysInsertTime = DateTime.MinValue;
            private DateTime _SysUpdateTime = DateTime.MinValue;
            private string _SysLock = string.Empty;
            private int _Year = 0;

            private static IBaseClient _client;

            // generated properties

            public Guid Test1Id => _Test1Id;

            public string naam { get => _naam; set => _naam = value; }

            public string omschrijving { get => _omschrijving; set => _omschrijving = value; }

            public DateTime SysInsertTime => _SysInsertTime;

            public DateTime SysUpdateTime => _SysUpdateTime;

            public string SysLock { get => _SysLock; set => _SysLock = value; }

            public int Year => _Year;

            static Test1DataItem()
            {
                _client = _serviceProvider.GetService<IBaseClient>();
            }

            public Test1DataItem()
            {
                Client = _client;
            }

            public Test1DataItem(int id) : this()
            {
                GetItem(id);
            }

            public Test1DataItem(Guid test1Id) : this()
            {
                GetItem(test1Id);
            }

            public override void InitializeDataItem()
            {
                AutoIdField = ID;
                GuidIdField = TEST1ID;

                AddAutoUpdateField(TEST1ID, DbType.Guid);
                AddAutoUpdateField(SYSINSERTTIME, DbType.DateTime);
                AddAutoUpdateField(SYSUPDATETIME, DbType.DateTime);
                AddAutoUpdateField(YEAR, DbType.Int32);

                GetStoredProcedure = TEST1_GETROW;
                DeleteStoredProcedure = TEST1_DELETEROW;
                SaveStoredProcedure = TEST1_SAVEROW;
                GetByGuidStoredProcedure = TEST1_GETROWBYGUID;

                base.InitializeDataItem();
            }

            protected override void GetAutoUpdateData()
            {
                this.GetAutoUpdateField(TEST1ID, ref _Test1Id);
                this.GetAutoUpdateField(SYSINSERTTIME, ref _SysInsertTime);
                this.GetAutoUpdateField(SYSUPDATETIME, ref _SysUpdateTime);
                this.GetAutoUpdateField(YEAR, ref _Year);
            }

            public override void GetData(IDataReader dataReader)
            {
                dataReader.GetField(TEST1ID, out _Test1Id);
                dataReader.GetField(NAAM, out _naam);
                dataReader.GetField(OMSCHRIJVING, out _omschrijving);
                dataReader.GetField(SYSINSERTTIME, out _SysInsertTime);
                dataReader.GetField(SYSUPDATETIME, out _SysUpdateTime);
                dataReader.GetField(SYSLOCK, out _SysLock);
                dataReader.GetField(YEAR, out _Year);
            }

            protected override void SetData()
            {
                base.SetData();

                this.SetField(NAAM, _naam);
                this.SetField(OMSCHRIJVING, _omschrijving);
                this.SetField(SYSLOCK, _SysLock);
            }

            public static List<Test1DataItem> GetList()
            {
                var test1List = new List<Test1DataItem>();

                _client.BuildItemList(test1List, TEST1_GETLIST);

                return test1List;
            }
        }
    }
}
