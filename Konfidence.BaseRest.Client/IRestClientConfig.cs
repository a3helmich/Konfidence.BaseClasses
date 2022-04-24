using System;

namespace Konfidence.BaseRest.Client
{
    public interface IRestClientConfig
    {
        int PortNr { get; set; }

        string Address { get; set; }

        string BaseRoute { get; set; }

        string Route { get; set; }

        Uri BaseUri();
    }
}