namespace Konfidence.Test.MenuClasses
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
            foreach (var testItem in TestItemList)
            {
                testItem.MethodOne();
            }
        }

        public void MethodTwo()
        {
            var testList = new TestItemClassList();

            testList.AddItem();
        }
    }
}
