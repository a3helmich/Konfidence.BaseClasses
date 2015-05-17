using Konfidence.BaseData;

namespace DbSiteMapMenuClasses
{
    public partial class Bl
    {
        public partial class MenuTextDataItemList : BaseDataItemList<MenuTextDataItem>
        {
            // partial methods
            partial void BeforeInitializeDataItemList();
            partial void AfterInitializeDataItemList();

            private const string MENUTEXT_GETLIST = "gen_MenuText_GetList";

            protected MenuTextDataItemList() : base()
            {
            }

            static public MenuTextDataItemList GetEmptyList()
            {
                MenuTextDataItemList menutextList = new MenuTextDataItemList();

                return menutextList;
            }

            static public MenuTextDataItemList GetList()
            {
                MenuTextDataItemList menutextList = new MenuTextDataItemList();

                menutextList.BuildItemList(MENUTEXT_GETLIST);

                return menutextList;
            }

            public MenuTextDataItemList FindAll()
            {
                MenuTextDataItemList menutextList = new MenuTextDataItemList();

                foreach (MenuTextDataItem menutext in this)
                {
                    menutextList.Add(menutext);
                }

                return menutextList;
            }
        }
    }
}
