using System.Collections.Generic;
using JetBrains.Annotations;
using Konfidence.MenuClasses.Tests.interfaces;

namespace Konfidence.MenuClasses.Tests.objects
{
    public static class ItemListExtensions
    {
        public static void AddItem([NotNull] this List<ITestItemClass> itemList)
        {
            itemList.Add(new TestItemClass("testString"));
        }
    }
}
