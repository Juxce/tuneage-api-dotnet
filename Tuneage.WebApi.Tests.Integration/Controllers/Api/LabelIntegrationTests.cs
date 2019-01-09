namespace Tuneage.WebApi.Tests.Integration.Controllers.Api
{
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Tuneage.WebApi;
    using Xunit;

    public class LabelIntegrationTests
    {

        private readonly HttpClient _httpClient;

        public LabelIntegrationTests()
        {
            var server = new TestServer(new WebHostBuilder()
                .UseEnvironment("Development")
                .UseStartup<Startup>());
            this._httpClient = server.CreateClient();
        }

        [Fact(Skip= "Skipping!")]
        public async Task LabelGetAllTestAsync()
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod("GET"), "/api/Labels/");

            // Act
            var response = await this._httpClient.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(Skip="Skipping!")]
        public async Task LabelGetTestAsync()
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod("GET"), $"/api/Label/{1}");

            // Act
            var response = await this._httpClient.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
