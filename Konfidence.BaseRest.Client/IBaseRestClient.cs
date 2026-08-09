using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Konfidence.BaseRest.Client;

/// <summary>
/// A thin REST client abstraction for making JSON POST/GET requests against a configured base URI.
/// </summary>
public interface IBaseRestClient
{
    /// <summary>
    /// Do a POST request with (or without) an object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="relativePath"></param>
    /// <param name="requestObject"></param>
    /// <param name="headerParameters"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>T</returns>
    [UsedImplicitly]
    Task<T?> PostAsync<T>(string relativePath, object requestObject, Dictionary<string, string>? headerParameters = null, CancellationToken cancellationToken = default) where T : notnull, new();

    /// <summary>
    /// Do a GET request
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="relativePath"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>T</returns>
    [UsedImplicitly]
    Task<T?> GetAsync<T>(string relativePath, CancellationToken cancellationToken = default) where T : notnull, new();
}
