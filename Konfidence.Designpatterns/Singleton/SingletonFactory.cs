using System;
using System.Collections;
using System.Reflection;
using System.Threading;
using JetBrains.Annotations;
using Konfidence.Base;

namespace Konfidence.DesignPatterns.Singleton
{
    public class SingletonFactory
    {
        private static readonly Hashtable SingletonTable = new Hashtable();
        private static readonly TypeFilter SingletonFilter = SingletonInterfaceFilter;

        private static bool SingletonInterfaceFilter([NotNull] Type typeObject, [NotNull] object criteriaObject)
        {
            if (typeObject.ToString() == criteriaObject.ToString())
            {
                return true;
            }

            return false;
        }

        // WORMGAATJES !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        protected static ISingleton GetInstance(Type singletonType)
        {
            if (!singletonType.IsAssigned())
            {
                return null;
            }

            const string warningMessage = ": class must implement ISingleton interface"; 

            var iSingletonName = typeof(ISingleton).FullName;

            var singletonInterfaces = singletonType.FindInterfaces(SingletonFilter, iSingletonName);

            if (singletonInterfaces.Length == 0)
            {
                throw new SingletonException(singletonType + warningMessage);
            }

            var mutex = new Mutex();
            mutex.WaitOne();

            var singleton = SingletonTable[singletonType];

            if (!singleton.IsAssigned())
            {
                singleton = Activator.CreateInstance(singletonType);

                SingletonTable.Add(singletonType, singleton);
            }

            mutex.Close();

            return singleton as ISingleton;
        }
    }
}
