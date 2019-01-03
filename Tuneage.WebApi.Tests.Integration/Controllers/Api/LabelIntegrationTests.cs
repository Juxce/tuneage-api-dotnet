//-----------------------------------------------------------------------
// <copyright file="LabelIntegrationTests.cs" company="Tuneage">
//     (c) 2018 Tuneage
// </copyright>
//-----------------------------------------------------------------------
namespace Tuneage.WebApi.Tests.Integration.Controllers.Api
{
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Tuneage.WebApi;
    using Xunit;

    /// <summary>
    /// Tunage Label Integration Tests
    /// </summary>
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
            var request = new HttpRequestMessage(new HttpMethod(method), "/api/Album/");

            // Act
            var response = await this.client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        /// <summary>
        /// The album get test async.
        /// </summary>
        /// <param name="method">
        /// The method.
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Theory]
        [InlineData("GET", 1)]
        public async Task AlbumGetTestAsync(string method, int? id = null)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"/api/Album/{id}");

            // Act
            var response = await this.client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}


