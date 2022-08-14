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
        public class Test4DataItem : BaseDataItem
        {
            // field definitions
            private const string ID = "Id";
            private const string TEST4ID = "Test4Id";
            private const string NAAM = "Naam";
            private const string OMSCHRIJVING = "Omschrijving";
            private const string SYSINSERTTIME = "SysInsertTime";
            private const string SYSUPDATETIME = "SysUpdateTime";
            private const string SYSLOCK = "SysLock";
            private const string TESTDOCUMENT = "TestDocument";

            // stored procedures
            private const string TEST4_GETROW = "gen_Test4_GetRow";
            private const string TEST4_GETROWBYGUID = "gen_Test4_GetRowByGuid";
            private const string TEST4_SAVEROW = "gen_Test4_SaveRow";
            private const string TEST4_DELETEROW = "gen_Test4_DeleteRow";
            private const string TEST4_GETLIST = "gen_Test4_GetList";

            // property storage
            private Guid _Test4Id = Guid.Empty;
            private string _Naam = string.Empty;
            private string _Omschrijving = string.Empty;
            private DateTime _SysInsertTime = DateTime.MinValue;
            private DateTime _SysUpdateTime = DateTime.MinValue;
            private string _SysLock = string.Empty;
            private string _TestDocument = string.Empty;

            private static IBaseClient _client;

            // generated properties

            public Guid Test4Id => _Test4Id;

            public string Naam { get => _Naam; set => _Naam = value; }

            public string Omschrijving { get => _Omschrijving; set => _Omschrijving = value; }

            public DateTime SysInsertTime => _SysInsertTime;

            public DateTime SysUpdateTime => _SysUpdateTime;

            public string SysLock { get => _SysLock; set => _SysLock = value; }

            public string TestDocument { get => _TestDocument; set => _TestDocument = value; }

            static Test4DataItem()
            {
                _client = _serviceProvider.GetService<IBaseClient>();
            }

            public Test4DataItem()
            {
                Client = _client;
            }

            public Test4DataItem(int id) : this()
            {
                GetItem(id);
            }

            public Test4DataItem(Guid test4Id) : this()
            {
                GetItem(test4Id);
            }

            public override void InitializeDataItem()
            {
                AutoIdField = ID;
                GuidIdField = TEST4ID;

                AddAutoUpdateField(TEST4ID, DbType.Guid);
                AddAutoUpdateField(SYSINSERTTIME, DbType.DateTime);
                AddAutoUpdateField(SYSUPDATETIME, DbType.DateTime);

                GetStoredProcedure = TEST4_GETROW;
                DeleteStoredProcedure = TEST4_DELETEROW;
                SaveStoredProcedure = TEST4_SAVEROW;
                GetByGuidStoredProcedure = TEST4_GETROWBYGUID;

                base.InitializeDataItem();
            }

            protected override void GetAutoUpdateData()
            {
                this.GetAutoUpdateField(TEST4ID, ref _Test4Id);
                this.GetAutoUpdateField(SYSINSERTTIME, ref _SysInsertTime);
                this.GetAutoUpdateField(SYSUPDATETIME, ref _SysUpdateTime);
            }

            public override void GetData(IDataReader dataReader)
            {
                dataReader.GetField(TEST4ID, out _Test4Id);
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

            public static List<Test4DataItem> GetList()
            {
                var test4List = new List<Test4DataItem>();

                _client.BuildItemList(test4List, TEST4_GETLIST);

                return test4List;
            }
        }
    }
}
