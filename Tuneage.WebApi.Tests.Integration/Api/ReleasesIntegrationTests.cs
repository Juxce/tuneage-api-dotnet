using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Tuneage.Data.Constants;
using Tuneage.Data.TestData;
using Tuneage.Domain.Entities;
using Xunit;

namespace Tuneage.WebApi.Tests.Integration.Api
{
    public class ReleasesIntegrationTests : IntegrationTestFixture
    {
        [Fact]
        public async Task GetReleases_ShouldReturnAllReleasesInAlphabeticalOrder()
        {
            // Arrange
            await EnsureAntiforgeryTokenHeader();

            // Act
            var response = await Client.GetAsync("/api/releases/");
            var responseString = await response.Content.ReadAsStringAsync();
            var returnedReleases = JsonConvert.DeserializeObject<List<Release>>(responseString);

            // Assert
            for (int i = 0; i < returnedReleases.Count; i++)
            {
                Assert.Equal(TestDataGraph.Releases.ReleasesAlphabetizedByTitle[i].Title, returnedReleases[i].Title);
            }
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetRelease_ShouldReturnExistingRelease()
        {
            // Arrange
            await EnsureAntiforgeryTokenHeader();
            var existingRelease = TestDataGraph.Releases.ExistingRelease;

            // Act
            var response = await Client.GetAsync("/api/releases/" + existingRelease.ReleaseId);
            var responseString = await response.Content.ReadAsStringAsync();
            var returnedRelease = JsonConvert.DeserializeObject<Release>(responseString);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(JsonConvert.SerializeObject(existingRelease), JsonConvert.SerializeObject(returnedRelease));
        }

        [Fact]
        public async Task GetRelease_ShouldReturnNotFoundResultWhenCalledWithBadId()
        {
            // Arrange
            await EnsureAntiforgeryTokenHeader();

            // Act
            var response = await Client.GetAsync("/api/releases/" + TestDataGraph.Releases.NonExistentReleaseId);
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(string.Empty, responseString);
        }

        [Fact(Skip = Explainations.DbConcurrencyExceptionFromInMemoryDb)]
        public async Task PutRelease_WhenReleaseWasOriginallySingleArtist_ShouldReturnNoContentResult()
        {
            // Arrange
            await EnsureAntiforgeryTokenHeader();
            var updatedSingleArtistRelease = TestDataGraph.Releases.UpdatedSingleArtistRelease;
            var contents = new StringContent(JsonConvert.SerializeObject(updatedSingleArtistRelease), Encoding.UTF8, "application/json");

            // Act
            var response = await Client.PutAsync("/api/releases/" + updatedSingleArtistRelease.ReleaseId, contents);
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Equal(string.Empty, responseString);
        }
        #region "Commented tests with same approach for additional subtypes"

        //[Fact]
        //public async Task PutRelease_WhenReleaseWasOriginallyVariousArtists_ShouldReturnNoContentResult()
        //{
        //    // Arrange
        //    await EnsureAntiforgeryTokenHeader();
        //    var updatedVariousArtistsRelease = TestDataGraph.Releases.UpdatedVariousArtistsRelease;
        //    var contents = new StringContent(JsonConvert.SerializeObject(updatedVariousArtistsRelease), Encoding.UTF8, "application/json");

        //    // Act
        //    var response = await Client.PutAsync("/api/releases/" + updatedVariousArtistsRelease.ReleaseId, contents);
        //    var responseString = await response.Content.ReadAsStringAsync();

        //    // Assert
        //    response.EnsureSuccessStatusCode();
        //    Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        //    Assert.Equal(string.Empty, responseString);
        //}

        #endregion "Commented tests with same approach for additional subtypes"

        [Fact]
        public async Task PutRelease_ShouldReturnBadRequestResultWhenCalledWithNonMatchingIdData()
        {
            // Arrange
            await EnsureAntiforgeryTokenHeader();
            var updatedRelease = TestDataGraph.Releases.UpdatedSingleArtistRelease;
            var contents = new StringContent(JsonConvert.SerializeObject(updatedRelease), Encoding.UTF8, "application/json");

            // Act
            var response = await Client.PutAsync("/api/releases/" + TestDataGraph.Releases.NonExistentReleaseId, contents);
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(string.Empty, responseString);
        }

        [Fact]
        public async Task PostRelease_WhenSingleArtist_ShouldReturnAddedRelease()
        {
            // Arrange
            await EnsureAntiforgeryTokenHeader();
            var newSingleArtistRelease = TestDataGraph.Releases.NewSingleArtistRelease;
            newSingleArtistRelease.ReleaseId = TestDataGraph.Releases.ReleasesRaw.Count + 1;
            var contents = new StringContent(JsonConvert.SerializeObject(newSingleArtistRelease), Encoding.UTF8, "application/json");

            // Act
            var response = await Client.PostAsync("api/releases", contents);
            var responseString = await response.Content.ReadAsStringAsync();
            var addedRelease = JsonConvert.DeserializeObject<Release>(responseString);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(JsonConvert.SerializeObject(addedRelease), JsonConvert.SerializeObject(newSingleArtistRelease));

            // Act
            var response2 = await Client.GetAsync("/api/releases/" + newSingleArtistRelease.ReleaseId);
            var responseString2 = await response2.Content.ReadAsStringAsync();
            var returnedRelease = JsonConvert.DeserializeObject<Release>(responseString2);

            // Assert
            response2.EnsureSuccessStatusCode();
            Assert.Equal(newSingleArtistRelease.Title, returnedRelease.Title);
        }

        [Fact]
        public async Task PostRelease_WhenVariousArtists_ShouldReturnAddedRelease()
        {
            // Arrange
            await EnsureAntiforgeryTokenHeader();
            var newVariousArtistsRelease = TestDataGraph.Releases.NewVariousArtistsRelease;
            newVariousArtistsRelease.ReleaseId = TestDataGraph.Releases.ReleasesRaw.Count + 1;
            var contents = new StringContent(JsonConvert.SerializeObject(newVariousArtistsRelease), Encoding.UTF8, "application/json");

            // Act
            var response = await Client.PostAsync("api/releases", contents);
            var responseString = await response.Content.ReadAsStringAsync();
            var addedRelease = JsonConvert.DeserializeObject<Release>(responseString);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(JsonConvert.SerializeObject(addedRelease), JsonConvert.SerializeObject(newVariousArtistsRelease));

            // Act
            var response2 = await Client.GetAsync("/api/releases/" + newVariousArtistsRelease.ReleaseId);
            var responseString2 = await response2.Content.ReadAsStringAsync();
            var returnedRelease = JsonConvert.DeserializeObject<Release>(responseString2);

            // Assert
            response2.EnsureSuccessStatusCode();
            Assert.Equal(newVariousArtistsRelease.Title, returnedRelease.Title);

        }

        [Fact]
        public async Task DeleteRelease_ShouldReturnDeletedRelease()
        {
            // Arrange
            await EnsureAntiforgeryTokenHeader();
            var existingRelease = TestDataGraph.Releases.ExistingRelease;

            // Act
            var response = await Client.DeleteAsync("api/releases/" + existingRelease.ReleaseId);
            var responseString = await response.Content.ReadAsStringAsync();
            var deletedRelease = JsonConvert.DeserializeObject<Release>(responseString);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(JsonConvert.SerializeObject(deletedRelease), JsonConvert.SerializeObject(existingRelease));
        }

        [Fact]
        public async Task DeleteRelease_ShouldReturnNotFoundResultWhenCalledWithBadId()
        {
            // Arrange
            await EnsureAntiforgeryTokenHeader();

            // Act
            var response = await Client.DeleteAsync("api/releases/" + TestDataGraph.Releases.NonExistentReleaseId);
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(string.Empty, responseString);
        }
    }
}
