namespace Konfidence.BaseUserControlHelpers.DbSiteMapProvider
{
	public class MenuItemTree: MenuItem 
	{
		private MenuItemTree()
		{
		}
		
		public MenuItemTree(int menuId): base(menuId, true)
		{
		}

		protected override void InitializeDataItem()
		{
			AutoIdField = MENUID_FIELD;
		}
	}	
}
