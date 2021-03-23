using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Extensions.DependencyInjection;
using Konfidence.BaseData;
using Konfidence.DatabaseInterface;

namespace DbMenuClasses
{
    public partial class Dl
    {
        public class MenuTextDataItem : BaseDataItem
        {
            // field definitions
            private const string NODEID = "NodeId";
            private const string LANGUAGE = "Language";
            private const string MENUTEXT = "MenuText";
            private const string DESCRIPTION = "Description";
            private const string SYSINSERTTIME = "SysInsertTime";
            private const string SYSUPDATETIME = "SysUpdateTime";
            private const string SYSLOCK = "SysLock";
            private const string MENUID = "MenuId";

            // stored procedures
            private const string MENUTEXT_GETROW = "gen_MenuText_GetRow";
            private const string MENUTEXT_SAVEROW = "gen_MenuText_SaveRow";
            private const string MENUTEXT_DELETEROW = "gen_MenuText_DeleteRow";
            private const string MENUTEXT_GETLIST = "gen_MenuText_GetList";
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

            // generated properties
            public int NodeId => GetId();

            public string Language => _Language;

            public string MenuText { get => _MenuText; set => _MenuText = value; }

            public string Description { get => _Description; set => _Description = value; }

            public DateTime SysInsertTime => _SysInsertTime;

            public DateTime SysUpdateTime => _SysUpdateTime;

            public string SysLock { get => _SysLock; set => _SysLock = value; }

            public int MenuId { get => _MenuId; set => _MenuId = value; }

            static MenuTextDataItem()
            {
                _client = _serviceProvider.GetService<IBaseClient>();
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
                this.GetAutoUpdateField(LANGUAGE, ref _Language);
                this.GetAutoUpdateField(SYSINSERTTIME, ref _SysInsertTime);
                this.GetAutoUpdateField(SYSUPDATETIME, ref _SysUpdateTime);
            }

            public override void GetData(IDataReader dataReader)
            {
                dataReader.GetField(LANGUAGE, out _Language);
                dataReader.GetField(MENUTEXT, out _MenuText);
                dataReader.GetField(DESCRIPTION, out _Description);
                dataReader.GetField(SYSINSERTTIME, out _SysInsertTime);
                dataReader.GetField(SYSUPDATETIME, out _SysUpdateTime);
                dataReader.GetField(SYSLOCK, out _SysLock);
                dataReader.GetField(MENUID, out _MenuId);
            }

            protected override void SetData()
            {
                base.SetData();

                this.SetField(MENUTEXT, _MenuText);
                this.SetField(DESCRIPTION, _Description);
                this.SetField(SYSLOCK, _SysLock);
                this.SetField(MENUID, _MenuId);
            }

            public static List<MenuTextDataItem> GetList()
            {
                var menutextList = new List<MenuTextDataItem>();

                _client.BuildItemList(menutextList, MENUTEXT_GETLIST);

                return menutextList;
            }

            public static MenuTextDataItem GetByNodeId(int nodeid)
            {
                MenuTextDataItem menutextDataItem = new MenuTextDataItem();

                menutextDataItem.SetField(NODEID, nodeid);

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
