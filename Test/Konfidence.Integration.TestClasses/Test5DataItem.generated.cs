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
        public class Test5DataItem : BaseDataItem
        {
            // field definitions
            private const string ID = "Id";
            private const string NAAM = "Naam";
            private const string OMSCHRIJVING = "Omschrijving";
            private const string TEST6ID = "Test6Id";
            private const string SYSINSERTTIME = "SysInsertTime";
            private const string SYSUPDATETIME = "SysUpdateTime";
            private const string SYSLOCK = "SysLock";
            private const string TESTDOCUMENT = "TestDocument";

            // stored procedures
            private const string TEST5_GETROW = "gen_Test5_GetRow";
            private const string TEST5_SAVEROW = "gen_Test5_SaveRow";
            private const string TEST5_DELETEROW = "gen_Test5_DeleteRow";
            private const string TEST5_GETLIST = "gen_Test5_GetList";

            // property storage
            private string _Naam = string.Empty;
            private string _Omschrijving = string.Empty;
            private int _Test6Id = 0;
            private DateTime _SysInsertTime = DateTime.MinValue;
            private DateTime _SysUpdateTime = DateTime.MinValue;
            private string _SysLock = string.Empty;
            private string _TestDocument = string.Empty;

            private static IBaseClient _client;

            // generated properties

            public string Naam { get => _Naam; set => _Naam = value; }

            public string Omschrijving { get => _Omschrijving; set => _Omschrijving = value; }

            public int Test6Id { get => _Test6Id; set => _Test6Id = value; }

            public DateTime SysInsertTime => _SysInsertTime;

            public DateTime SysUpdateTime => _SysUpdateTime;

            public string SysLock { get => _SysLock; set => _SysLock = value; }

            public string TestDocument { get => _TestDocument; set => _TestDocument = value; }

            static Test5DataItem()
            {
                _client = _serviceProvider.GetService<IBaseClient>();
            }

            public Test5DataItem()
            {
                Client = _client;
            }

            public Test5DataItem(int id) : this()
            {
                GetItem(id);
            }

            public override void InitializeDataItem()
            {
                AutoIdField = ID;

                AddAutoUpdateField(SYSINSERTTIME, DbType.DateTime);
                AddAutoUpdateField(SYSUPDATETIME, DbType.DateTime);

                GetStoredProcedure = TEST5_GETROW;
                DeleteStoredProcedure = TEST5_DELETEROW;
                SaveStoredProcedure = TEST5_SAVEROW;

                base.InitializeDataItem();
            }

            protected override void GetAutoUpdateData()
            {
                this.GetAutoUpdateField(SYSINSERTTIME, ref _SysInsertTime);
                this.GetAutoUpdateField(SYSUPDATETIME, ref _SysUpdateTime);
            }

            public override void GetData(IDataReader dataReader)
            {
                dataReader.GetField(NAAM, out _Naam);
                dataReader.GetField(OMSCHRIJVING, out _Omschrijving);
                dataReader.GetField(TEST6ID, out _Test6Id);
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
                this.SetField(TEST6ID, _Test6Id);
                this.SetField(SYSLOCK, _SysLock);
                this.SetField(TESTDOCUMENT, _TestDocument);
            }

            public static List<Test5DataItem> GetList()
            {
                var test5List = new List<Test5DataItem>();

                _client.BuildItemList(test5List, TEST5_GETLIST);

                return test5List;
            }
        }
    }
}
