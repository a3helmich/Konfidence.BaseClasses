using System;
using Konfidence.BaseData;
using Konfidence.BaseDataInterfaces;

namespace DbSiteMapMenuClasses
{
    public partial class Bl
    {
        public partial class MenuDataItemList : BaseDataItemList<MenuDataItem>
        {
            // partial methods
            partial void BeforeInitializeDataItemList();
            partial void AfterInitializeDataItemList();

            private const string MENU_GETLIST = "gen_Menu_GetList";
            private const string MENU_GETLISTBY_MENUCODE = "gen_Menu_GetListByMenuCode";

            private int _MenuCode = 0;

            protected MenuDataItemList() : base()
            {
            }

            protected override IBaseClient ClientBind<SqlClient>()
            {
                return base.ClientBind<SqlClient>();
            }

            static public MenuDataItemList GetEmptyList()
            {
                MenuDataItemList menuList = new MenuDataItemList();

                return menuList;
            }

            static public MenuDataItemList GetList()
            {
                MenuDataItemList menuList = new MenuDataItemList();

                menuList.BuildItemList(MENU_GETLIST);

                return menuList;
            }

            static public MenuDataItemList GetListByMenuCode(int menucode)
            {
                MenuDataItemList menuList = new MenuDataItemList();

                menuList._MenuCode = menucode;

                menuList.BuildItemList(MENU_GETLISTBY_MENUCODE);

                return menuList;
            }

            public MenuDataItem FindByParentMenuId(Guid parentmenuid)
            {
                foreach (MenuDataItem menu in this)
                {
                    if (menu.ParentMenuId.Equals(parentmenuid))
                    {
                        return menu;
                    }
                }

                return null;
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

            public MenuDataItemList FindListByParentMenuId(Guid parentmenuid)
            {
                MenuDataItemList menuList = new MenuDataItemList();

                foreach (MenuDataItem menu in this)
                {
                    if (menu.ParentMenuId.Equals(parentmenuid))
                    {
                        menuList.Add(menu);
                    }
                }

                return menuList;
            }

            public override void SetParameters(string storedProcedure)
            {
                if (storedProcedure.Equals(MENU_GETLISTBY_MENUCODE))
                {
                    SetParameter(MenuDataItem.MENUCODE, _MenuCode);
                }
            }
        }
    }
}
