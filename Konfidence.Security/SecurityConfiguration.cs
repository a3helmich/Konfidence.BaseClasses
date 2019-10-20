using System;
using System.Reflection;
using System.Runtime.Versioning;
using JetBrains.Annotations;
using Konfidence.Base;

namespace Konfidence.Security
{
    public class SecurityConfiguration : ISecurityConfiguration
    {
        private string _framework;

        public PlatformID OSVersionPlatform => Environment.OSVersion.Platform;

        [CanBeNull] public string Framework => GetFramework();

        [CanBeNull]
        private string GetFramework()
        {
            if (!_framework.IsAssigned())
            {
                _framework = Assembly.GetEntryAssembly()?.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName;
            }

            return _framework;
        }
    }
}
