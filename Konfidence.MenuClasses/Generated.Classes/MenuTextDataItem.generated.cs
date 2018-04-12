using System;
using System.Data;
using Konfidence.BaseData;
using Konfidence.BaseDataInterfaces;
using Konfidence.SqlHostProvider.SqlAccess;

namespace DbSiteMapMenuClasses
{
    public partial class Bl
    {
        public partial class MenuTextDataItem : BaseDataItem
        {
            // field definitions
            internal const string MENUTEXTID = "MenuTextId";
            internal const string MENUID = "MenuId";
            internal const string ID = "Id";
            internal const string SYSINSERTTIME = "SysInsertTime";
            internal const string SYSUPDATETIME = "SysUpdateTime";
            internal const string LANGUAGE = "Language";
            internal const string SYSLOCK = "SysLock";
            internal const string DESCRIPTION = "Description";
            internal const string MENUTEXT = "MenuText";

            // stored procedures
            private const string MENUTEXT_GETROW = "gen_MenuText_GetRow";
            private const string MENUTEXT_GETROWBYGUID = "gen_MenuText_GetRowByGuid";
            private const string MENUTEXT_SAVEROW = "gen_MenuText_SaveRow";
            private const string MENUTEXT_DELETEROW = "gen_MenuText_DeleteRow";
            private const string MENUTEXT_GETROWBY_MENUID = "gen_MenuText_GetRowByMenuId";

            // property storage
            private Guid _MenuTextId = Guid.NewGuid();
            private Guid _MenuId = Guid.Empty;
            private DateTime _SysInsertTime = DateTime.MinValue;
            private DateTime _SysUpdateTime = DateTime.MinValue;
            private string _Language = string.Empty;
            private string _SysLock = string.Empty;
            private string _Description = string.Empty;
            private string _MenuText = string.Empty;

            #region generated properties

            public Guid MenuTextId
            {
                get { return _MenuTextId; }
                set { _MenuTextId = value; }
            }

            public Guid MenuId
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

            public string SysLock
            {
                get { return _SysLock; }
                set { _SysLock = value; }
            }

            public string Description
            {
                get { return _Description; }
                set { _Description = value; }
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

            public MenuTextDataItem(int id) : this()
            {
                GetItem(MENUTEXT_GETROW, id);
            }

            public MenuTextDataItem(Guid menutextId) : this()
            {
                GetItem(MENUTEXT_GETROWBYGUID, menutextId);
            }

            protected override IBaseClient ClientBind()
            {
                return base.ClientBind<SqlClient>(DatabaseName);
            }

            public override void InitializeDataItem()
            {
                AutoIdField = ID;
                GuidIdField = MENUTEXTID;

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
            }

            public override void GetData()
            {
                GetField(MENUTEXTID, out _MenuTextId);
                GetField(MENUID, out _MenuId);
                GetField(SYSINSERTTIME, out _SysInsertTime);
                GetField(SYSUPDATETIME, out _SysUpdateTime);
                GetField(LANGUAGE, out _Language);
                GetField(SYSLOCK, out _SysLock);
                GetField(DESCRIPTION, out _Description);
                GetField(MENUTEXT, out _MenuText);
            }

            protected override void SetData()
            {
                base.SetData();

                SetField(MENUTEXTID, _MenuTextId);
                SetField(MENUID, _MenuId);
                SetField(SYSLOCK, _SysLock);
                SetField(DESCRIPTION, _Description);
                SetField(MENUTEXT, _MenuText);
            }

            public static MenuTextDataItem GetByMenuId(Guid menuid)
            {
                MenuTextDataItem menutextDataItem = new MenuTextDataItem();

                menutextDataItem.SetParameter(MENUID, menuid);

                menutextDataItem.GetItem(MENUTEXT_GETROWBY_MENUID);

                if (!menutextDataItem.IsNew)
                {
                    return menutextDataItem;
                }

                return null;
            }
        }
    }
}
