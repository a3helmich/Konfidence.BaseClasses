using System;
using Konfidence.Base;
using Konfidence.BaseData;
using Konfidence.DataBaseInterface;
using Konfidence.SqlHostProvider.SqlAccess;

namespace DbMenuClasses
{
    public partial class Bl
    {
        public partial class TestIntDataItemList : BaseDataItemList<TestIntDataItem>
        {
            private const string TESTINT_GETLIST = "gen_TestInt_GetList";

            public TestIntDataItemList() : base()
            {
            }

            protected override IBaseClient ClientBind()
            {
                return base.ClientBind<SqlClient>();
            }

            static public TestIntDataItemList GetList()
            {
                TestIntDataItemList testintList = new TestIntDataItemList();

                testintList.BuildItemList(TESTINT_GETLIST);

                return testintList;
            }
        }
    }
}
