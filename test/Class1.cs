using Konfidence.Base;
using System;
using JetBrains.Annotations;

namespace test
{
    public class Class1
    {
        public void Initialize()
        {
            var testClass = Factory();

            if (testClass.IsAssigned())
            {
                Console.WriteLine(testClass.ToString());
            }

            Console.WriteLine(testClass.GetType().Name);
        }

        [CanBeNull]
        private Class1 Factory()
        {
            return new Class1();
        }
    }
}
