using System.Diagnostics.CodeAnalysis;
using Konfidence.MenuClasses.Tests.interfaces;

namespace Konfidence.MenuClasses.Tests.objects
{
    [ExcludeFromCodeCoverage]
    public class TestItemClass : ITestItemClass
    {
        public string Something { get; }

        public TestItemClass(string something)
        {
            Something = something;
        }

        public void MethodOne()
        {
        }

        public void MethodTwo()
        {
        }
    }
}
