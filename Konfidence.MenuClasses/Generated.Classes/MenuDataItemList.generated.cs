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
        public partial class MenuDataItemList : BaseDataItemList<MenuDataItem>
        {
            private const string MENU_GETLIST = "gen_Menu_GetList";
            private const string MENU_GETLISTBY_MENUID = "gen_Menu_GetListByMenuId";

            private int _MenuId = 0;

            public MenuDataItemList() : base()
            {
            }

            protected override IBaseClient ClientBind()
            {
                return base.ClientBind<SqlClient>();
            }

            static public MenuDataItemList GetList()
            {
                MenuDataItemList menuList = new MenuDataItemList();

                menuList.BuildItemList(MENU_GETLIST);

                return menuList;
            }

            static public MenuDataItemList GetListByMenuId(int menuid)
            {
                MenuDataItemList menuList = new MenuDataItemList();

                menuList._MenuId = menuid;

                menuList.BuildItemList(MENU_GETLISTBY_MENUID);

                return menuList;
            }

            public override void SetParameters(string storedProcedure)
            {
                if (storedProcedure.Equals(MENU_GETLISTBY_MENUID))
                {
                    SetParameter(MenuDataItem.MENUID, _MenuId);
                }
            }
        }
    }
}
