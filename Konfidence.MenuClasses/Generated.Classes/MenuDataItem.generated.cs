using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Extensions.DependencyInjection;
using Konfidence.BaseData;
using Konfidence.BaseData.Sp;
using Konfidence.DataBaseInterface;

namespace DbMenuClasses
{
    public partial class Dl
    {
        public partial class MenuDataItem : BaseDataItem
        {
            // field definitions
            private const string NODEID = "NodeId";
            private const string PARENTNODEID = "ParentNodeId";
            private const string URL = "Url";
            private const string APPLICATIONID = "ApplicationId";
            private const string ISROOT = "IsRoot";
            private const string ISVISIBLE = "IsVisible";
            private const string MENUID = "MenuId";
            private const string ISLOGONVISIBLE = "IsLogonVisible";
            private const string ISADMINISTRATOR = "IsAdministrator";
            private const string ISNOTLOGONVISIBLE = "IsNotLogonVisible";
            private const string ISLOCALVISIBLE = "IsLocalVisible";
            private const string SYSINSERTTIME = "SysInsertTime";
            private const string SYSUPDATETIME = "SysUpdateTime";
            private const string SYSLOCK = "SysLock";

            // stored procedures
            private const string MENU_GETROW = "gen_Menu_GetRow";
            private const string MENU_SAVEROW = "gen_Menu_SaveRow";
            private const string MENU_DELETEROW = "gen_Menu_DeleteRow";
            private const string MENU_GETLIST = "gen_Menu_GetList";
            private const string MENU_GETLISTBY_MENUID = "gen_Menu_GetListByMenuId";

            // property storage
            private int _ParentNodeId = 0;
            private string _Url = string.Empty;
            private string _ApplicationId = string.Empty;
            private bool _IsRoot = false;
            private bool _IsVisible = false;
            private int _MenuId = 0;
            private bool _IsLogonVisible = false;
            private bool _IsAdministrator = false;
            private bool _IsNotLogonVisible = false;
            private bool _IsLocalVisible = false;
            private DateTime _SysInsertTime = DateTime.MinValue;
            private DateTime _SysUpdateTime = DateTime.MinValue;
            private string _SysLock = string.Empty;

            private MenuTextDataItem _MenuText = null;

            private static IBaseClient _client;

            // generated properties
            public int NodeId => GetId();

            public int ParentNodeId { get => _ParentNodeId; set => _ParentNodeId = value; }

            public string Url { get => _Url; set => _Url = value; }

            public string ApplicationId { get => _ApplicationId; set => _ApplicationId = value; }

            public bool IsRoot { get => _IsRoot; set => _IsRoot = value; }

            public bool IsVisible { get => _IsVisible; set => _IsVisible = value; }

            public int MenuId { get => _MenuId; set => _MenuId = value; }

            public bool IsLogonVisible { get => _IsLogonVisible; set => _IsLogonVisible = value; }

            public bool IsAdministrator { get => _IsAdministrator; set => _IsAdministrator = value; }

            public bool IsNotLogonVisible { get => _IsNotLogonVisible; set => _IsNotLogonVisible = value; }

            public bool IsLocalVisible { get => _IsLocalVisible; set => _IsLocalVisible = value; }

            public DateTime SysInsertTime => _SysInsertTime;

            public DateTime SysUpdateTime => _SysUpdateTime;

            public string SysLock { get => _SysLock; set => _SysLock = value; }

            public MenuTextDataItem MenuText => _MenuText ?? (_MenuText = MenuTextDataItem.GetByNodeId(NodeId));

            static MenuDataItem()
            {
                _client = _serviceProvider.GetService<IBaseClient>();
            }

            public MenuDataItem()
            {
                Client = _client;
            }

            public MenuDataItem(int nodeid) : this()
            {
                GetItem(nodeid);
            }

            public override void InitializeDataItem()
            {
                AutoIdField = NODEID;

                AddAutoUpdateField(SYSINSERTTIME, DbType.DateTime);
                AddAutoUpdateField(SYSUPDATETIME, DbType.DateTime);

                GetStoredProcedure = MENU_GETROW;
                DeleteStoredProcedure = MENU_DELETEROW;
                SaveStoredProcedure = MENU_SAVEROW;

                base.InitializeDataItem();
            }

            protected override void GetAutoUpdateData()
            {
                this.GetAutoUpdateField(SYSINSERTTIME, ref _SysInsertTime);
                this.GetAutoUpdateField(SYSUPDATETIME, ref _SysUpdateTime);
            }

            public override void GetData(IDataReader dataReader)
            {
                dataReader.GetField(PARENTNODEID, out _ParentNodeId);
                dataReader.GetField(URL, out _Url);
                dataReader.GetField(APPLICATIONID, out _ApplicationId);
                dataReader.GetField(ISROOT, out _IsRoot);
                dataReader.GetField(ISVISIBLE, out _IsVisible);
                dataReader.GetField(MENUID, out _MenuId);
                dataReader.GetField(ISLOGONVISIBLE, out _IsLogonVisible);
                dataReader.GetField(ISADMINISTRATOR, out _IsAdministrator);
                dataReader.GetField(ISNOTLOGONVISIBLE, out _IsNotLogonVisible);
                dataReader.GetField(ISLOCALVISIBLE, out _IsLocalVisible);
                dataReader.GetField(SYSINSERTTIME, out _SysInsertTime);
                dataReader.GetField(SYSUPDATETIME, out _SysUpdateTime);
                dataReader.GetField(SYSLOCK, out _SysLock);
            }

            protected override void SetData()
            {
                base.SetData();

                this.SetField(PARENTNODEID, _ParentNodeId);
                this.SetField(URL, _Url);
                this.SetField(APPLICATIONID, _ApplicationId);
                this.SetField(ISROOT, _IsRoot);
                this.SetField(ISVISIBLE, _IsVisible);
                this.SetField(MENUID, _MenuId);
                this.SetField(ISLOGONVISIBLE, _IsLogonVisible);
                this.SetField(ISADMINISTRATOR, _IsAdministrator);
                this.SetField(ISNOTLOGONVISIBLE, _IsNotLogonVisible);
                this.SetField(ISLOCALVISIBLE, _IsLocalVisible);
                this.SetField(SYSLOCK, _SysLock);
            }

            public static List<MenuDataItem> GetList()
            {
                var menuList = new List<MenuDataItem>();

                _client.BuildItemList(menuList, MENU_GETLIST);

                return menuList;
            }

            public static List<MenuDataItem> GetListByMenuId(int menuid)
            {
                var menuList = new List<MenuDataItem>();

                var spParameterList = new List<ISpParameterData>();

                spParameterList.SetParameter(MENUID, menuid);

                _client.BuildItemList(menuList, MENU_GETLISTBY_MENUID, spParameterList);

                return menuList;
            }
        }
    }
}
