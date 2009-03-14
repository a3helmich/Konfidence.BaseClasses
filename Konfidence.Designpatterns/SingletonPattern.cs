using System;
using System.Reflection;
using System.Collections;
using System.Threading;

namespace Konfidence.DesignPatterns
{
	public interface ISingleton
  {
		// NOP
	}
    
  internal class SingletonException : Exception 
  {
    public SingletonException(string message): base(message){}
  }
  
  public class SingletonFactory
  {
    static private Hashtable SingletonTable = new Hashtable();
    static private TypeFilter SingletonFilter = new TypeFilter(SingletonInterfaceFilter);

    static private bool SingletonInterfaceFilter(Type typeObject, Object criteriaObject)
    {
      if(typeObject.ToString() == criteriaObject.ToString())
        return true;
      else
        return false;
    }
    
    // WORMGAATJES !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		static protected ISingleton Instance(Type singletonType)
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


