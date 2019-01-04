namespace Tuneage.WebApi.Tests.Integration.Controllers.Api
{
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Tuneage.WebApi;
    using Xunit;

    public class LabelIntegrationTests
    {

        private readonly HttpClient client;

        public LabelIntegrationTests()
        {
            var server = new TestServer(new WebHostBuilder()
                .UseEnvironment("Development")
                .UseStartup<Startup>());
            this.client = server.CreateClient();
        }

        [Theory]
        [InlineData("GET")]
        public async Task LabelGetAllTestAsync(string method)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), "/api/Label/");

            // Act
            var response = await this.client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("GET", 1)]
        public async Task LabelGetTestAsync(string method, int? id = null)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"/api/Label/{id}");

            // Act
            var response = await this.client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
