using System;

namespace Konfidence.Security
{
    public class Configuration : IConfiguration
    {
        public PlatformID OSVersionPlatform => Environment.OSVersion.Platform;
    }
}
