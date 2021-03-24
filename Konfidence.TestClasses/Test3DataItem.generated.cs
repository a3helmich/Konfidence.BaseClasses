using System;
using System.Collections.Generic;
using System.Data;
using System.Xml;
using Microsoft.Extensions.DependencyInjection;
using Konfidence.BaseData;
using Konfidence.DatabaseInterface;

namespace TestClasses
{
    public partial class Dl
    {
        public class Test3DataItem : BaseDataItem
        {
            // field definitions
            private const string ID = "Id";
            private const string TEST3ID = "Test3Id";
            private const string NAAM = "Naam";
            private const string OMSCHRIJVING = "Omschrijving";
            private const string SYSINSERTTIME = "SysInsertTime";
            private const string SYSUPDATETIME = "SysUpdateTime";
            private const string SYSLOCK = "SysLock";
            private const string TESTDOCUMENT = "TestDocument";

            // stored procedures
            private const string TEST3_GETROW = "gen_Test3_GetRow";
            private const string TEST3_GETROWBYGUID = "gen_Test3_GetRowByGuid";
            private const string TEST3_SAVEROW = "gen_Test3_SaveRow";
            private const string TEST3_DELETEROW = "gen_Test3_DeleteRow";
            private const string TEST3_GETLIST = "gen_Test3_GetList";

            // property storage
            private Guid _Test3Id = Guid.Empty;
            private string _Naam = string.Empty;
            private string _Omschrijving = string.Empty;
            private DateTime _SysInsertTime = DateTime.MinValue;
            private DateTime _SysUpdateTime = DateTime.MinValue;
            private string _SysLock = string.Empty;
            private XmlDocument _TestDocument = new XmlDocument();

            private static IBaseClient _client;

            // generated properties

            public Guid Test3Id => _Test3Id;

            public string Naam { get => _Naam; set => _Naam = value; }

            public string Omschrijving { get => _Omschrijving; set => _Omschrijving = value; }

            public DateTime SysInsertTime => _SysInsertTime;

            public DateTime SysUpdateTime => _SysUpdateTime;

            public string SysLock { get => _SysLock; set => _SysLock = value; }

            public XmlDocument TestDocument { get => _TestDocument; set => _TestDocument = value; }

            static Test3DataItem()
            {
                _client = _serviceProvider.GetService<IBaseClient>();
            }

            public Test3DataItem()
            {
                Client = _client;

                _TestDocument.LoadXml("<TestDocument />");
            }

            public Test3DataItem(int id) : this()
            {
                GetItem(id);
            }

            public Test3DataItem(Guid test3Id) : this()
            {
                GetItem(test3Id);
            }

            public override void InitializeDataItem()
            {
                AutoIdField = ID;
                GuidIdField = TEST3ID;

                AddAutoUpdateField(TEST3ID, DbType.Guid);
                AddAutoUpdateField(SYSINSERTTIME, DbType.DateTime);
                AddAutoUpdateField(SYSUPDATETIME, DbType.DateTime);

                GetStoredProcedure = TEST3_GETROW;
                DeleteStoredProcedure = TEST3_DELETEROW;
                SaveStoredProcedure = TEST3_SAVEROW;
                GetByGuidStoredProcedure = TEST3_GETROWBYGUID;

                base.InitializeDataItem();
            }

            protected override void GetAutoUpdateData()
            {
                this.GetAutoUpdateField(TEST3ID, ref _Test3Id);
                this.GetAutoUpdateField(SYSINSERTTIME, ref _SysInsertTime);
                this.GetAutoUpdateField(SYSUPDATETIME, ref _SysUpdateTime);
            }

            public override void GetData(IDataReader dataReader)
            {
                dataReader.GetField(TEST3ID, out _Test3Id);
                dataReader.GetField(NAAM, out _Naam);
                dataReader.GetField(OMSCHRIJVING, out _Omschrijving);
                dataReader.GetField(SYSINSERTTIME, out _SysInsertTime);
                dataReader.GetField(SYSUPDATETIME, out _SysUpdateTime);
                dataReader.GetField(SYSLOCK, out _SysLock);
                dataReader.GetField(TESTDOCUMENT, ref _TestDocument);
            }

            protected override void SetData()
            {
                base.SetData();

                this.SetField(NAAM, _Naam);
                this.SetField(OMSCHRIJVING, _Omschrijving);
                this.SetField(SYSLOCK, _SysLock);
                this.SetField(TESTDOCUMENT, _TestDocument.InnerXml);
            }

            public static List<Test3DataItem> GetList()
            {
                var test3List = new List<Test3DataItem>();

                _client.BuildItemList(test3List, TEST3_GETLIST);

                return test3List;
            }
        }
    }
}
