using Konfidence.TestClasses.Tests.interfaces;

namespace Konfidence.TestClasses.Tests.objects
{
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
