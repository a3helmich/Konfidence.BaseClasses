using System;

namespace Konfidence.Security
{
    public interface IConfiguration
    {
        PlatformID OSVersionPlatform { get; }
    }
}
