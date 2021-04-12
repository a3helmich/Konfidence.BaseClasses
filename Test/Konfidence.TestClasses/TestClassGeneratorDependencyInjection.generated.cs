using System;
using Konfidence.SqlHostProvider;

namespace TestClasses
{
    public partial class Dl
    {
        internal static IServiceProvider _serviceProvider;

        static Dl()
        {
            _serviceProvider = DependencyInjectionFactory.ConfigureDependencyInjection("--DefaultDatabase=TestClassGenerator");
        }
    }
}
