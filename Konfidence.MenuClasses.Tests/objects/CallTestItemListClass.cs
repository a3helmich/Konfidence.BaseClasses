using System.Collections.Generic;
using Konfidence.MenuClasses.Tests.interfaces;
using System.Diagnostics.CodeAnalysis;

namespace Konfidence.MenuClasses.Tests.objects
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
