using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MenuTest
{
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
