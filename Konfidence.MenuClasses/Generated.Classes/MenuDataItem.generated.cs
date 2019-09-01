using System.Data;
using System;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Konfidence.Base;
using Konfidence.BaseData;
using Konfidence.SqlHostProvider.SqlAccess;
using Konfidence.BaseDataInterfaces;

namespace DbMenuClasses
{
    public partial class Bl
    {
        public partial class MenuDataItem : BaseDataItem
        {
            // field definitions
            internal const string NODEID = "NodeId";
            internal const string PARENTNODEID = "ParentNodeId";
            internal const string MENUID = "MenuId";
            internal const string SYSINSERTTIME = "SysInsertTime";
            internal const string SYSUPDATETIME = "SysUpdateTime";
            internal const string ISROOT = "IsRoot";
            internal const string ISVISIBLE = "IsVisible";
            internal const string ISLOGONVISIBLE = "IsLogonVisible";
            internal const string ISADMINISTRATOR = "IsAdministrator";
            internal const string ISNOTLOGONVISIBLE = "IsNotLogonVisible";
            internal const string ISLOCALVISIBLE = "IsLocalVisible";
            internal const string URL = "Url";
            internal const string APPLICATIONID = "ApplicationId";
            internal const string SYSLOCK = "SysLock";

            // stored procedures
            private const string MENU_GETROW = "gen_Menu_GetRow";
            private const string MENU_SAVEROW = "gen_Menu_SaveRow";
            private const string MENU_DELETEROW = "gen_Menu_DeleteRow";

            // property storage
            private int _ParentNodeId = 0;
            private int _MenuId = 0;
            private DateTime _SysInsertTime = DateTime.MinValue;
            private DateTime _SysUpdateTime = DateTime.MinValue;
            private bool _IsRoot = false;
            private bool _IsVisible = false;
            private bool _IsLogonVisible = false;
            private bool _IsAdministrator = false;
            private bool _IsNotLogonVisible = false;
            private bool _IsLocalVisible = false;
            private string _Url = string.Empty;
            private string _ApplicationId = string.Empty;
            private string _SysLock = string.Empty;

            private MenuTextDataItem _MenuText = null;

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

            public int MenuId
            {
                get { return _MenuId; }
                set { _MenuId = value; }
            }

            public DateTime SysInsertTime
            {
                get { return _SysInsertTime; }
            }

            public DateTime SysUpdateTime
            {
                get { return _SysUpdateTime; }
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

            public string SysLock
            {
                get { return _SysLock; }
                set { _SysLock = value; }
            }
            #endregion generated properties

            public MenuTextDataItem MenuText
            {
                get
                {
                    if (!_MenuText.IsAssigned())
                    {
                        _MenuText = MenuTextDataItem.GetByNodeId(NodeId);
                    }

                    return _MenuText;
                }
            }

            public MenuDataItem()
            {
            }

            public MenuDataItem(int nodeid) : this()
            {
                GetItem(MENU_GETROW, nodeid);
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

                LoadStoredProcedure = MENU_GETROW;
                DeleteStoredProcedure = MENU_DELETEROW;
                SaveStoredProcedure = MENU_SAVEROW;

                base.InitializeDataItem();

            }

            protected override void GetAutoUpdateData()
            {
                GetAutoUpdateField(SYSINSERTTIME, out _SysInsertTime);
                GetAutoUpdateField(SYSUPDATETIME, out _SysUpdateTime);
            }

            public override void GetData()
            {
                GetField(PARENTNODEID, out _ParentNodeId);
                GetField(MENUID, out _MenuId);
                GetField(SYSINSERTTIME, out _SysInsertTime);
                GetField(SYSUPDATETIME, out _SysUpdateTime);
                GetField(ISROOT, out _IsRoot);
                GetField(ISVISIBLE, out _IsVisible);
                GetField(ISLOGONVISIBLE, out _IsLogonVisible);
                GetField(ISADMINISTRATOR, out _IsAdministrator);
                GetField(ISNOTLOGONVISIBLE, out _IsNotLogonVisible);
                GetField(ISLOCALVISIBLE, out _IsLocalVisible);
                GetField(URL, out _Url);
                GetField(APPLICATIONID, out _ApplicationId);
                GetField(SYSLOCK, out _SysLock);
            }

            protected override void SetData()
            {
                base.SetData();

                SetField(PARENTNODEID, _ParentNodeId);
                SetField(MENUID, _MenuId);
                SetField(ISROOT, _IsRoot);
                SetField(ISVISIBLE, _IsVisible);
                SetField(ISLOGONVISIBLE, _IsLogonVisible);
                SetField(ISADMINISTRATOR, _IsAdministrator);
                SetField(ISNOTLOGONVISIBLE, _IsNotLogonVisible);
                SetField(ISLOCALVISIBLE, _IsLocalVisible);
                SetField(URL, _Url);
                SetField(APPLICATIONID, _ApplicationId);
                SetField(SYSLOCK, _SysLock);
            }
        }
    }
}
