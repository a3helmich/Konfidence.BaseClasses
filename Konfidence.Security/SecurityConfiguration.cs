using System;
using System.Reflection;
using System.Runtime.Versioning;

namespace Konfidence.Security;

internal class SecurityConfiguration : ISecurityConfiguration
{
    public PlatformID OSVersionPlatform { get; set; }

    public string? Framework { get; set; }

    internal SecurityConfiguration()
    {
        OSVersionPlatform = Environment.OSVersion.Platform;
        Framework = Assembly.GetEntryAssembly()?.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName;
    }
}
