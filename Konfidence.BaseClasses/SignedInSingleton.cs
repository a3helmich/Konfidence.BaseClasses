//using Konfidence.DesignPatterns;

//namespace Konfidence.Base
//{
//  public interface ISignedInSingleton: ISingleton
//  {
//    bool IsLoggedOn
//    {
//      get;
//      set;
//    }
//  }

//  internal class SignedInSingleton: ISignedInSingleton
//  {
//    private bool _IsLoggedOn;

//    public bool IsLoggedOn
//    {
//      get
//      {
//        return _IsLoggedOn;
//      }
//      set
//      {
//        _IsLoggedOn = value;
//      }
//    }
//  }

//  public class SignedInSingletonFactory: SingletonFactory
//  {
//    static public ISignedInSingleton SignedInSingleton()
//    {
//      return Instance(typeof(SignedInSingleton)) as ISignedInSingleton;
//    }
//  }
//}
