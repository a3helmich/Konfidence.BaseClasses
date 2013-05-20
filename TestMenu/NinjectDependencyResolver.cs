using Ninject;
using Ninject.Syntax;

namespace MenuTest
{
    public class NinjectDependencyResolver 
    {
        private readonly IKernel _Kernel;

        public IKernel Kernel
        {
            get { return _Kernel; }
        }

        public NinjectDependencyResolver()
        {
            _Kernel = new StandardKernel();

            AddBindings();
        }

        //public object GetService(Type serviceType)
        //{
        //    return Kernel.TryGet(serviceType);
        //}

        //public IEnumerable<object> GetServices(Type serviceType)
        //{
        //    return Kernel.GetAll(serviceType);
        //}

        public IBindingToSyntax<T> Bind<T>()
        {
            return _Kernel.Bind<T>();
        }

        private void AddBindings()
        {
            Bind<ITestItemClass>().To<TestItemClass>();
        }
    }
}
