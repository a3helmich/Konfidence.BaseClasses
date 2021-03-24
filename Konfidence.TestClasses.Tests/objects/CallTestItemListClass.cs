using System.Collections.Generic;
using Konfidence.TestClasses.Tests.interfaces;

namespace Konfidence.TestClasses.Tests.objects
{
    public class CallTestItemListClass
    {
        protected List<ITestItemClass> TestItemList { get; } = new List<ITestItemClass>();

        public void MethodOne()
        {
            foreach (var testItem in TestItemList)
            {
                testItem.MethodOne();
            }
        }

        public void MethodTwo()
        {
            var testList = new List<ITestItemClass>();

            testList.AddItem();
        }
    }
}
