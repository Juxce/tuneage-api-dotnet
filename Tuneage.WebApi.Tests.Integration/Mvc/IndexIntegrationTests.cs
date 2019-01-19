using Xunit;

namespace Tuneage.WebApi.Tests.Integration.Mvc
{
    public class IndexIntegrationTests : IntegrationTestFixture
    {
        [Fact]
        public async void IndexGet_ShouldReturnIndexHtmlPage()
        {
            // Arrange

            // Act
            var response = await Client.GetAsync("/");
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Contains("<title>Home Page - Tuneage.WebApi</title>", responseString);
        }
    }
}
