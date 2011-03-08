using System.Data;
using System.Data.Common;
using Konfidence.BaseData;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Konfidence.BaseUserControlHelpers.DbSiteMapProvider
{
	public class MenuItemList : BaseDataItemList<MenuItem>
	{
		private string MENUITEM_GETLIST = "MENU_GETLIST";

		private const string MENUITEM_SERVICE = "MenuItemService";

		private int _ParentId;

		public MenuItemList(int parentId)
		{
			_ParentId = parentId;

			BuildItemList();
		}

		protected override void InitializeDataItemList()
		{
			base.InitializeDataItemList();

			GetListStoredProcedure = MENUITEM_GETLIST;

			ServiceName = MENUITEM_SERVICE;
		}

        public override void SetParameters(string storedProcedure, Database database, DbCommand dbCommand)
        {
            base.SetParameters(storedProcedure, database, dbCommand);

            database.AddInParameter(dbCommand, MenuItem.PARENTNODEID_FIELD, DbType.Int32, _ParentId);
        }

		protected override MenuItem GetNewDataItem()
		{
			return new MenuItem();
		}
	}
}