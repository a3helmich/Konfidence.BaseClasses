using Konfidence.MenuClasses.Tests.interfaces;
using System.Diagnostics.CodeAnalysis;

namespace Konfidence.MenuClasses.Tests.objects
{
    public class CallTestItemListClass
    {
        protected ITestItemClassList TestItemList { get; } = new TestItemClassList();// as List<ITestItemClass>; }

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
