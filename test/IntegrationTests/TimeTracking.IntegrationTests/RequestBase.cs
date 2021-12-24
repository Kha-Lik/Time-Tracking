using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using TimeTracking.Tests.Common;


namespace TimeTracking.IntegrationTests
{
    public class RequestBase<TStartup> : IDisposable where TStartup : class
    {
        private HttpClient _client;
        protected IntegrationTestsWebApplicationFactory<TStartup> Factory { get; set; }

        [SetUp]
        public virtual void SetUp()
        {
            _client = Factory.CreateClient();
        }

        protected string GetJwtToken(IEnumerable<Claim> claims = null)
        {
            return Factory.GenerateJwtToken(claims);
        }

        public T GetService<T>()
        {
            var scope = Factory.Services.CreateScope();
            return scope.ServiceProvider.GetService<T>();
        }

        public async Task ReSeedDatabase()
        {
            await Factory.ReSeedDatabase(Factory.Services);
        }

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            return await _client.GetAsync(url);
        }

        public RequestBase<TStartup> AddAuth(string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return this;
        }

        public async Task<HttpResponseMessage> PostAsync<T>(string url, T body)
        {
            return await _client.PostAsJsonAsync<T>(url, body);
        }

        public async Task<HttpResponseMessage> PutAsync<T>(string url, T body)
        {
            return await _client.PutAsJsonAsync<T>(url, body);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string url)
        {
            return await _client.DeleteAsync(url);
        }

        [TearDown]
        public void Dispose()
        {
            _client.Dispose();
            Factory.Dispose();
        }
    }
}