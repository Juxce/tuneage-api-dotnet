using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Tuneage.Data.TestData;
using Xunit;

namespace Tuneage.WebApi.Tests.Integration.Mvc
{
    public class LabelsIntegrationTests : IntegrationTestFixture
    {
        [Fact]
        public async Task AllGet_ShouldReturnViewWithLabelsData()
        {
            // Arrange

            // Act
            var response = await Client.GetAsync("/labels");
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Contains("<title>Index - Tuneage.WebApi</title>", responseString);
            foreach (var label in TestDataGraph.Labels.LabelsRaw)
            {
                Assert.Contains(label.Name, responseString);
                Assert.Contains(label.WebsiteUrl, responseString);
            }
        }

        [Fact]
        public async Task DetailsGet_ShouldReturnViewWithExistingLabelData()
        {
            // Arrange

            // Act
            var response = await Client.GetAsync("/labels/details/" + TestDataGraph.Labels.ExistingLabel.LabelId);
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Contains("<title>Details - Tuneage.WebApi</title>", responseString);
            Assert.Contains(TestDataGraph.Labels.ExistingLabel.Name, responseString);
            Assert.Contains(TestDataGraph.Labels.ExistingLabel.WebsiteUrl, responseString);
        }

        [Fact]
        public async Task DetailsGet_ShouldReturnNotFoundResultWhenCalledWithBadId()
        {
            // Arrange

            // Act
            var response = await Client.GetAsync("/labels/details/" + TestDataGraph.Labels.NonExistentLabelId);
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(string.Empty, responseString);
        }

        [Fact]
        public async Task CreateGet_ShouldReturnViewForCreatingNewLabel()
        {
            // Arrange

            // Act
            var response = await Client.GetAsync("/labels/create");
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains("<title>Create - Tuneage.WebApi</title>", responseString);
        }
        
        [Fact]
        public async Task CreatePost_ShouldReturnFoundStatusAndRedirectionLocationToAll()
        {
            // Arrange
            var nextId = TestDataGraph.Labels.LabelsRaw.Count + 1;
            var formData = await EnsureAntiforgeryTokenOnForm(new Dictionary<string, string>()
            {
                { "LabelId" , nextId.ToString() },
                { "Name", TestDataGraph.Labels.NewLabel.Name },
                { "WebsiteUrl", TestDataGraph.Labels.NewLabel.WebsiteUrl }
            });

            // Act
            var response = await Client.PostAsync("/labels/create", new FormUrlEncodedContent(formData));
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Found, response.StatusCode);
            Assert.Equal("/labels", response.Headers.Location.ToString());
            Assert.Equal(string.Empty, responseString);

            // Act
            var response2 = await Client.GetAsync("/labels/details/" + nextId);
            var responseString2 = await response2.Content.ReadAsStringAsync();

            // Assert
            response2.EnsureSuccessStatusCode();
            Assert.Contains("<title>Details - Tuneage.WebApi</title>", responseString2);
            Assert.Contains(TestDataGraph.Labels.NewLabel.Name, responseString2);
            Assert.Contains(TestDataGraph.Labels.NewLabel.WebsiteUrl, responseString2);
        }

        [Fact]
        public async Task CreatePost_ShouldReturnErrorWhenCalledWithExistingId()
        {
            // Arrange
            var formData = await EnsureAntiforgeryTokenOnForm(new Dictionary<string, string>()
            {
                { "LabelId" , TestDataGraph.Labels.ExistingLabel.LabelId.ToString() },
                { "Name", TestDataGraph.Labels.ExistingLabel.Name },
                { "WebsiteUrl", TestDataGraph.Labels.ExistingLabel.WebsiteUrl }
            });

            // Act
            var response = await Client.PostAsync("/labels/create", new FormUrlEncodedContent(formData));
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.Contains("ArgumentException: An item with the same key has already been added.", responseString);
        }

        [Fact]
        public async Task EditGet_ShouldReturnViewWithExistingLabelData()
        {
            // Arrange

            // Act
            var response = await Client.GetAsync("/labels/edit/" + TestDataGraph.Labels.ExistingLabel.LabelId);
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Contains("<title>Edit - Tuneage.WebApi</title>", responseString);
            Assert.Contains(TestDataGraph.Labels.ExistingLabel.Name, responseString);
            Assert.Contains(TestDataGraph.Labels.ExistingLabel.WebsiteUrl, responseString);
        }

        [Fact]
        public async Task EditGet_ShouldReturnNotFoundResultWhenCalledWithBadId()
        {
            // Arrange

            // Act
            var response = await Client.GetAsync("/labels/edit/" + TestDataGraph.Labels.NonExistentLabelId);
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(string.Empty, responseString);
        }

        [Fact]
        public async Task EditPost_ShouldReturnFoundStatusAndRedirectionLocationToAll()
        {
            // Arrange
            var formData = await EnsureAntiforgeryTokenOnForm(new Dictionary<string, string>()
            {
                { "LabelId", TestDataGraph.Labels.UpdatedLabel.LabelId.ToString() },
                { "Name", TestDataGraph.Labels.UpdatedLabel.Name },
                { "WebsiteUrl", TestDataGraph.Labels.UpdatedLabel.WebsiteUrl }
            });

            // Act
            var response = await Client.PostAsync("/labels/edit/" + formData["LabelId"], new FormUrlEncodedContent(formData));
            var responseString = await response.Content.ReadAsStringAsync();
            
            // Assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Found, response.StatusCode);
            Assert.Equal("/labels", response.Headers.Location.ToString());
            Assert.Equal(string.Empty, responseString);
        }

        [Fact]
        public async Task EditPost_ShouldReturnErrorWhenCalledWithBadId()
        {
            // Arrange
            var formData = await EnsureAntiforgeryTokenOnForm(new Dictionary<string, string>()
            {
                { "LabelId", TestDataGraph.Labels.NonExistentLabelId.ToString() },
                { "Name", TestDataGraph.Labels.UpdatedLabel.Name },
                { "WebsiteUrl", TestDataGraph.Labels.UpdatedLabel.WebsiteUrl }
            });

            // Act
            var response = await Client.PostAsync("/labels/edit/" + formData["LabelId"], new FormUrlEncodedContent(formData));
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.Contains("DbUpdateConcurrencyException: Attempted to update or delete an entity that does not exist in the store.", responseString);
        }

        [Fact]
        public async Task EditPost_ShouldReturnNotFoundResultWhenCalledWithNonMatchingIdData()
        {
            // Arrange
            var formData = await EnsureAntiforgeryTokenOnForm(new Dictionary<string, string>()
            {
                { "LabelId", TestDataGraph.Labels.NonExistentLabelId.ToString() },
                { "Name", TestDataGraph.Labels.UpdatedLabel.Name },
                { "WebsiteUrl", TestDataGraph.Labels.UpdatedLabel.WebsiteUrl }
            });

            // Act
            var response = await Client.PostAsync("/labels/edit/" + TestDataGraph.Labels.UpdatedLabel.LabelId, new FormUrlEncodedContent(formData));
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(string.Empty, responseString);
        }

        [Fact]
        public async Task DeleteGet_ShouldReturnViewWithExistingLabelData()
        {
            // Arrange

            // Act
            var response = await Client.GetAsync("/labels/delete/" + TestDataGraph.Labels.ExistingLabel.LabelId);
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Contains("<title>Delete - Tuneage.WebApi</title>", responseString);
            Assert.Contains(TestDataGraph.Labels.ExistingLabel.Name, responseString);
            Assert.Contains(TestDataGraph.Labels.ExistingLabel.WebsiteUrl, responseString);
        }

        [Fact]
        public async Task DeletePost_ShouldReturnViewWithExistingLabelDataRemoved()
        {
            // Arrange
            var formData = await EnsureAntiforgeryTokenOnForm(new Dictionary<string, string>()
            {
                { "LabelId", TestDataGraph.Labels.ExistingLabel.LabelId.ToString() },
                { "Name", TestDataGraph.Labels.ExistingLabel.Name },
                { "WebsiteUrl", TestDataGraph.Labels.ExistingLabel.WebsiteUrl }
            });

            // Act
            var response = await Client.PostAsync("/labels/delete/" + formData["LabelId"], new FormUrlEncodedContent(formData));
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Found, response.StatusCode);
            Assert.Equal("/labels", response.Headers.Location.ToString());
            Assert.Equal(string.Empty, responseString);
        }

        [Fact]
        public async Task DeletePost_ShouldReturnErrorWhenCalledWithBadId()
        {
            // Arrange
            var formData = await EnsureAntiforgeryTokenOnForm(new Dictionary<string, string>()
            {
                { "LabelId", TestDataGraph.Labels.NonExistentLabelId.ToString() },
                { "Name", TestDataGraph.Labels.ExistingLabel.Name },
                { "WebsiteUrl", TestDataGraph.Labels.ExistingLabel.WebsiteUrl }
            });

            // Act
            var response = await Client.PostAsync("/labels/delete/" + formData["LabelId"], new FormUrlEncodedContent(formData));
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.Contains("ArgumentNullException: Value cannot be null.", responseString);
        }
    }
}
