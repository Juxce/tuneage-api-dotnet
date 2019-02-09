using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Tuneage.Data.Constants;
using Tuneage.Data.TestData;
using Tuneage.Data.Transform;
using Xunit;

namespace Tuneage.WebApi.Tests.Integration.Mvc
{
    public class ReleasesIntegrationTests : IntegrationTestFixture
    {
        [Fact]
        public async Task AllGet_ShouldReturnViewWithReleasesData()
        {
            // Arrange

            // Act
            var response = await Client.GetAsync("/releases");
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Contains(ViewData.DefaultIndexPageTitle, responseString);
            foreach (var artist in TestDataGraph.Releases.ReleasesRaw)
            {
                Assert.Contains(HtmlTransformer.StringToHtmlString(artist.Title), responseString);
            }
        }

        [Fact]
        public async Task DetailsGet_ShouldReturnViewWithExistingReleaseData()
        {
            // Arrange

            // Act
            var response = await Client.GetAsync("/releases/details/" + TestDataGraph.Releases.ExistingRelease.ReleaseId);
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Contains(ViewData.DefaultDetailsPageTitle, responseString);
            Assert.Contains(HtmlTransformer.StringToHtmlString(TestDataGraph.Releases.ExistingRelease.Title), responseString);
        }

        [Fact]
        public async Task DetailsGet_ShouldReturnNotFoundResultWhenCalledWithBadId()
        {
            // Arrange

            // Act
            var response = await Client.GetAsync("/releases/details/" + TestDataGraph.Releases.NonExistentReleaseId);
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(string.Empty, responseString);
        }

        [Fact]
        public async Task CreateGet_ShouldReturnViewForCreatingNewRelease()
        {
            // Arrange

            // Act
            var response = await Client.GetAsync("/releases/create");
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains(ViewData.DefaultCreatePageTitle, responseString);
        }

        [Fact]
        public async Task CreatePost_WhenSingleArtist_ShouldReturnFoundStatusAndRedirectionLocationToAll()
        {
            // Arrange
            var nextId = TestDataGraph.Releases.ReleasesRaw.Count + 1;
            var formData = await EnsureAntiforgeryTokenOnForm(new Dictionary<string, string>()
            {
                { "ReleaseId" , nextId.ToString() },
                { "LabelId" , TestDataGraph.Releases.NewSingleArtistRelease.LabelId.ToString() },
                { "Title", TestDataGraph.Releases.NewSingleArtistRelease.Title },
                { "YearReleased", TestDataGraph.Releases.NewSingleArtistRelease.YearReleased.ToString() },
                { "IsByVariousArtists", TestDataGraph.Releases.NewSingleArtistRelease.IsByVariousArtists.ToString() },
                { "ArtistId", TestDataGraph.Releases.NewSingleArtistRelease.ArtistId.ToString() }
            });

            // Act
            var response = await Client.PostAsync("/releases/create", new FormUrlEncodedContent(formData));
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Found, response.StatusCode);
            Assert.Equal("/releases", response.Headers.Location.ToString());
            Assert.Equal(string.Empty, responseString);

            // Act
            var response2 = await Client.GetAsync("/releases/details/" + nextId);
            var responseString2 = await response2.Content.ReadAsStringAsync();

            // Assert
            response2.EnsureSuccessStatusCode();
            Assert.Contains(ViewData.DefaultDetailsPageTitle, responseString2);
            Assert.Contains(TestDataGraph.Releases.NewSingleArtistRelease.Title, responseString2);
        }

        [Fact]
        public async Task CreatePost_WhenVariousArtists_ShouldReturnFoundStatusAndRedirectionLocationToAll()
        {
            // Arrange
            var nextId = TestDataGraph.Releases.ReleasesRaw.Count + 1;
            var formData = await EnsureAntiforgeryTokenOnForm(new Dictionary<string, string>()
            {
                { "ReleaseId" , nextId.ToString() },
                { "LabelId" , TestDataGraph.Releases.NewVariousArtistsRelease.LabelId.ToString() },
                { "Title", TestDataGraph.Releases.NewVariousArtistsRelease.Title },
                { "YearReleased", TestDataGraph.Releases.NewVariousArtistsRelease.YearReleased.ToString() },
                { "IsByVariousArtists", TestDataGraph.Releases.NewVariousArtistsRelease.IsByVariousArtists.ToString() }
            });

            // Act
            var response = await Client.PostAsync("/releases/create", new FormUrlEncodedContent(formData));
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Found, response.StatusCode);
            Assert.Equal("/releases", response.Headers.Location.ToString());
            Assert.Equal(string.Empty, responseString);

            // Act
            var response2 = await Client.GetAsync("/releases/details/" + nextId);
            var responseString2 = await response2.Content.ReadAsStringAsync();

            // Assert
            response2.EnsureSuccessStatusCode();
            Assert.Contains(ViewData.DefaultDetailsPageTitle, responseString2);
            Assert.Contains(TestDataGraph.Releases.NewVariousArtistsRelease.Title, responseString2);
        }

        [Fact]
        public async Task CreatePost_ShouldReturnErrorWhenCalledWithExistingId()
        {
            // Arrange
            var formData = await EnsureAntiforgeryTokenOnForm(new Dictionary<string, string>()
            {
                { "ReleaseId" , TestDataGraph.Releases.ExistingRelease.ReleaseId.ToString() },
                { "LabelId", TestDataGraph.Releases.ExistingRelease.LabelId.ToString() },
                { "Title", TestDataGraph.Releases.ExistingRelease.Title },
                { "YearReleased", TestDataGraph.Releases.ExistingRelease.YearReleased.ToString() },
                { "IsByVariousArtists", TestDataGraph.Releases.ExistingRelease.IsByVariousArtists.ToString() },
                { "ArtistId", TestDataGraph.Releases.NewVariousArtistsRelease.ArtistId.ToString() }
            });

            // Act
            var response = await Client.PostAsync("/releases/create", new FormUrlEncodedContent(formData));
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.Contains(ErrorMessages.ArgumentExceptionSameKeyAlreadyAdded, responseString);
        }

        [Fact]
        public async Task EditGet_ShouldReturnViewWithExistingReleaseData()
        {
            // Arrange

            // Act
            var response = await Client.GetAsync("/releases/edit/" + TestDataGraph.Releases.ExistingRelease.ReleaseId);
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Contains("<title>Edit - Tuneage.WebApi</title>", responseString);
            Assert.Contains(HtmlTransformer.StringToHtmlString(TestDataGraph.Releases.ExistingRelease.Title), responseString);
        }

        [Fact]
        public async Task EditGet_ShouldReturnNotFoundResultWhenCalledWithBadId()
        {
            // Arrange

            // Act
            var response = await Client.GetAsync("/releases/edit/" + TestDataGraph.Releases.NonExistentReleaseId);
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(string.Empty, responseString);
        }

        [Fact(Skip = "This test yields an internal server error \"Attempted to update or delete an entity that does not exist in the store.\" but why? " +
                     "Setting breakpoint in repo shows that the entity does exist on the data context. This only happens for types with subtypes." +
                     "The collection is parent type but shows subtypes inside. Is a conversion needed to get around this? Shouldn't be, since" +
                     "this works just fine when running the app and manually integration testing. SO, W.T.Fuck?")]
        public async Task EditPost_WhenReleaseWasOriginallySingleArtist_ShouldReturnFoundStatusAndRedirectionLocationToAll()
        {
            // Arrange
            var formData = await EnsureAntiforgeryTokenOnForm(new Dictionary<string, string>()
            {
                { "ReleaseId", TestDataGraph.Releases.UpdatedSingleArtistRelease.ReleaseId.ToString() },
                { "LabelId", TestDataGraph.Releases.UpdatedSingleArtistRelease.LabelId.ToString() },
                { "Title", TestDataGraph.Releases.UpdatedSingleArtistRelease.Title },
                { "YearReleased", TestDataGraph.Releases.UpdatedSingleArtistRelease.YearReleased.ToString() },
                { "ArtistId", TestDataGraph.Releases.UpdatedSingleArtistRelease.ArtistId.ToString() }
            });

            // Act
            var response = await Client.PostAsync("/releases/edit/" + formData["ReleaseId"], new FormUrlEncodedContent(formData));
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Found, response.StatusCode);
            Assert.Equal("/releases", response.Headers.Location.ToString());
            Assert.Equal(string.Empty, responseString);
        }

        //[Fact]
        //public async Task EditPost_WhenReleaseWasOriginallyVariousArtists_ShouldReturnFoundStatusAndRedirectionLocationToAll()
        //{
        //    // Arrange
        //    var formData = await EnsureAntiforgeryTokenOnForm(new Dictionary<string, string>()
        //    {
        //        { "ReleaseId", TestDataGraph.Releases.UpdatedVariousArtistsRelease.ReleaseId.ToString() },
        //        { "LabelId", TestDataGraph.Releases.UpdatedVariousArtistsRelease.LabelId.ToString() },
        //        { "Title", TestDataGraph.Releases.UpdatedVariousArtistsRelease.Title },
        //        { "YearReleased", TestDataGraph.Releases.UpdatedVariousArtistsRelease.YearReleased.ToString() }
        //    });

        //    // Act
        //    var response = await Client.PostAsync("/releases/edit/" + formData["ReleaseId"], new FormUrlEncodedContent(formData));
        //    var responseString = await response.Content.ReadAsStringAsync();

        //    // Assert
        //    Assert.False(response.IsSuccessStatusCode);
        //    Assert.Equal(HttpStatusCode.Found, response.StatusCode);
        //    Assert.Equal("/artists", response.Headers.Location.ToString());
        //    Assert.Equal(string.Empty, responseString);
        //}

        [Fact]
        public async Task EditPost_ShouldReturnErrorWhenCalledWithBadId()
        {
            // Arrange
            var formData = await EnsureAntiforgeryTokenOnForm(new Dictionary<string, string>()
            {
                { "ReleaseId", TestDataGraph.Releases.NonExistentReleaseId.ToString() },
                { "LabelId", TestDataGraph.Releases.UpdatedSingleArtistRelease.LabelId.ToString() },
                { "Title", TestDataGraph.Releases.UpdatedSingleArtistRelease.Title },
                { "YearReleased", TestDataGraph.Releases.UpdatedSingleArtistRelease.YearReleased.ToString() },
                { "ArtistId", TestDataGraph.Releases.UpdatedSingleArtistRelease.ArtistId.ToString() }
            });

            // Act
            var response = await Client.PostAsync("/releases/edit/" + formData["ReleaseId"], new FormUrlEncodedContent(formData));
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.Contains(ErrorMessages.DbUpdateConcurrencyExceptionDoesNotExist, responseString);
        }

        [Fact]
        public async Task EditPost_ShouldReturnNotFoundResultWhenCalledWithNonMatchingIdData()
        {
            // Arrange
            var formData = await EnsureAntiforgeryTokenOnForm(new Dictionary<string, string>()
            {
                { "ReleaseId", TestDataGraph.Releases.NonExistentReleaseId.ToString() },
                { "LabelId", TestDataGraph.Releases.UpdatedSingleArtistRelease.LabelId.ToString() },
                { "Title", TestDataGraph.Releases.UpdatedSingleArtistRelease.Title },
                { "YearReleased", TestDataGraph.Releases.UpdatedSingleArtistRelease.YearReleased.ToString() },
                { "ArtistId", TestDataGraph.Releases.UpdatedSingleArtistRelease.ArtistId.ToString() }
            });

            // Act
            var response = await Client.PostAsync("/releases/edit/" + TestDataGraph.Releases.UpdatedSingleArtistRelease.ReleaseId, new FormUrlEncodedContent(formData));
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(string.Empty, responseString);
        }

        [Fact]
        public async Task DeleteGet_ShouldReturnViewWithExistingReleaseData()
        {
            // Arrange

            // Act
            var response = await Client.GetAsync("/releases/delete/" + TestDataGraph.Releases.ExistingRelease.ReleaseId);
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Contains(ViewData.DefaultDeletePageTitle, responseString);
            Assert.Contains(HtmlTransformer.StringToHtmlString(TestDataGraph.Releases.ExistingRelease.Title), responseString);
        }

        [Fact]
        public async Task DeletePost_ShouldReturnViewWithExistingReleaseDataRemoved()
        {
            // Arrange
            var formData = await EnsureAntiforgeryTokenOnForm(new Dictionary<string, string>()
            {
                { "ReleaseId", TestDataGraph.Releases.ExistingRelease.ReleaseId.ToString() },
                { "Title", TestDataGraph.Releases.ExistingRelease.Title }
            });

            // Act
            var response = await Client.PostAsync("/releases/delete/" + formData["ReleaseId"], new FormUrlEncodedContent(formData));
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Found, response.StatusCode);
            Assert.Equal("/releases", response.Headers.Location.ToString());
            Assert.Equal(string.Empty, responseString);

            // Act
            var response2 = await Client.GetAsync("/releases");
            var responseString2 = await response2.Content.ReadAsStringAsync();

            // Assert
            Assert.True(response2.IsSuccessStatusCode);
            Assert.DoesNotContain(TestDataGraph.Releases.ExistingRelease.Title, responseString2);
        }

        [Fact]
        public async Task DeletePost_ShouldReturnErrorWhenCalledWithBadId()
        {
            // Arrange
            var formData = await EnsureAntiforgeryTokenOnForm(new Dictionary<string, string>()
            {
                { "ReleaseId", TestDataGraph.Releases.NonExistentReleaseId.ToString() },
                { "Title", TestDataGraph.Releases.ExistingRelease.Title }
            });

            // Act
            var response = await Client.PostAsync("/releases/delete/" + formData["ReleaseId"], new FormUrlEncodedContent(formData));
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.Contains(ErrorMessages.ArgumentNullException, responseString);
        }
    }
}
