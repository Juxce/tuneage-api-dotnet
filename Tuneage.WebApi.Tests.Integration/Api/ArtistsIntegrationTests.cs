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
    public class ArtistsIntegrationTests : IntegrationTestFixture
    {
        [Fact]
        public async Task GetArtists_ShouldReturnAllArtistsInAlphabeticalOrder()
        {
            // Arrange
            var rawSerializedAlphaArtists = JsonConvert.SerializeObject(TestDataGraph.Artists.ArtistsAlphabetizedByArtistName);
            var expectedSerializedArtists = rawSerializedAlphaArtists.Replace("\"PrincipalArtist\":null,", string.Empty);
            await EnsureAntiforgeryTokenHeader();

            // Act
            var response = await Client.GetAsync("/api/artists/");
            var responseString = await response.Content.ReadAsStringAsync();
            var returnedArtists = JsonConvert.DeserializeObject<List<Artist>>(responseString);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(expectedSerializedArtists, JsonConvert.SerializeObject(returnedArtists));
        }

        [Fact]
        public async Task GetArtist_ShouldReturnExistingArtist()
        {
            // Arrange
            await EnsureAntiforgeryTokenHeader();
            var existingArtist = TestDataGraph.Artists.ExistingArtist;

            // Act
            var response = await Client.GetAsync("/api/artists/" + existingArtist.ArtistId);
            var responseString = await response.Content.ReadAsStringAsync();
            var returnedArtist = JsonConvert.DeserializeObject<Artist>(responseString);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(JsonConvert.SerializeObject(existingArtist), JsonConvert.SerializeObject(returnedArtist));
        }

        [Fact]
        public async Task GetArtist_ShouldReturnNotFoundResultWhenCalledWithBadId()
        {
            // Arrange
            await EnsureAntiforgeryTokenHeader();

            // Act
            var response = await Client.GetAsync("/api/artists/" + TestDataGraph.Artists.NonExistentArtistId);
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(string.Empty, responseString);
        }

        // TODO: Come back to this test to drive out additional Put methods for controller
        [Fact]
        public async Task PutArtist_WhenArtistWasOriginallySolo_ShouldReturnNoContentResult()
        {
            // Arrange
            await EnsureAntiforgeryTokenHeader();
            var updatedSoloArtist = TestDataGraph.Artists.UpdatedSoloArtist;
            var contents = new StringContent(JsonConvert.SerializeObject(updatedSoloArtist), Encoding.UTF8, "application/json");

            // Act
            var response = await Client.PutAsync("/api/artists/" + updatedSoloArtist.ArtistId, contents);
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Equal(string.Empty, responseString);
        }

        [Fact]
        public async Task PutArtist_WhenArtistWasOriginallyBand_ShouldReturnNoContentResult()
        {
            // Arrange
            await EnsureAntiforgeryTokenHeader();
            var updatedBand = TestDataGraph.Artists.UpdatedBand;
            var contents = new StringContent(JsonConvert.SerializeObject(updatedBand), Encoding.UTF8, "application/json");

            // Act
            var response = await Client.PutAsync("/api/artists/" + updatedBand.ArtistId, contents);
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Equal(string.Empty, responseString);
        }

        [Fact]
        public async Task PutArtist_WhenArtistWasOriginallyAlias_ShouldReturnNoContentResult()
        {
            // Arrange
            await EnsureAntiforgeryTokenHeader();
            var updatedAlias = TestDataGraph.Artists.UpdatedAlias;
            var contents = new StringContent(JsonConvert.SerializeObject(updatedAlias), Encoding.UTF8, "application/json");

            // Act
            var response = await Client.PutAsync("/api/artists/" + updatedAlias.ArtistId, contents);
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Equal(string.Empty, responseString);
        }

        [Fact]
        public async Task PutArtist_ShouldReturnBadRequestResultWhenCalledWithNonMatchingIdData()
        {
            // Arrange
            await EnsureAntiforgeryTokenHeader();
            var updatedArtist = TestDataGraph.Artists.UpdatedSoloArtist;
            var contents = new StringContent(JsonConvert.SerializeObject(updatedArtist), Encoding.UTF8, "application/json");

            // Act
            var response = await Client.PutAsync("/api/artists/" + TestDataGraph.Artists.NonExistentArtistId, contents);
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(string.Empty, responseString);
        }

        [Fact]
        public async Task PostArtist_WhenSolo_ShouldReturnAddedArtist()
        {
            // Arrange
            await EnsureAntiforgeryTokenHeader();
            var newSoloArtist = TestDataGraph.Artists.NewSoloArtist;
            newSoloArtist.ArtistId = TestDataGraph.Artists.ArtistsRaw.Count + 1;
            var contents = new StringContent(JsonConvert.SerializeObject(newSoloArtist), Encoding.UTF8, "application/json");

            // Act
            var response = await Client.PostAsync("api/artists", contents);
            var responseString = await response.Content.ReadAsStringAsync();
            var addedArtist = JsonConvert.DeserializeObject<Artist>(responseString);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(JsonConvert.SerializeObject(addedArtist), JsonConvert.SerializeObject(newSoloArtist));

            // Act
            var response2 = await Client.GetAsync("/api/artists/" + newSoloArtist.ArtistId);
            var responseString2 = await response2.Content.ReadAsStringAsync();
            var returnedArtist = JsonConvert.DeserializeObject<Artist>(responseString2);

            // Assert
            response2.EnsureSuccessStatusCode();
            Assert.Equal(JsonConvert.SerializeObject(newSoloArtist), JsonConvert.SerializeObject(returnedArtist));
        }

        [Fact]
        public async Task PostArtist_WhenBand_ShouldReturnAddedArtist()
        {
            // Arrange
            await EnsureAntiforgeryTokenHeader();
            var newBand = TestDataGraph.Artists.NewBand;
            newBand.ArtistId = TestDataGraph.Artists.ArtistsRaw.Count + 1;
            var contents = new StringContent(JsonConvert.SerializeObject(newBand), Encoding.UTF8, "application/json");

            // Act
            var response = await Client.PostAsync("api/artists", contents);
            var responseString = await response.Content.ReadAsStringAsync();
            var addedArtist = JsonConvert.DeserializeObject<Artist>(responseString);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(JsonConvert.SerializeObject(addedArtist), JsonConvert.SerializeObject(newBand));

            // Act
            var response2 = await Client.GetAsync("/api/artists/" + newBand.ArtistId);
            var responseString2 = await response2.Content.ReadAsStringAsync();
            var returnedArtist = JsonConvert.DeserializeObject<Artist>(responseString2);

            // Assert
            response2.EnsureSuccessStatusCode();
            Assert.Equal(JsonConvert.SerializeObject(newBand), JsonConvert.SerializeObject(returnedArtist));
        }

        [Fact]
        public async Task PostArtist_WhenAlias_ShouldReturnAddedArtist()
        {
            // Arrange
            await EnsureAntiforgeryTokenHeader();
            var newAlias = TestDataGraph.Artists.NewAliasedArtist;
            newAlias.ArtistId = TestDataGraph.Artists.ArtistsRaw.Count + 1;
            var contents = new StringContent(JsonConvert.SerializeObject(newAlias), Encoding.UTF8, "application/json");
            var newAliasSerializedAndCleaned = JsonConvert.SerializeObject(newAlias)
                .Replace("\"PrincipalArtist\":null,", string.Empty);

            // Act
            var response = await Client.PostAsync("api/artists", contents);
            var responseString = await response.Content.ReadAsStringAsync();
            var addedArtist = JsonConvert.DeserializeObject<Artist>(responseString);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(JsonConvert.SerializeObject(addedArtist), newAliasSerializedAndCleaned);

            // Act
            var response2 = await Client.GetAsync("/api/artists/" + newAlias.ArtistId);
            var responseString2 = await response2.Content.ReadAsStringAsync();
            var returnedArtist = JsonConvert.DeserializeObject<Artist>(responseString2);

            // Assert
            response2.EnsureSuccessStatusCode();
            Assert.Equal(newAliasSerializedAndCleaned, JsonConvert.SerializeObject(returnedArtist));
        }

        [Fact]
        public async Task DeleteArtist_ShouldReturnDeletedArtist()
        {
            // Arrange
            await EnsureAntiforgeryTokenHeader();
            var existingArtist = TestDataGraph.Artists.ExistingArtist;

            // Act
            var response = await Client.DeleteAsync("api/artists/" + existingArtist.ArtistId);
            var responseString = await response.Content.ReadAsStringAsync();
            var deletedArtist = JsonConvert.DeserializeObject<Artist>(responseString);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(JsonConvert.SerializeObject(deletedArtist), JsonConvert.SerializeObject(existingArtist));
        }

        [Fact]
        public async Task DeleteArtist_ShouldReturnNotFoundResultWhenCalledWithBadId()
        {
            // Arrange
            await EnsureAntiforgeryTokenHeader();

            // Act
            var response = await Client.DeleteAsync("api/artists/" + TestDataGraph.Artists.NonExistentArtistId);
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(string.Empty, responseString);
        }
    }
}
