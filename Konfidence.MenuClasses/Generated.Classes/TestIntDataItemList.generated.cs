using System;
using Konfidence.Base;
using Konfidence.BaseData;
using Konfidence.BaseDataInterfaces;
using Konfidence.SqlHostProvider.SqlAccess;

namespace DbMenuClasses
{
    public partial class Bl
    {
        public partial class TestIntDataItemList : BaseDataItemList<TestIntDataItem>
        {
            // partial methods
            partial void BeforeInitializeDataItemList();
            partial void AfterInitializeDataItemList();

            private const string TESTINT_GETLIST = "gen_TestInt_GetList";

            protected TestIntDataItemList() : base()
            {
            }

            protected override IBaseClient ClientBind()
            {
                return base.ClientBind<SqlClient>();
            }

            static public TestIntDataItemList GetEmptyList()
            {
                TestIntDataItemList testintList = new TestIntDataItemList();

                return testintList;
            }

            static public TestIntDataItemList GetList()
            {
                TestIntDataItemList testintList = new TestIntDataItemList();

                testintList.BeforeInitializeDataItemList();

                testintList.BuildItemList(TESTINT_GETLIST);

                return testintList;
            }

            public TestIntDataItemList FindAll()
            {
                TestIntDataItemList testintList = new TestIntDataItemList();

                foreach (TestIntDataItem testint in this)
                {
                    testintList.Add(testint);
                }

                return testintList;
            }
        }
    }
}
