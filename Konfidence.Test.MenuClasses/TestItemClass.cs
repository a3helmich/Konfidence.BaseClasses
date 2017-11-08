using System.Diagnostics.CodeAnalysis;

namespace Konfidence.MenuClasses.Tests
{
    [ExcludeFromCodeCoverage]
    public class TestItemClass : ITestItemClass
    {
        private string _Something;

        public string Something 
        {
            get { return _Something; }
        }

        public TestItemClass(string something)
        {
            _Something = something;
        }

        public void MethodOne()
        {
        }

        public void MethodTwo()
        {
        }
    }
}
