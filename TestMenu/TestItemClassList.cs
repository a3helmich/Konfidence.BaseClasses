using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using Ninject.Parameters;

namespace MenuTest
{
    public class TestItemClassList : List<TestItemClass>, ITestItemClassList
    {
        public void AddItem()
        {
            var ninject = new NinjectDependencyResolver();

            var mystring = "testString";

            var param = new ConstructorArgument("something", mystring);

            var newItem = ninject.Kernel.Get<ITestItemClass>(param) as TestItemClass;

            Add(newItem);
        }
    }
}
