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
        public class Test2DataItem : BaseDataItem
        {
            // field definitions
            private const string ID = "Id";
            private const string TEST2ID = "Test2Id";
            private const string NAAM = "Naam";
            private const string OMSCHRIJVING = "Omschrijving";
            private const string SYSINSERTTIME = "SysInsertTime";
            private const string SYSUPDATETIME = "SysUpdateTime";
            private const string SYSLOCK = "SysLock";
            private const string TESTDOCUMENT = "TestDocument";

            // stored procedures
            private const string TEST2_GETROW = "gen_Test2_GetRow";
            private const string TEST2_GETROWBYGUID = "gen_Test2_GetRowByGuid";
            private const string TEST2_SAVEROW = "gen_Test2_SaveRow";
            private const string TEST2_DELETEROW = "gen_Test2_DeleteRow";
            private const string TEST2_GETLIST = "gen_Test2_GetList";

            // property storage
            private Guid _Test2Id = Guid.Empty;
            private string _Naam = string.Empty;
            private string _Omschrijving = string.Empty;
            private DateTime _SysInsertTime = DateTime.MinValue;
            private DateTime _SysUpdateTime = DateTime.MinValue;
            private string _SysLock = string.Empty;
            private string _TestDocument = string.Empty;

            private static IBaseClient _client;

            // generated properties

            public Guid Test2Id => _Test2Id;

            public string Naam { get => _Naam; set => _Naam = value; }

            public string Omschrijving { get => _Omschrijving; set => _Omschrijving = value; }

            public DateTime SysInsertTime => _SysInsertTime;

            public DateTime SysUpdateTime => _SysUpdateTime;

            public string SysLock { get => _SysLock; set => _SysLock = value; }

            public string TestDocument { get => _TestDocument; set => _TestDocument = value; }

            static Test2DataItem()
            {
                _client = _serviceProvider.GetService<IBaseClient>();
            }

            public Test2DataItem()
            {
                Client = _client;
            }

            public Test2DataItem(int id) : this()
            {
                GetItem(id);
            }

            public Test2DataItem(Guid test2Id) : this()
            {
                GetItem(test2Id);
            }

            public override void InitializeDataItem()
            {
                AutoIdField = ID;
                GuidIdField = TEST2ID;

                AddAutoUpdateField(TEST2ID, DbType.Guid);
                AddAutoUpdateField(SYSINSERTTIME, DbType.DateTime);
                AddAutoUpdateField(SYSUPDATETIME, DbType.DateTime);

                GetStoredProcedure = TEST2_GETROW;
                DeleteStoredProcedure = TEST2_DELETEROW;
                SaveStoredProcedure = TEST2_SAVEROW;
                GetByGuidStoredProcedure = TEST2_GETROWBYGUID;

                base.InitializeDataItem();
            }

            protected override void GetAutoUpdateData()
            {
                this.GetAutoUpdateField(TEST2ID, ref _Test2Id);
                this.GetAutoUpdateField(SYSINSERTTIME, ref _SysInsertTime);
                this.GetAutoUpdateField(SYSUPDATETIME, ref _SysUpdateTime);
            }

            public override void GetData(IDataReader dataReader)
            {
                dataReader.GetField(TEST2ID, out _Test2Id);
                dataReader.GetField(NAAM, out _Naam);
                dataReader.GetField(OMSCHRIJVING, out _Omschrijving);
                dataReader.GetField(SYSINSERTTIME, out _SysInsertTime);
                dataReader.GetField(SYSUPDATETIME, out _SysUpdateTime);
                dataReader.GetField(SYSLOCK, out _SysLock);
                dataReader.GetField(TESTDOCUMENT, out _TestDocument);
            }

            protected override void SetData()
            {
                base.SetData();

                this.SetField(NAAM, _Naam);
                this.SetField(OMSCHRIJVING, _Omschrijving);
                this.SetField(SYSLOCK, _SysLock);
                this.SetField(TESTDOCUMENT, _TestDocument);
            }

            public static List<Test2DataItem> GetList()
            {
                var test2List = new List<Test2DataItem>();

                _client.BuildItemList(test2List, TEST2_GETLIST);

                return test2List;
            }
        }
    }
}
