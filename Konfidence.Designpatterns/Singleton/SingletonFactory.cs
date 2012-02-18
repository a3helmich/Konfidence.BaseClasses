using System.Collections;
using System.Reflection;
using System.Threading;
using System;
using System.Collections.Generic;
using System.Text;
using Konfidence.Base;

namespace Konfidence.DesignPatterns.Singleton
{
    public class SingletonFactory : BaseItem
    {
        static private Hashtable _SingletonTable = new Hashtable();
        static private TypeFilter _SingletonFilter = new TypeFilter(SingletonInterfaceFilter);

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
            if (!IsAssigned(singletonType))
            {
                return null;
            }

            Object singleton;
            string warningMessage = ": class must implement ISingleton interface"; 

            string iSingletonName = typeof(ISingleton).FullName;

            Type[] singletonInterfaces = singletonType.FindInterfaces(_SingletonFilter, iSingletonName);

            if (singletonInterfaces.Length == 0)
            {
                throw new SingletonException(singletonType + warningMessage);
            }

            Mutex mutex = new Mutex();
            mutex.WaitOne();

            singleton = _SingletonTable[singletonType];

            if (!IsAssigned(singleton))
            {
                singleton = Activator.CreateInstance(singletonType);

                _SingletonTable.Add(singletonType, singleton);
            }

            mutex.Close();

            return singleton as ISingleton;
        }
    }
}
