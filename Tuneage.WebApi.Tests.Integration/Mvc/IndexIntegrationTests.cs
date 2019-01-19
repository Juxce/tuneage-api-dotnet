using System;
using System.Net.Http;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace Tuneage.WebApi.Tests.Integration.Mvc
{
    public class IndexIntegrationTests : IDisposable
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public IndexIntegrationTests()
        {
            _server = new TestServer(WebHost.CreateDefaultBuilder().UseStartup<TestStartup>().UseEnvironment("Development"));
            _client = _server.CreateClient();
        }

        [Fact]
        public async void IndexGet_ShouldReturnIndexHtmlPage()
        {
            // Arrange

            // Act
            var response = await _client.GetAsync("/");
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Contains("<title>Home Page - Tuneage.WebApi</title>", responseString);
        }

        public void Dispose()
        {
            _client.Dispose();
            _server.Dispose();
        }
    }
}
