using System;
using System.Data;
using System.Collections.Generic;
using Konfidence.BaseData.Sp;
using System;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Konfidence.Base;
using Konfidence.BaseData;
using Konfidence.DataBaseInterface;
using Konfidence.SqlHostProvider.SqlAccess;

namespace DbMenuClasses
{
    public partial class Bl
    {
        public partial class MenuDataItem : BaseDataItem
        {
            // field definitions
            internal const string NODEID = "NodeId";
            internal const string PARENTNODEID = "ParentNodeId";
            internal const string URL = "Url";
            internal const string APPLICATIONID = "ApplicationId";
            internal const string ISROOT = "IsRoot";
            internal const string ISVISIBLE = "IsVisible";
            internal const string MENUID = "MenuId";
            internal const string ISLOGONVISIBLE = "IsLogonVisible";
            internal const string ISADMINISTRATOR = "IsAdministrator";
            internal const string ISNOTLOGONVISIBLE = "IsNotLogonVisible";
            internal const string ISLOCALVISIBLE = "IsLocalVisible";
            internal const string SYSINSERTTIME = "SysInsertTime";
            internal const string SYSUPDATETIME = "SysUpdateTime";
            internal const string SYSLOCK = "SysLock";

            // stored procedures
            private const string MENU_GETROW = "gen_Menu_GetRow";
            private const string MENU_SAVEROW = "gen_Menu_SaveRow";
            private const string MENU_DELETEROW = "gen_Menu_DeleteRow";
            internal const string MENU_GETLIST = "gen_Menu_GetList";
            internal const string MENU_GETLISTBY_MENUID = "gen_Menu_GetListByMenuId";

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

            #region generated properties
            // id storage
            public int NodeId
            {
                get { return Id; }
            }

            public int ParentNodeId
            {
                get { return _ParentNodeId; }
                set { _ParentNodeId = value; }
            }

            public string Url
            {
                get { return _Url; }
                set { _Url = value; }
            }

            public string ApplicationId
            {
                get { return _ApplicationId; }
                set { _ApplicationId = value; }
            }

            public bool IsRoot
            {
                get { return _IsRoot; }
                set { _IsRoot = value; }
            }

            public bool IsVisible
            {
                get { return _IsVisible; }
                set { _IsVisible = value; }
            }

            public int MenuId
            {
                get { return _MenuId; }
                set { _MenuId = value; }
            }

            public bool IsLogonVisible
            {
                get { return _IsLogonVisible; }
                set { _IsLogonVisible = value; }
            }

            public bool IsAdministrator
            {
                get { return _IsAdministrator; }
                set { _IsAdministrator = value; }
            }

            public bool IsNotLogonVisible
            {
                get { return _IsNotLogonVisible; }
                set { _IsNotLogonVisible = value; }
            }

            public bool IsLocalVisible
            {
                get { return _IsLocalVisible; }
                set { _IsLocalVisible = value; }
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
            #endregion generated properties

            public MenuTextDataItem MenuText => _MenuText ?? (_MenuText = MenuTextDataItem.GetByNodeId(NodeId));

            static MenuDataItem()
            {
                _client = new SqlClient(string.Empty);
            }

            public MenuDataItem()
            {
                Client = _client;
            }

            public MenuDataItem(int nodeid) : this()
            {
                GetItem(nodeid);
            }

            protected override IBaseClient ClientBind()
            {
                return base.ClientBind<SqlClient>();
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
                GetAutoUpdateField(SYSINSERTTIME, out _SysInsertTime);
                GetAutoUpdateField(SYSUPDATETIME, out _SysUpdateTime);
            }

            public override void GetData(IDataReader dataReader)
            {
                GetField(PARENTNODEID, dataReader, out _ParentNodeId);
                GetField(URL, dataReader, out _Url);
                GetField(APPLICATIONID, dataReader, out _ApplicationId);
                GetField(ISROOT, dataReader, out _IsRoot);
                GetField(ISVISIBLE, dataReader, out _IsVisible);
                GetField(MENUID, dataReader, out _MenuId);
                GetField(ISLOGONVISIBLE, dataReader, out _IsLogonVisible);
                GetField(ISADMINISTRATOR, dataReader, out _IsAdministrator);
                GetField(ISNOTLOGONVISIBLE, dataReader, out _IsNotLogonVisible);
                GetField(ISLOCALVISIBLE, dataReader, out _IsLocalVisible);
                GetField(SYSINSERTTIME, dataReader, out _SysInsertTime);
                GetField(SYSUPDATETIME, dataReader, out _SysUpdateTime);
                GetField(SYSLOCK, dataReader, out _SysLock);
            }

            protected override void SetData()
            {
                base.SetData();

                SetField(PARENTNODEID, _ParentNodeId);
                SetField(URL, _Url);
                SetField(APPLICATIONID, _ApplicationId);
                SetField(ISROOT, _IsRoot);
                SetField(ISVISIBLE, _IsVisible);
                SetField(MENUID, _MenuId);
                SetField(ISLOGONVISIBLE, _IsLogonVisible);
                SetField(ISADMINISTRATOR, _IsAdministrator);
                SetField(ISNOTLOGONVISIBLE, _IsNotLogonVisible);
                SetField(ISLOCALVISIBLE, _IsLocalVisible);
                SetField(SYSLOCK, _SysLock);
            }

            public static MenuDataItemList GetList()
            {
                MenuDataItemList menuList = new MenuDataItemList();

                _client.BuildItemList(menuList, MenuDataItem.MENU_GETLIST);

                return menuList;
            }

            public static MenuDataItemList GetListByMenuId(int menuid)
            {
                var menuList = new MenuDataItemList();

                var spParameterList = new List<ISpParameterData>();

                spParameterList.SetParameter(MenuDataItem.MENUID, menuid);

                _client.BuildItemList(menuList, MENU_GETLISTBY_MENUID, spParameterList);

                return menuList;
            }
        }
    }
}
