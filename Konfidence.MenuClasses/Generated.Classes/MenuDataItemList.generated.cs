using System;
using System.Data;
using Konfidence.BaseData;

namespace Konfidence.DbSiteMapMenuClasses
{
	public partial class Bl
	{
		public partial class MenuDataItemList : BaseDataItemList<MenuDataItem>
		{
			// partial methods
			partial void BeforeInitializeDataItemList();
			partial void AfterInitializeDataItemList();
			
			private const string MENU_GETLIST = "gen_Menu_GetList";
			
			protected MenuDataItemList() : base()
			{
			}
			
			protected sealed override void InitializeDataItemList()
			{
				BeforeInitializeDataItemList();
				
				GetListStoredProcedure = MENU_GETLIST;
				
				AfterInitializeDataItemList();
			}
			
			static public MenuDataItemList GetEmptyList()
			{
				MenuDataItemList menuList = new MenuDataItemList();
				
				return menuList;
			}
			
			static public MenuDataItemList GetList()
			{
				MenuDataItemList menuList = new MenuDataItemList();
				
				menuList.BuildItemList();
				
				return menuList;
			}
			
			public MenuDataItemList FindAll()
			{
				MenuDataItemList menuList = new MenuDataItemList();
				
				foreach (MenuDataItem menu in this)
				{
					menuList.Add(menu);
				}
				
				return menuList;
			}
		}
	}
}
