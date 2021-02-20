using System;
using Konfidence.SqlHostProvider;

namespace DbMenuClasses
{
    public partial class Bl
    {
        internal static IServiceProvider _serviceProvider;

        static Bl()
        {
            _serviceProvider = DependencyInjectionFactory.ConfigureDependencyInjection("--DefaultDatabase=DbMenu");
        }
    }
}
