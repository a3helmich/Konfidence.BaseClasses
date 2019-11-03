using System;

namespace Konfidence.Security
{
    public interface ISecurityConfiguration
    {
        PlatformID OSVersionPlatform { get; }

        string Framework { get; }
    }
}
