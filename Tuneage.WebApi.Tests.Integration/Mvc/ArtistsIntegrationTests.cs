using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Tuneage.Data.Constants;
using Tuneage.Data.TestData;
using Xunit;

namespace Tuneage.WebApi.Tests.Integration.Mvc
{
    public class ArtistsIntegrationTests : IntegrationTestFixture
    {
        [Fact]
        public async Task AllGet_ShouldReturnViewWithArtistsData()
        {
            // Arrange

            // Act
            var response = await Client.GetAsync("/artists");
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Contains("<title>Index - Tuneage.WebApi</title>", responseString);
            foreach (var artist in TestDataGraph.Artists.ArtistsRaw)
            {
                Assert.Contains(artist.Name.Replace("&", "&amp;"), responseString);
            }
        }

        [Fact]
        public async Task DetailsGet_ShouldReturnViewWithExistingArtistData()
        {
            // Arrange

            // Act
            var response = await Client.GetAsync("/artists/details/" + TestDataGraph.Artists.ExistingArtist.ArtistId);
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Contains("<title>Details - Tuneage.WebApi</title>", responseString);
            Assert.Contains(TestDataGraph.Artists.ExistingArtist.Name, responseString);
        }

        [Fact]
        public async Task DetailsGet_ShouldReturnNotFoundResultWhenCalledWithBadId()
        {
            // Arrange

            // Act
            var response = await Client.GetAsync("/artists/details/" + TestDataGraph.Artists.NonExistentArtistId);
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(string.Empty, responseString);
        }

        [Fact]
        public async Task CreateGet_ShouldReturnViewForCreatingNewArtist()
        {
            // Arrange

            // Act
            var response = await Client.GetAsync("/artists/create");
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains("<title>Create - Tuneage.WebApi</title>", responseString);
        }
        
        [Fact]
        public async Task CreatePost_WhenSoloArtist_ShouldReturnFoundStatusAndRedirectionLocationToAll()
        {
            // Arrange
            var nextId = TestDataGraph.Artists.ArtistsRaw.Count + 1;
            var formData = await EnsureAntiforgeryTokenOnForm(new Dictionary<string, string>()
            {
                { "ArtistId" , nextId.ToString() },
                { "Name", TestDataGraph.Artists.NewSoloArtist.Name },
                { "IsBand", TestDataGraph.Artists.NewSoloArtist.IsBand.ToString() },
                { "IsPrinciple", TestDataGraph.Artists.NewSoloArtist.IsPrinciple.ToString() }
            });

            // Act
            var response = await Client.PostAsync("/artists/create", new FormUrlEncodedContent(formData));
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Found, response.StatusCode);
            Assert.Equal("/artists", response.Headers.Location.ToString());
            Assert.Equal(string.Empty, responseString);

            // Act
            var response2 = await Client.GetAsync("/artists/details/" + nextId);
            var responseString2 = await response2.Content.ReadAsStringAsync();

            // Assert
            response2.EnsureSuccessStatusCode();
            Assert.Contains("<title>Details - Tuneage.WebApi</title>", responseString2);
            Assert.Contains(TestDataGraph.Artists.NewSoloArtist.Name, responseString2);
        }

        [Fact]
        public async Task CreatePost_WhenBand_ShouldReturnFoundStatusAndRedirectionLocationToAll()
        {
            // Arrange
            var nextId = TestDataGraph.Artists.ArtistsRaw.Count + 1;
            var formData = await EnsureAntiforgeryTokenOnForm(new Dictionary<string, string>()
            {
                { "ArtistId" , nextId.ToString() },
                { "Name", TestDataGraph.Artists.NewBand.Name },
                { "IsBand", TestDataGraph.Artists.NewBand.IsBand.ToString() },
                { "IsPrinciple", TestDataGraph.Artists.NewBand.IsPrinciple.ToString() }
            });

            // Act
            var response = await Client.PostAsync("/artists/create", new FormUrlEncodedContent(formData));
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Found, response.StatusCode);
            Assert.Equal("/artists", response.Headers.Location.ToString());
            Assert.Equal(string.Empty, responseString);

            // Act
            var response2 = await Client.GetAsync("/artists/details/" + nextId);
            var responseString2 = await response2.Content.ReadAsStringAsync();

            // Assert
            response2.EnsureSuccessStatusCode();
            Assert.Contains("<title>Details - Tuneage.WebApi</title>", responseString2);
            Assert.Contains(TestDataGraph.Artists.NewBand.Name, responseString2);
        }

        [Fact]
        public async Task CreatePost_ShouldReturnErrorWhenCalledWithExistingId()
        {
            // Arrange
            var formData = await EnsureAntiforgeryTokenOnForm(new Dictionary<string, string>()
            {
                { "ArtistId" , TestDataGraph.Artists.ExistingArtist.ArtistId.ToString() },
                { "Name", TestDataGraph.Artists.ExistingArtist.Name },
                { "IsBand", "true" },
                { "IsPrinciple", "true" }
            });

            // Act
            var response = await Client.PostAsync("/artists/create", new FormUrlEncodedContent(formData));
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.Contains("ArgumentException: An item with the same key has already been added.", responseString);
        }

        [Fact]
        public async Task EditGet_ShouldReturnViewWithExistingArtistData()
        {
            // Arrange

            // Act
            var response = await Client.GetAsync("/artists/edit/" + TestDataGraph.Artists.ExistingArtist.ArtistId);
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Contains("<title>Edit - Tuneage.WebApi</title>", responseString);
            Assert.Contains(TestDataGraph.Artists.ExistingArtist.Name, responseString);
        }

        [Fact]
        public async Task EditGet_ShouldReturnNotFoundResultWhenCalledWithBadId()
        {
            // Arrange

            // Act
            var response = await Client.GetAsync("/artists/edit/" + TestDataGraph.Artists.NonExistentArtistId);
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(string.Empty, responseString);
        }

        [Fact]
        public async Task EditPost_WhenArtistWasOriginallySolo_ShouldReturnFoundStatusAndRedirectionLocationToAll()
        {
            // Arrange
            var formData = await EnsureAntiforgeryTokenOnForm(new Dictionary<string, string>()
            {
                { "ArtistId", TestDataGraph.Artists.UpdatedSoloArtist.ArtistId.ToString() },
                { "Name", TestDataGraph.Artists.UpdatedSoloArtist.Name },
                { "IsBand", TestDataGraph.Artists.UpdatedSoloArtist.IsBand.ToString() },
                { "IsPrinciple", TestDataGraph.Artists.UpdatedSoloArtist.IsPrinciple.ToString() }
            });

            // Act
            var response = await Client.PostAsync("/artists/edit/" + formData["ArtistId"], new FormUrlEncodedContent(formData));
            var responseString = await response.Content.ReadAsStringAsync();
            
            // Assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Found, response.StatusCode);
            Assert.Equal("/artists", response.Headers.Location.ToString());
            Assert.Equal(string.Empty, responseString);
        }

        [Fact]
        public async Task EditPost_WhenArtistWasOriginallyBand_ShouldReturnFoundStatusAndRedirectionLocationToAll()
        {
            // Arrange
            var formData = await EnsureAntiforgeryTokenOnForm(new Dictionary<string, string>()
            {
                { "ArtistId", TestDataGraph.Artists.UpdatedBand.ArtistId.ToString() },
                { "Name", TestDataGraph.Artists.UpdatedBand.Name },
                { "IsBand", TestDataGraph.Artists.UpdatedBand.IsBand.ToString() },
                { "IsPrinciple", TestDataGraph.Artists.UpdatedBand.IsPrinciple.ToString() }
            });

            // Act
            var response = await Client.PostAsync("/artists/edit/" + formData["ArtistId"], new FormUrlEncodedContent(formData));
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Found, response.StatusCode);
            Assert.Equal("/artists", response.Headers.Location.ToString());
            Assert.Equal(string.Empty, responseString);
        }

        [Fact]
        public async Task EditPost_WhenArtistWasOriginallyAlias_ShouldReturnFoundStatusAndRedirectionLocationToAll()
        {
            // Arrange
            var formData = await EnsureAntiforgeryTokenOnForm(new Dictionary<string, string>()
            {
                { "ArtistId", TestDataGraph.Artists.UpdatedAlias.ArtistId.ToString() },
                { "Name", TestDataGraph.Artists.UpdatedAlias.Name },
                { "IsBand", TestDataGraph.Artists.UpdatedAlias.IsBand.ToString() },
                { "IsPrinciple", TestDataGraph.Artists.UpdatedAlias.IsPrinciple.ToString() },
                { "PrincipleArtistId", TestDataGraph.Artists.UpdatedAlias.PrincipleArtistId.ToString() }
            });

            // Act
            var response = await Client.PostAsync("/artists/edit/" + formData["ArtistId"], new FormUrlEncodedContent(formData));
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Found, response.StatusCode);
            Assert.Equal("/artists", response.Headers.Location.ToString());
            Assert.Equal(string.Empty, responseString);
        }

        [Fact]
        public async Task EditPost_ShouldReturnErrorWhenCalledWithBadId()
        {
            // Arrange
            var formData = await EnsureAntiforgeryTokenOnForm(new Dictionary<string, string>()
            {
                { "ArtistId", TestDataGraph.Artists.NonExistentArtistId.ToString() },
                { "Name", TestDataGraph.Artists.UpdatedBand.Name },
                { "IsBand", TestDataGraph.Artists.UpdatedBand.IsBand.ToString() },
                { "IsPrinciple", TestDataGraph.Artists.UpdatedBand.IsPrinciple.ToString() }
            });

            // Act
            var response = await Client.PostAsync("/artists/edit/" + formData["ArtistId"], new FormUrlEncodedContent(formData));
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.Contains(ErrorMessages.ArtistIdForUpdateDoesNotExist, responseString);
        }

        [Fact]
        public async Task EditPost_ShouldReturnNotFoundResultWhenCalledWithNonMatchingIdData()
        {
            // Arrange
            var formData = await EnsureAntiforgeryTokenOnForm(new Dictionary<string, string>()
            {
                { "ArtistId", TestDataGraph.Artists.NonExistentArtistId.ToString() },
                { "Name", TestDataGraph.Artists.UpdatedBand.Name },
                { "IsBand", TestDataGraph.Artists.UpdatedBand.IsBand.ToString() },
                { "IsPrinciple", TestDataGraph.Artists.UpdatedBand.IsPrinciple.ToString() }
            });

            // Act
            var response = await Client.PostAsync("/artists/edit/" + TestDataGraph.Artists.UpdatedBand.ArtistId, new FormUrlEncodedContent(formData));
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(string.Empty, responseString);
        }

        [Fact]
        public async Task DeleteGet_ShouldReturnViewWithExistingArtistData()
        {
            // Arrange

            // Act
            var response = await Client.GetAsync("/artists/delete/" + TestDataGraph.Artists.ExistingArtist.ArtistId);
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Contains("<title>Delete - Tuneage.WebApi</title>", responseString);
            Assert.Contains(TestDataGraph.Artists.ExistingArtist.Name, responseString);
        }

        [Fact]
        public async Task DeletePost_ShouldReturnViewWithExistingArtistDataRemoved()
        {
            // Arrange
            var formData = await EnsureAntiforgeryTokenOnForm(new Dictionary<string, string>()
            {
                { "ArtistId", TestDataGraph.Artists.ExistingArtist.ArtistId.ToString() },
                { "Name", TestDataGraph.Artists.ExistingArtist.Name }
            });

            // Act
            var response = await Client.PostAsync("/artists/delete/" + formData["ArtistId"], new FormUrlEncodedContent(formData));
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Found, response.StatusCode);
            Assert.Equal("/artists", response.Headers.Location.ToString());
            Assert.Equal(string.Empty, responseString);
        }

        [Fact]
        public async Task DeletePost_ShouldReturnErrorWhenCalledWithBadId()
        {
            // Arrange
            var formData = await EnsureAntiforgeryTokenOnForm(new Dictionary<string, string>()
            {
                { "ArtistId", TestDataGraph.Artists.NonExistentArtistId.ToString() },
                { "Name", TestDataGraph.Artists.ExistingArtist.Name }
            });

            // Act
            var response = await Client.PostAsync("/artists/delete/" + formData["ArtistId"], new FormUrlEncodedContent(formData));
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.Contains("ArgumentNullException: Value cannot be null.", responseString);
        }
    }
}
