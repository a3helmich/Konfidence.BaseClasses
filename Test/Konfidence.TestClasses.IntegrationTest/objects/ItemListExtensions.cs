using System.Collections.Generic;
using Konfidence.TestClasses.IntegrationTest.interfaces;

namespace Konfidence.TestClasses.IntegrationTest.objects
{
    public static class ItemListExtensions
    {
        public static void AddItem(this List<ITestItemClass> itemList)
        {
            itemList.Add(new TestItemClass("testString"));
        }
    }
}
