using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Tuneage.Data.TestData;
using Tuneage.Domain.Entities;
using Xunit;

namespace Tuneage.WebApi.Tests.Integration.Api
{
    public class LabelsIntegrationTests : IntegrationTestFixture
    {
        [Fact]
        public async Task GetLabels_ShouldReturnAllLabelsInAlphabeticalOrder()
        {
            // Arrange
            await EnsureAntiForgeryTokenHeader();

            // Act
            var response = await Client.GetAsync("/api/labels/");
            var responseString = await response.Content.ReadAsStringAsync();
            var returnedLabels = JsonConvert.DeserializeObject<List<Label>>(responseString);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(JsonConvert.SerializeObject(TestDataGraph.Labels.LabelsAlphabetizedByLabelName), JsonConvert.SerializeObject(returnedLabels));
        }

        [Fact]
        public async Task GetLabel_ShouldReturnExistingLabel()
        {
            // Arrange
            await EnsureAntiForgeryTokenHeader();
            var existingLabel = TestDataGraph.Labels.LabelExisting;

            // Act
            var response = await Client.GetAsync("/api/labels/" + existingLabel.LabelId);
            var responseString = await response.Content.ReadAsStringAsync();
            var returnedLabel = JsonConvert.DeserializeObject<Label>(responseString);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(JsonConvert.SerializeObject(existingLabel), JsonConvert.SerializeObject(returnedLabel));
        }

        [Fact]
        public async Task GetLabel_ShouldReturnNotFoundResultWhenCalledWithBadId()
        {
            // Arrange
            await EnsureAntiForgeryTokenHeader();

            // Act
            var response = await Client.GetAsync("/api/labels/" + TestDataGraph.Labels.LabelIdNonExistent);
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(string.Empty, responseString);
        }

        [Fact]
        public async Task PutLabel_ShouldReturnNoContentResult()
        {
            // Arrange
            await EnsureAntiForgeryTokenHeader();
            var updatedLabel = TestDataGraph.Labels.LabelUpdated;
            var contents = new StringContent(JsonConvert.SerializeObject(updatedLabel), Encoding.UTF8, "application/json");

            // Act
            var response = await Client.PutAsync("/api/labels/" + updatedLabel.LabelId, contents);
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Equal(string.Empty, responseString);
        }

        [Fact]
        public async Task PutLabel_ShouldReturnBadRequestResultWhenCalledWithNonMatchingIdData()
        {
            // Arrange
            await EnsureAntiForgeryTokenHeader();
            var updatedLabel = TestDataGraph.Labels.LabelUpdated;
            var contents = new StringContent(JsonConvert.SerializeObject(updatedLabel), Encoding.UTF8, "application/json");

            // Act
            var response = await Client.PutAsync("/api/labels/" + TestDataGraph.Labels.LabelIdNonExistent, contents);
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(string.Empty, responseString);
        }

        [Fact]
        public async Task PostLabel_ShouldReturnAddedLabel()
        {
            // Arrange
            await EnsureAntiForgeryTokenHeader();
            var newLabel = TestDataGraph.Labels.LabelNew;
            var contents = new StringContent(JsonConvert.SerializeObject(newLabel), Encoding.UTF8, "application/json");

            // Act
            var response = await Client.PostAsync("api/labels", contents);
            var responseString = await response.Content.ReadAsStringAsync();
            var addedLabel = JsonConvert.DeserializeObject<Label>(responseString);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(JsonConvert.SerializeObject(addedLabel), JsonConvert.SerializeObject(newLabel));
        }

        [Fact]
        public async Task DeleteLabel_ShouldReturnDeletedLabel()
        {
            // Arrange
            await EnsureAntiForgeryTokenHeader();
            var existingLabel = TestDataGraph.Labels.LabelExisting;

            // Act
            var response = await Client.DeleteAsync("api/labels/" + existingLabel.LabelId);
            var responseString = await response.Content.ReadAsStringAsync();
            var deletedLabel = JsonConvert.DeserializeObject<Label>(responseString);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(JsonConvert.SerializeObject(deletedLabel), JsonConvert.SerializeObject(existingLabel));
        }

        [Fact]
        public async Task DeleteLabel_ShouldReturnNotFoundResultWhenCalledWithBadId()
        {
            // Arrange
            await EnsureAntiForgeryTokenHeader();

            // Act
            var response = await Client.DeleteAsync("api/labels/" + TestDataGraph.Labels.LabelIdNonExistent);
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(string.Empty, responseString);
        }
    }
}
