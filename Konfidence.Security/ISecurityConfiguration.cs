using System;

namespace Konfidence.Security
{
    internal interface ISecurityConfiguration
    {
        PlatformID OSVersionPlatform { get; set; }

        string? Framework { get; set; }
    }
}
