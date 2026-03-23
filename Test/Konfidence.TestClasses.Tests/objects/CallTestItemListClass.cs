using System.Collections.Generic;
using Konfidence.TestClasses.Tests.interfaces;

namespace Konfidence.TestClasses.Tests.objects
{
    public class CallTestItemListClass
    {
        protected List<ITestItemClass> TestItemList { get; } = [];

        public void MethodOne()
        {
            foreach (ITestItemClass testItem in TestItemList)
            {
                testItem.MethodOne();
            }
        }

        public void MethodTwo()
        {
            List<ITestItemClass> testList = [];

            testList.AddItem();
        }
    }
}
