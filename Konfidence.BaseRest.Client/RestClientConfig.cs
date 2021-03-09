using System;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;

namespace Konfidence.BaseRest.Client
{
    public class RestClientConfig : IRestClientConfig
    {
        public int PortNr { get; set; }

        public string Address { get; set; }

        public string BaseRoute { get; set; }

        public string Route { get; set; }

        public RestClientConfig([NotNull] IConfiguration configuration)
        {
            var section = configuration.GetSection(@"WebHost");

            section.Bind(this);
        }

        [NotNull]
        public Uri BaseUri()
        {
            return new($"{Host()}{BaseRoute}/{Route}");
        }

        [NotNull]
        public Uri Host()
        {
            const string prefix = @"http";

            return new Uri($"{prefix}://{Address}:{PortNr}");
        }
    }
}