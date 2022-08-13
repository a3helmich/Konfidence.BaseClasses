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
        private RestClient RestClient { get; }

        public BaseRestClient(IRestClientConfig clientConfig)
        {
            var restClientOptions = new RestClientOptions(clientConfig.BaseUri());

            RestClient = new RestClient(restClientOptions);
        }

        public async Task<T?> PostAsync<T>(string relativePath, object requestObject, Dictionary<string, string>? headerParameters = null) where T : new()
        {
            return await ExecuteMethodAsync<T>(relativePath, Method.Post, requestObject, headerParameters);
        }

        public async Task<T?> GetAsync<T>(string relativePath) where T : new()
        {
            return await ExecuteMethodAsync<T>(relativePath, Method.Get);
        }

        private async Task<T?> ExecuteMethodAsync<T>(string relativePath, Method httpMethod, object? requestObject = null, Dictionary<string, string>? headerParameters = null) where T : new()
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
                if (!response.ErrorException.IsAssigned())
                {
                    return default;
                }

                throw response.ErrorException;
            }

            if (!response.Content.IsAssigned() && response.StatusCode != HttpStatusCode.OK)
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(response.Content??string.Empty, new JsonSerializerOptions(JsonSerializerDefaults.Web));
        }
    }
}