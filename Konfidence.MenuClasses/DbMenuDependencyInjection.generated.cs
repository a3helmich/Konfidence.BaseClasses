using System;
using Konfidence.SqlHostProvider;

namespace DbMenuClasses
{
    public partial class Dl
    {
        internal static IServiceProvider _serviceProvider;

        static Dl()
        {
            _serviceProvider = DependencyInjectionFactory.ConfigureDependencyInjection("--DefaultDatabase=DbMenu");
        }
    }
}
