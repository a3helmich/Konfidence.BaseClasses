using System;
using System.Data;
using Konfidence.BaseData;

namespace DbSiteMapMenuClasses.Generated.Classes
{
	public partial class Bl
	{
		public partial class MenuDataItem : BaseDataItem
		{
			// field definitions
			internal const string MENUID = "MenuId";
			internal const string PARENTMENUID = "ParentMenuId";
			internal const string ID = "Id";
			internal const string MENUCODE = "MenuCode";
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
			private const string MENU_GETROWBYGUID = "gen_Menu_GetRowByGuid";
			private const string MENU_SAVEROW = "gen_Menu_SaveRow";
			private const string MENU_DELETEROW = "gen_Menu_DeleteRow";
			
			// property storage
			private Guid _MenuId = Guid.NewGuid();
			private Guid _ParentMenuId = Guid.Empty;
			private int _MenuCode = 0;
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
			
			private DbSiteMapMenuClasses.Bl.MenuTextDataItem _MenuText = null;
			
			#region generated properties
			
			public Guid MenuId
			{
				get { return _MenuId; }
				set { _MenuId = value; }
			}
			
			public Guid ParentMenuId
			{
				get { return _ParentMenuId; }
				set { _ParentMenuId = value; }
			}
			
			public int MenuCode
			{
				get { return _MenuCode; }
				set { _MenuCode = value; }
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
			
			public DbSiteMapMenuClasses.Bl.MenuTextDataItem MenuText
			{
				get
				{
					if (!IsAssigned(_MenuText))
					{
						_MenuText = Bl.MenuTextDataItem.GetByMenuId(MenuId);
					}
					
					return _MenuText;
				}
			}
			
			public MenuDataItem()
			{
			}
			
			public MenuDataItem(int id) : this()
			{
				GetItem(MENU_GETROW, id);
			}
			
			public MenuDataItem(Guid menuId) : this()
			{
				GetItem(MENU_GETROWBYGUID, menuId);
			}
			
			protected override void InitializeDataItem()
			{
				AutoIdField = ID;
				GuidIdField = MENUID;
				
				AddAutoUpdateField(SYSINSERTTIME, DbType.DateTime);
				AddAutoUpdateField(SYSUPDATETIME, DbType.DateTime);
				
				LoadStoredProcedure = MENU_GETROW;
				DeleteStoredProcedure = MENU_DELETEROW;
				SaveStoredProcedure = MENU_SAVEROW;
			}
			
			protected override void GetAutoUpdateData()
			{
				GetAutoUpdateField(SYSINSERTTIME, out _SysInsertTime);
				GetAutoUpdateField(SYSUPDATETIME, out _SysUpdateTime);
			}
			
			protected override void GetData()
			{
				GetField(MENUID, out _MenuId);
				GetField(PARENTMENUID, out _ParentMenuId);
				GetField(MENUCODE, out _MenuCode);
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
				
				SetField(MENUID, _MenuId);
				SetField(PARENTMENUID, _ParentMenuId);
				SetField(MENUCODE, _MenuCode);
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
