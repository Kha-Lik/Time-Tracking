using System.Net.Http;
using System.Threading.Tasks;

namespace TimeTracking.Common.HttpClientHandler
{
    public interface IHttpProvider
    {
        void SetBearerAuthorization(string token);
        Task<HttpResponseMessage> GetAsync(string requestUri);
        Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content);
        Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content);
        Task<HttpResponseMessage> DeleteAsync(string requestUri);
    }
}