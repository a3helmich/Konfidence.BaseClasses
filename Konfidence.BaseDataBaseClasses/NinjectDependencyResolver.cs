using JetBrains.Annotations;
using Konfidence.Base;
using Ninject;
using Ninject.Syntax;
using Serilog;

namespace Konfidence.BaseData
{
    public class NinjectDependencyResolver
    {
        private static IKernel _kernel;

        public IKernel Kernel => _kernel;

        public NinjectDependencyResolver()
        {
            if (!_kernel.IsAssigned())
            {
                _kernel = new StandardKernel();
            }
        }

        [NotNull]
        public IBindingToSyntax<T> Bind<T>()
        {
            Log.Information($"Ninject Binding: ClientBind {typeof(T).FullName}");

            return _kernel.Bind<T>();
        }
    }
}
