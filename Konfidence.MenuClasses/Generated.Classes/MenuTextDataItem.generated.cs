using System;
using System.Data;
using System.Collections.Generic;
using Konfidence.BaseData;
using Konfidence.DataBaseInterface;
using Konfidence.SqlHostProvider;
using Microsoft.Extensions.DependencyInjection;

namespace DbMenuClasses
{
    public partial class Bl
    {
        public partial class MenuTextDataItem : BaseDataItem
        {
            // field definitions
            internal const string NODEID = "NodeId";
            internal const string LANGUAGE = "Language";
            internal const string MENUTEXT = "MenuText";
            internal const string DESCRIPTION = "Description";
            internal const string SYSINSERTTIME = "SysInsertTime";
            internal const string SYSUPDATETIME = "SysUpdateTime";
            internal const string SYSLOCK = "SysLock";
            internal const string MENUID = "MenuId";

            // stored procedures
            private const string MENUTEXT_GETROW = "gen_MenuText_GetRow";
            private const string MENUTEXT_SAVEROW = "gen_MenuText_SaveRow";
            private const string MENUTEXT_DELETEROW = "gen_MenuText_DeleteRow";
            internal const string MENUTEXT_GETLIST = "gen_MenuText_GetList";
            private const string MENUTEXT_GETROWBY_NODEID = "gen_MenuText_GetRowByNodeId";

            // property storage
            private string _Language = string.Empty;
            private string _MenuText = string.Empty;
            private string _Description = string.Empty;
            private DateTime _SysInsertTime = DateTime.MinValue;
            private DateTime _SysUpdateTime = DateTime.MinValue;
            private string _SysLock = string.Empty;
            private int _MenuId = 0;

            private static IBaseClient _client;

            #region generated properties
            // id storage
            public int NodeId
            {
                get { return Id; }
            }

            public string Language
            {
                get { return _Language; }
            }

            public string MenuText
            {
                get { return _MenuText; }
                set { _MenuText = value; }
            }

            public string Description
            {
                get { return _Description; }
                set { _Description = value; }
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

            public int MenuId
            {
                get { return _MenuId; }
                set { _MenuId = value; }
            }
            #endregion generated properties

            static MenuTextDataItem()
            {
                var provider = DependencyInjectionFactory.ConfigureDependencyInjection();

                _client = provider.GetService<IBaseClient>();
            }

            public MenuTextDataItem()
            {
                Client = _client;
            }

            public MenuTextDataItem(int nodeid) : this()
            {
                GetItem(nodeid);
            }

            public override void InitializeDataItem()
            {
                AutoIdField = NODEID;

                AddAutoUpdateField(LANGUAGE, DbType.String);
                AddAutoUpdateField(SYSINSERTTIME, DbType.DateTime);
                AddAutoUpdateField(SYSUPDATETIME, DbType.DateTime);

                GetStoredProcedure = MENUTEXT_GETROW;
                DeleteStoredProcedure = MENUTEXT_DELETEROW;
                SaveStoredProcedure = MENUTEXT_SAVEROW;

                base.InitializeDataItem();

            }

            protected override void GetAutoUpdateData()
            {
                GetAutoUpdateField(LANGUAGE, out _Language);
                GetAutoUpdateField(SYSINSERTTIME, out _SysInsertTime);
                GetAutoUpdateField(SYSUPDATETIME, out _SysUpdateTime);
            }

            public override void GetData(IDataReader dataReader)
            {
                GetField(LANGUAGE, dataReader, out _Language);
                GetField(MENUTEXT, dataReader, out _MenuText);
                GetField(DESCRIPTION, dataReader, out _Description);
                GetField(SYSINSERTTIME, dataReader, out _SysInsertTime);
                GetField(SYSUPDATETIME, dataReader, out _SysUpdateTime);
                GetField(SYSLOCK, dataReader, out _SysLock);
                GetField(MENUID, dataReader, out _MenuId);
            }

            protected override void SetData()
            {
                base.SetData();

                SetField(MENUTEXT, _MenuText);
                SetField(DESCRIPTION, _Description);
                SetField(SYSLOCK, _SysLock);
                SetField(MENUID, _MenuId);
            }

            public static List<MenuTextDataItem> GetList()
            {
                var menutextList = new List<MenuTextDataItem>();

                _client.BuildItemList(menutextList, MenuTextDataItem.MENUTEXT_GETLIST);

                return menutextList;
            }

            public static MenuTextDataItem GetByNodeId(int nodeid)
            {
                MenuTextDataItem menutextDataItem = new MenuTextDataItem();

                menutextDataItem.SetParameter(NODEID, nodeid);

                menutextDataItem.GetItemBy(MENUTEXT_GETROWBY_NODEID);

                if (!menutextDataItem.IsNew)
                {
                    return menutextDataItem;
                }

                return null;
            }
        }
    }
}
