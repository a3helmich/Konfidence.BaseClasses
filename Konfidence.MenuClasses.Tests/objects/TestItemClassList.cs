﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Konfidence.BaseData;
using Konfidence.MenuClasses.Tests.interfaces;
using Ninject;
using Ninject.Parameters;

namespace Konfidence.MenuClasses.Tests.objects
{
    public class TestItemClassList : List<TestItemClass>, ITestItemClassList
    {
        public void AddItem()
        {
            var ninject = new NinjectDependencyResolver();

            ninject.Bind<ITestItemClass>().To<TestItemClass>();

            const string mystring = "testString";

            var param = new ConstructorArgument("something", mystring);

            var newItem = ninject.Kernel.Get<ITestItemClass>(param) as TestItemClass;

            Add(newItem);
        }
    }
}
