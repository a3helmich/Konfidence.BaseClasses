using System;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;

namespace Konfidence.BaseRest.Client
{
    [UsedImplicitly]
    public class RestClientConfig : IRestClientConfig
    {
        public int PortNr { get; set; }

        public string Address { get; set; } = string.Empty;

        public string BaseRoute { get; set; } = string.Empty;

        public string Route { get; set; } = string.Empty;

        public RestClientConfig(IConfiguration configuration)
        {
            var section = configuration.GetSection(@"WebHost");

            section.Bind(this);
        }

        public Uri BaseUri()
        {
            return new($"{Host()}{BaseRoute}/{Route}");
        }

        private Uri Host()
        {
            const string prefix = @"http";

            return new Uri($"{prefix}://{Address}:{PortNr}");
        }
    }
}