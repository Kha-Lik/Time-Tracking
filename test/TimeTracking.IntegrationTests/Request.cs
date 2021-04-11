using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using TimeTracking.Dal.Impl;
using TimeTracking.Tests.Common;

namespace TimeTracking.IntegrationTests
{
    [TestFixture]
    public class Request<TStartup> : IDisposable where TStartup : class
    {
        protected HttpClient _client;
        private CustomWebApplicationFactory<TStartup> _factory;
        
        [SetUp]
        public virtual void SetUp()
        {
            _factory = new CustomWebApplicationFactory<TStartup>();
            _client = _factory.CreateClient();
        }

        public T GetService<T>()
        {
            var scope =  _factory.Services.CreateScope();
            return scope.ServiceProvider.GetService<T>();
        }

        //public JwtAuthentication Jwt => new JwtAuthentication(ConfigurationSingleton.GetConfiguration());

        public  void ReSeedDatabase()
        {
            _factory.ReSeedDatabase(_factory.Services);
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