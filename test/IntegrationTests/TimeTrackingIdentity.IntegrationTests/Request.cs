using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace TimeTrackingIdentity.IntegrationTests
{
    [TestFixture]
    public class Request<TStartup> : IDisposable where TStartup : class
    {
        protected HttpClient _client;
        private IdentityWebApplicationFactory _factory;

        [SetUp]
        public virtual void SetUp()
        {
            _factory = new IdentityWebApplicationFactory();
            _client = _factory.CreateClient();
        }

        public T GetService<T>()
        {
            var scope = _factory.Services.CreateScope();
            return scope.ServiceProvider.GetService<T>();
        }

        public async Task ReSeedDatabase()
        {
            await _factory.ReSeedDatabase(_factory.Services);
        }

        public async Task<HttpResponseMessage> Get(string url)
        {
            return await _client.GetAsync(url);
        }

        public Request<TStartup> AddAuth(string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return this;
        }

        public async Task<HttpResponseMessage> Post<T>(string url, T body)
        {
            return await _client.PostAsJsonAsync<T>(url, body);
        }

        public async Task<HttpResponseMessage> Put<T>(string url, T body)
        {
            return await _client.PutAsJsonAsync<T>(url, body);
        }

        public async Task<HttpResponseMessage> Delete(string url)
        {
            return await _client.DeleteAsync(url);
        }

        [TearDown]
        public void Dispose()
        {
            _client.Dispose();
            _factory.Dispose();
        }
    }
}