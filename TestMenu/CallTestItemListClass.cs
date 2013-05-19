using System.Collections.Generic;

namespace MenuTest
{
    public class CallTestItemListClass
    {
        private readonly ITestItemClassList _TestItemList = new TestItemClassList();

        protected ITestItemClassList TestItemList
        {
            get { return _TestItemList; } // as List<ITestItemClass>; }
        }

        public void MethodOne()
        {
            var x = new TestItemClassList();

            //var y = x.IsReadOnly;

            foreach (var testItem in TestItemList)
            {
                testItem.MethodOne();
            }
        }
    }
}
