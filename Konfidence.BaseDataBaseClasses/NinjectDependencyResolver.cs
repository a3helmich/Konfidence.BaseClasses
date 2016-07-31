using Konfidence.Base;
using Konfidence.BaseData.Repositories;
using Ninject;
using Ninject.Syntax;

namespace Konfidence.BaseData
{
    public class NinjectDependencyResolver : BaseItem
    {
        private static IKernel _kernel;

        public IKernel Kernel => _kernel;

        public NinjectDependencyResolver()
        {
            if (!_kernel.IsAssigned())
            {
                _kernel = new StandardKernel();

                AddBindings();
            }
        }

        public IBindingToSyntax<T> Bind<T>()
        {
            return _kernel.Bind<T>();
        }

        private void AddBindings()
        {
            Bind<IDatabaseRepository>().To<DatabaseRepository>();
        }
    }
}
