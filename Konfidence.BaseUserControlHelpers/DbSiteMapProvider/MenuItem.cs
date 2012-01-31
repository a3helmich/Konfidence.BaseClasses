using Konfidence.BaseData;

namespace Konfidence.BaseUserControlHelpers.DbSiteMapProvider
{
	public class MenuItem : BaseDataItem
	{
		protected const string NODEID_FIELD = "NodeId";
		protected internal const string PARENTNODEID_FIELD = "ParentNodeId";
		protected const string URL_FIELD = "Url";
		protected const string ROOT_FIELD = "IsRoot";
		protected const string VISIBLE_FIELD = "IsVisible";
		protected const string MENUID_FIELD = "MenuId";
		protected const string TITLE_FIELD = "MenuText";
		protected const string DESCRIPTION_FIELD = "Description";
		protected const string LANGUAGE_FIELD = "Language";
		protected const string CHILDCOUNT_FIELD = "ChildCount";
		protected const string NOTLOGONVISIBLE_FIELD = "IsNotLogonVisible";
		protected const string LOGONVISIBLE_FIELD = "IsLogonVisible";
		protected const string ADMINISTRATORS_FIELD = "IsAdministrator";
		protected const string LOCALVISIBLE_FIELD = "IsLocalVisible";

		private const string MENUITEM_GET = "MENU_GET";

		private string MENUITEM_GETROOTNODE = "MENU_GETROOTNODE";

		private int _ParentNodeId;
		private string _Url;
		private bool _Root;
		private bool _Visible;
		private int _MenuId;
		private string _MenuText;
		private string _Description;
		private string _Language;
		private bool _LogonVisible;
		private bool _NotLogonVisible;
		private bool _Administrators;
		private bool _LocalVisible;

		private int _ChildCount = 0;
		private MenuItemList _ChildNodes = null;

		#region properties

		public int NodeId
		{
			get { return Id; }
		}

		public int ParentNodeId
		{
			get { return _ParentNodeId; }
		}

		public string Url
		{
			get { return _Url; }
		}

		public string MenuText
		{
			get { return _MenuText; }
		}

		public string Description
		{
			get { return _Description; }
		}

		public string Language
		{
			get { return _Language; }
		}

		public bool Root
		{
			get { return _Root; }
		}

		public int MenuId
		{
			get { return _MenuId; }
		}

		public MenuItemList ChildNodes
		{
			get { return _ChildNodes; }
			set { _ChildNodes = value; }
		}

		protected int ChildCount
		{
			get { return _ChildCount; }
		}

		public bool Visible
		{
			get { return _Visible; }
		}

		public bool LogonVisible
		{
			get { return _LogonVisible; }
			set { _LogonVisible = value; }
		}

		public bool Administrators
		{
			get { return _Administrators; }
			set { _Administrators = value; }
		}

		public bool NotLogonVisible
		{
			get { return _NotLogonVisible; }
			set { _NotLogonVisible = value; }
		}

		public bool LocalVisible
		{
			get { return _LocalVisible; }
			set { _LocalVisible = value; }
		}

		#endregion

		public MenuItem()
		{
		}

		public MenuItem(int nodeId) : this(nodeId, false)
		{
		}

		public MenuItem(int nodeId, bool isRootNode)
		{
			WithLanguage = true;

			if (nodeId > 0)
			{
				if (isRootNode)
				{
					GetItem(MENUITEM_GETROOTNODE, nodeId);
				}
				else
				{
					GetItem(MENUITEM_GET, nodeId);
				}
			}
		}

		protected override void GetData()
		{
			_ParentNodeId = GetFieldInt32(PARENTNODEID_FIELD);
			_Url = GetFieldString(URL_FIELD);
			_Root = GetFieldBool(ROOT_FIELD);
			_Visible = GetFieldBool(VISIBLE_FIELD);
			_MenuId = GetFieldInt32(MENUID_FIELD);
			_MenuText = GetFieldString(TITLE_FIELD);
			_Description = GetFieldString(DESCRIPTION_FIELD);
			_Language = GetFieldString(LANGUAGE_FIELD);
			_ChildCount = GetFieldInt32(CHILDCOUNT_FIELD);
			_LogonVisible = GetFieldBool(LOGONVISIBLE_FIELD);
			_NotLogonVisible = GetFieldBool(NOTLOGONVISIBLE_FIELD);
			_Administrators = GetFieldBool(ADMINISTRATORS_FIELD);
			_LocalVisible = GetFieldBool(LOCALVISIBLE_FIELD);

			if (ChildCount > 0)
			{
				BuildChildMenuItemList(this, MenuId);
			}
		}

		protected override void InitializeDataItem()
		{
			AutoIdField = NODEID_FIELD;
		}

		private static void BuildChildMenuItemList(MenuItem menuItem, int parentId)
		{
			MenuItemList menuItemList = new MenuItemList(parentId);

			menuItem.ChildNodes = menuItemList;
		}
	}
}