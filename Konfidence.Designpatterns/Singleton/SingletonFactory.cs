﻿using System.Collections;
using System.Reflection;
using System.Threading;
using System;
using System.Collections.Generic;
using System.Text;

namespace Konfidence.DesignPatterns.Singleton
{
    public class SingletonFactory
    {
        static private Hashtable SingletonTable = new Hashtable();
        static private TypeFilter SingletonFilter = new TypeFilter(SingletonInterfaceFilter);

        static private bool SingletonInterfaceFilter(Type typeObject, Object criteriaObject)
        {
            if (typeObject.ToString() == criteriaObject.ToString())
                return true;
            else
                return false;
        }

        // WORMGAATJES !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        static protected ISingleton GetInstance(Type singletonType)
        {
            if (singletonType == null)
                return null;

            Object singleton;
            string WarningMessage = ": class must implement ISingleton interface";

            string ISingletonName = typeof(ISingleton).FullName;

            Type[] SingletonInterfaces = singletonType.FindInterfaces(SingletonFilter, ISingletonName);
            if (SingletonInterfaces.Length == 0)
                throw new SingletonException(singletonType + WarningMessage);

            Mutex mutex = new Mutex();
            mutex.WaitOne();

            singleton = SingletonTable[singletonType];
            if (singleton == null)
            {
                singleton = Activator.CreateInstance(singletonType);
                SingletonTable.Add(singletonType, singleton);
            }

            mutex.Close();

            return singleton as ISingleton;
        }
    }
}