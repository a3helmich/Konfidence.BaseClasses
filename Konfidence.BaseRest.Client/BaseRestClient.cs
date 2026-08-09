using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Konfidence.Base;
using RestSharp;

namespace Konfidence.BaseRest.Client;

[UsedImplicitly]
public class BaseRestClient : IBaseRestClient, IDisposable
{
    private RestClient RestClient { get; }

    public BaseRestClient(IRestClientConfig clientConfig)
    {
        RestClientOptions restClientOptions = new(clientConfig.BaseUri());

        RestClient = new RestClient(restClientOptions);
    }

    public void Dispose()
    {
        RestClient.Dispose();
    }

    public async Task<T?> PostAsync<T>(string relativePath, object requestObject, Dictionary<string, string>? headerParameters = null, CancellationToken cancellationToken = default) where T : notnull, new()
    {
        return await ExecuteMethodAsync<T>(relativePath, Method.Post, requestObject, headerParameters, cancellationToken);
    }

    public async Task<T?> GetAsync<T>(string relativePath, CancellationToken cancellationToken = default) where T : notnull, new()
    {
        return await ExecuteMethodAsync<T>(relativePath, Method.Get, cancellationToken: cancellationToken);
    }

    private async Task<T?> ExecuteMethodAsync<T>(string relativePath, Method httpMethod, object? requestObject = null, Dictionary<string, string>? headerParameters = null, CancellationToken cancellationToken = default) where T : notnull, new()
    {
        RestRequest request = new()
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
            foreach (KeyValuePair<string, string> kvp in headerParameters)
            {
                request.AddHeader(kvp.Key, kvp.Value);
            }
        }

        RestResponse<T> response = await RestClient.ExecuteAsync<T>(request, cancellationToken);

        if (response.ResponseStatus != ResponseStatus.Completed)
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

        return JsonSerializer.Deserialize<T>(response.Content ?? string.Empty, new JsonSerializerOptions(JsonSerializerDefaults.Web));
    }
}
