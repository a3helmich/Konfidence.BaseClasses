using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Text.Json;
using JetBrains.Annotations;
using Konfidence.Base;
using RestSharp;

namespace Konfidence.BaseRest.Client
{
    [UsedImplicitly]
    public class BaseRestClient : IBaseRestClient
    {
        internal IRestClient RestClient { get; }

        internal string Route { get; }

        public BaseRestClient(IRestClient restClient, [NotNull] IRestClientConfig clientConfig)
        {
            RestClient = restClient;

            Route = clientConfig.Route;

            RestClient.BaseUrl = clientConfig.BaseUri();
        }

        [ItemCanBeNull]
        public async Task<T> PostAsync<T>(string relativePath, object requestObject, [CanBeNull] Dictionary<string, string> headerParameters = null) where T : new()
        {
            return await ExecuteMethodAsync<T>(relativePath, Method.POST, requestObject, headerParameters);
        }

        [ItemCanBeNull]
        public async Task<T> GetAsync<T>(string relativePath) where T : new()
        {
            return await ExecuteMethodAsync<T>(relativePath, Method.GET);
        }

        [ItemCanBeNull]
        private async Task<T> ExecuteMethodAsync<T>(string relativePath, Method httpMethod, [CanBeNull] object requestObject = null, [CanBeNull] Dictionary<string, string> headerParameters = null) where T : new()
        {
            var request = new RestRequest
            {
                Resource = relativePath,
                RequestFormat = DataFormat.Json,
                Method = httpMethod,
            };

            if (requestObject.IsAssigned())
            {
                request.AddJsonBody(requestObject);
            }

            if (headerParameters.IsAssigned())
            {
                foreach (var kvp in headerParameters)
                {
                    request.AddHeader(kvp.Key, kvp.Value);
                }
            }

            var response = await RestClient.ExecuteAsync<T>(request);

            if (response.ResponseStatus == ResponseStatus.Error)
            {
                throw response.ErrorException;
            }

            if (!response.Content.IsAssigned() && response.StatusCode != HttpStatusCode.OK)
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(response.Content);
        }
    }
}