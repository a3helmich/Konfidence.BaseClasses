using System.Collections.Generic;
using JetBrains.Annotations;
using Konfidence.TestClasses.Tests.interfaces;

namespace Konfidence.TestClasses.Tests.objects
{
    public static class ItemListExtensions
    {
        public static void AddItem(this List<ITestItemClass> itemList)
        {
            itemList.Add(new TestItemClass("testString"));
        }
    }
}
