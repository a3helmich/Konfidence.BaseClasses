using Konfidence.Base;
using Konfidence.BaseData.IRepositories;
using Konfidence.BaseData.Repositories;
using Ninject;
using Ninject.Syntax;

namespace Konfidence.BaseData
{
    public class NinjectDependencyResolver : BaseItem
    {
        static private IKernel _Kernel;

        public IKernel Kernel
        {
            get { return _Kernel; }
        }

        public NinjectDependencyResolver()
        {
            if (!IsAssigned(_Kernel))
            {
                _Kernel = new StandardKernel();

                AddBindings();
            }
        }

        public IBindingToSyntax<T> Bind<T>()
        {
            return _Kernel.Bind<T>();
        }

        private void AddBindings()
        {
            Bind<IDatabaseRepository>().To<DatabaseRepository>();
        }
    }
}
