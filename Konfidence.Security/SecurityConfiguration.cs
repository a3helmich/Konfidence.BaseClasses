using System;
using System.Reflection;
using System.Runtime.Versioning;
using JetBrains.Annotations;
using Konfidence.Base;

namespace Konfidence.Security
{
    internal class SecurityConfiguration : ISecurityConfiguration
    {
        private string _framework;

        public PlatformID OSVersionPlatform { get; set; }

        [CanBeNull] public string Framework { get; set; }

        internal SecurityConfiguration()
        {
            OSVersionPlatform = Environment.OSVersion.Platform;
            Framework = GetFramework();
        }

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
