using System;
using Konfidence.Base;
using Konfidence.BaseData;
using Konfidence.DataBaseInterface;
using Konfidence.SqlHostProvider.SqlAccess;

namespace DbMenuClasses
{
    public partial class Bl
    {
        public partial class MenuTextDataItemList : BaseDataItemList<MenuTextDataItem>
        {
            private const string MENUTEXT_GETLIST = "gen_MenuText_GetList";

            public MenuTextDataItemList() : base()
            {
            }

            protected override IBaseClient ClientBind()
            {
                return base.ClientBind<SqlClient>();
            }

            static public MenuTextDataItemList GetList()
            {
                MenuTextDataItemList menutextList = new MenuTextDataItemList();

                menutextList.BuildItemList(MENUTEXT_GETLIST);

                return menutextList;
            }
        }
    }
}
