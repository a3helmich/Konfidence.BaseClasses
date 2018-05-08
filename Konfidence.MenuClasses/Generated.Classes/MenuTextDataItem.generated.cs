using System.Data;
using System;
using Konfidence.Base;
using Konfidence.BaseData;
using Konfidence.BaseDataInterfaces;
using Konfidence.SqlHostProvider.SqlAccess;

namespace DbMenuClasses
{
    public partial class Bl
    {
        public partial class MenuTextDataItem : BaseDataItem
        {
            // field definitions
            internal const string NODEID = "NodeId";
            internal const string MENUID = "MenuId";
            internal const string SYSINSERTTIME = "SysInsertTime";
            internal const string SYSUPDATETIME = "SysUpdateTime";
            internal const string LANGUAGE = "Language";
            internal const string DESCRIPTION = "Description";
            internal const string SYSLOCK = "SysLock";
            internal const string MENUTEXT = "MenuText";

            // stored procedures
            private const string MENUTEXT_GETROW = "gen_MenuText_GetRow";
            private const string MENUTEXT_SAVEROW = "gen_MenuText_SaveRow";
            private const string MENUTEXT_DELETEROW = "gen_MenuText_DeleteRow";
            private const string MENUTEXT_GETROWBY_NODEID = "gen_MenuText_GetRowByNodeId";

            // property storage
            private int _MenuId = 0;
            private DateTime _SysInsertTime = DateTime.MinValue;
            private DateTime _SysUpdateTime = DateTime.MinValue;
            private string _Language = string.Empty;
            private string _Description = string.Empty;
            private string _SysLock = string.Empty;
            private string _MenuText = string.Empty;

            #region generated properties
            // id storage
            public int NodeId
            {
                get { return Id; }
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

            public string Language
            {
                get { return _Language; }
            }

            public string Description
            {
                get { return _Description; }
                set { _Description = value; }
            }

            public string SysLock
            {
                get { return _SysLock; }
                set { _SysLock = value; }
            }

            public string MenuText
            {
                get { return _MenuText; }
                set { _MenuText = value; }
            }
            #endregion generated properties

            public MenuTextDataItem()
            {
            }

            public MenuTextDataItem(int nodeid) : this()
            {
                GetItem(MENUTEXT_GETROW, nodeid);
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
                AddAutoUpdateField(LANGUAGE, DbType.String);

                LoadStoredProcedure = MENUTEXT_GETROW;
                DeleteStoredProcedure = MENUTEXT_DELETEROW;
                SaveStoredProcedure = MENUTEXT_SAVEROW;

                base.InitializeDataItem();

            }

            protected override void GetAutoUpdateData()
            {
                GetAutoUpdateField(SYSINSERTTIME, out _SysInsertTime);
                GetAutoUpdateField(SYSUPDATETIME, out _SysUpdateTime);
                GetAutoUpdateField(LANGUAGE, out _Language);
            }

            public override void GetData()
            {
                GetField(MENUID, out _MenuId);
                GetField(SYSINSERTTIME, out _SysInsertTime);
                GetField(SYSUPDATETIME, out _SysUpdateTime);
                GetField(LANGUAGE, out _Language);
                GetField(DESCRIPTION, out _Description);
                GetField(SYSLOCK, out _SysLock);
                GetField(MENUTEXT, out _MenuText);
            }

            protected override void SetData()
            {
                base.SetData();

                SetField(MENUID, _MenuId);
                SetField(DESCRIPTION, _Description);
                SetField(SYSLOCK, _SysLock);
                SetField(MENUTEXT, _MenuText);
            }

            public static MenuTextDataItem GetByNodeId(int nodeid)
            {
                MenuTextDataItem menutextDataItem = new MenuTextDataItem();

                menutextDataItem.SetParameter(NODEID, nodeid);

                menutextDataItem.GetItem(MENUTEXT_GETROWBY_NODEID);

                if (!menutextDataItem.IsNew)
                {
                    return menutextDataItem;
                }

                return null;
            }
        }
    }
}
