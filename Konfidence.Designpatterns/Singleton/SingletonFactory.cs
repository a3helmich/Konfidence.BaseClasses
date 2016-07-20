using System;
using System.Collections;
using System.Reflection;
using System.Threading;
using Konfidence.Base;

namespace Konfidence.DesignPatterns.Singleton
{
    public class SingletonFactory : BaseItem
    {
        static private readonly Hashtable SingletonTable = new Hashtable();
        static private readonly TypeFilter SingletonFilter = SingletonInterfaceFilter;

        static private bool SingletonInterfaceFilter(Type typeObject, Object criteriaObject)
        {
            if (typeObject.ToString() == criteriaObject.ToString())
            {
                return true;
            }

            return false;
        }

        // WORMGAATJES !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        static protected ISingleton GetInstance(Type singletonType)
        {
            if (!singletonType.IsAssigned())
            {
                return null;
            }

            const string warningMessage = ": class must implement ISingleton interface"; 

            string iSingletonName = typeof(ISingleton).FullName;

            Type[] singletonInterfaces = singletonType.FindInterfaces(SingletonFilter, iSingletonName);

            if (singletonInterfaces.Length == 0)
            {
                throw new SingletonException(singletonType + warningMessage);
            }

            var mutex = new Mutex();
            mutex.WaitOne();

            object singleton = SingletonTable[singletonType];

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
