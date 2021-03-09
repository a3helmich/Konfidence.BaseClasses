using System.Collections.Generic;
using System.Threading.Tasks;

namespace Konfidence.BaseRest.Client
{
    /// <summary>
    /// 
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
        /// <returns></returns>
        Task<T> PostAsync<T>(string relativePath, object requestObject, Dictionary<string, string> headerParameters = null) where T : new();

        /// <summary>
        /// Do a GET request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="relativePath"></param>
        /// <returns></returns>

        Task<T> GetAsync<T>(string relativePath) where T : new();
    }
}