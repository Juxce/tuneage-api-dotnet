using System.Collections.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Moq;
using Newtonsoft.Json;
using Tuneage.Data.Constants;
using Tuneage.Data.Repositories.Sql.EfCore;
using Tuneage.Data.TestData;
using Tuneage.Domain.Entities;
using Tuneage.WebApi.Controllers.Mvc;
using Xunit;

namespace Tuneage.WebApi.Tests.Unit.Controllers.Mvc
{
    public class ReleasesControllerTests : UnitTestFixture
    {
        private readonly Mock<ReleaseRepository> _mockReleaseRepository;
        private readonly ReleasesController _controller;
        private readonly Release _existingRelease, _existingReleaseUpdated, _newSingleArtistRelease, _newVariousArtistsRelease;
        protected const string DefaultViewActionName = "Index";

        public ReleasesControllerTests()
        {
            var mockLabelSet = new Mock<DbSet<Label>>();
            var mockArtistSet = new Mock<DbSet<Artist>>();
            var mockReleaseSet = new Mock<DbSet<Release>>();

            _existingRelease = TestDataGraph.Releases.ExistingRelease;
            _existingReleaseUpdated = TestDataGraph.Releases.UpdatedSingleArtistRelease;
            _newSingleArtistRelease = TestDataGraph.Releases.NewSingleArtistRelease;
            _newVariousArtistsRelease = TestDataGraph.Releases.NewVariousArtistsRelease;
            var labelsData = TestDataGraph.Labels.LabelsRaw.AsQueryable();
            var artistsData = TestDataGraph.Artists.ArtistsRaw.AsQueryable();
            var releasesData = TestDataGraph.Releases.ReleasesRaw.AsQueryable();

            SetupMockDbSet(mockLabelSet, labelsData);
            SetupMockDbSet(mockArtistSet, artistsData);
            SetupMockDbSet(mockReleaseSet, releasesData);

            SetupMockSetOnMockContext(mockLabelSet);
            SetupMockSetOnMockContext(mockArtistSet);
            SetupMockSetOnMockContext(mockReleaseSet);

            var mockLabelRepository = new Mock<LabelRepository>(MockContext.Object);
            mockLabelRepository.Setup(mr => mr.GetAllAlphabetical()).Returns(Task.FromResult(TestDataGraph.Labels.LabelsAlphabetizedByLabelName));
            var mockArtistRepository = new Mock<ArtistRepository>(MockContext.Object);
            mockArtistRepository.Setup(mr => mr.GetAllAlphabetical()).Returns(Task.FromResult(TestDataGraph.Artists.ArtistsAlphabetizedByArtistName));
            _mockReleaseRepository = new Mock<ReleaseRepository>(MockContext.Object);
            _mockReleaseRepository.Setup(mrr => mrr.GetAllAlphabetical()).Returns(Task.FromResult(TestDataGraph.Releases.ReleasesAlphabetizedByTitle));
            _mockReleaseRepository.Setup(mrr => mrr.GetById(_existingRelease.ReleaseId)).Returns(Task.FromResult(_existingRelease));
            _mockReleaseRepository.Setup(mrr => mrr.GetById(_existingReleaseUpdated.ReleaseId)).Returns(Task.FromResult(_existingRelease));

            _controller = new ReleasesController(mockLabelRepository.Object, mockArtistRepository.Object, _mockReleaseRepository.Object);
        }

        [Fact]
        public async Task IndexGet_ShouldCallRepositoryToGetAllEntitiesAndReturnViewWithDataSortedAlphabeticallyByTitle()
        {
            // Arrange

            // Act
            var result = await _controller.Index();
            var viewResult = (ViewResult)result;
            var model = (List<Release>)viewResult.Model;

            // Assert
            _mockReleaseRepository.Verify(mr => mr.GetAllAlphabetical(), Times.Once);
            Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
            Assert.Equal(model, TestDataGraph.Releases.ReleasesAlphabetizedByTitle);
        }

        [Fact]
        public async Task DetailsGet_ShouldCallRepositoryToGetDetailsForExistingEntityAndReturnViewWithData()
        {
            // Arrange

            // Act
            var result = await _controller.Details(_existingRelease.ReleaseId);
            var viewResult = (ViewResult)result;
            var model = (Release)viewResult.Model;

            // Assert
            _mockReleaseRepository.Verify(mr => mr.GetById(_existingRelease.ReleaseId), Times.Once);
            Assert.Null(viewResult.ViewName);
            Assert.Equal(_existingRelease, model);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(TestDataGraph.Releases.NonExistentReleaseId)]
        public async Task DetailsGet_ShouldReturnNotFoundResultWhenCalledWithBadData(int? value)
        {
            // Arrange

            // Act
            var result = await _controller.Details(value);

            // Assert
            if (value != null) _mockReleaseRepository.Verify(mr => mr.GetById(It.IsAny<int>()));
            Assert.IsType<NotFoundResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, ((NotFoundResult)result).StatusCode);
        }

        [Fact]
        public void CreateGet_ShouldReturnViewWithAssociatedCollectionsData()
        {
            // Arrange

            // Act
            var result = _controller.Create();
            var viewResult = (ViewResult)result;
            var viewData = viewResult.ViewData;
            var returnedLabelsList = (SelectList)viewData["LabelId"];
            var returnedLabels = (List<Label>)returnedLabelsList.Items;
            var returnedArtistsList = (SelectList)viewData["ArtistId"];
            var returnedArtists = (List<Artist>)returnedArtistsList.Items;

            // Assert
            Assert.Null(viewResult.ViewName);
            Assert.NotNull(viewResult.ViewData);
            Assert.Null(viewResult.ViewData.Model);
            Assert.Equal(TestDataGraph.Labels.LabelsAlphabetizedByLabelName, returnedLabels);
            Assert.Equal(TestDataGraph.Artists.ArtistsAlphabetizedByArtistName, returnedArtists);
        }

        [Fact]
        public async Task CreatePost_WhenSingleArtist_ShouldCallRepositoryToToAddEntityAndRedirectToIndex()
        {
            // Arrange

            // Act
            var result = await _controller.Create(_newSingleArtistRelease);
            var redirectToActionResult = (RedirectToActionResult)result;

            // Assert
            _mockReleaseRepository.Verify(mr => mr.Create(It.IsAny<SingleArtistRelease>()), Times.Once);
            Assert.NotNull(redirectToActionResult);
            Assert.Equal(DefaultViewActionName, redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task CreatePost_WhenVariousArtists_ShouldCallRepositoryToToAddEntityAndRedirectToIndex()
        {
            // Arrange

            //Act
            var result = await _controller.Create(_newVariousArtistsRelease);
            var redirectToActionResult = (RedirectToActionResult)result;

            //Assert
            _mockReleaseRepository.Verify(mr => mr.Create(It.IsAny<VariousArtistsRelease>()), Times.Once);
            Assert.NotNull(redirectToActionResult);
            Assert.Equal(DefaultViewActionName, redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task CreatePost_ShouldOnlyReturnViewWithSameEntityWhenModelStateIsNotValid()
        {
            // Arrange
            _controller.ModelState.AddModelError("", "Error");

            // Act
            var result = await _controller.Create(_newSingleArtistRelease);
            var viewResult = (ViewResult)result;
            var model = (Release)viewResult.Model;
            var viewData = viewResult.ViewData;
            var returnedLabelsList = (SelectList)viewData["LabelId"];
            var returnedLabels = (List<Label>)returnedLabelsList.Items;
            var returnedArtistsList = (SelectList)viewData["ArtistId"];
            var returnedArtists = (List<Artist>)returnedArtistsList.Items;

            // Assert
            Assert.Null(viewResult.ViewName);
            Assert.NotNull(viewResult.ViewData);
            Assert.Equal(_newSingleArtistRelease, model);
            Assert.Equal(TestDataGraph.Labels.LabelsAlphabetizedByLabelName, returnedLabels);
            Assert.Equal(TestDataGraph.Artists.ArtistsAlphabetizedByArtistName, returnedArtists);
        }

        [Fact]
        public async Task EditGet_ShouldCallRepositoryToGetEntityAndReturnViewWithData()
        {
            // Arrange
            var expectedArtistListForDisplay = new List<Artist>
            {
                new Artist { ArtistId = 0, Name = DefaultValues.ArtistListDefaultForNoSelection }
            };
            expectedArtistListForDisplay.AddRange(TestDataGraph.Artists.ArtistsAlphabetizedByArtistName);

            // Act
            var result = await _controller.Edit(_existingRelease.ReleaseId);
            var viewResult = (ViewResult)result;
            var model = (Release)viewResult.Model;
            var viewData = viewResult.ViewData;
            var returnedLabelsList = (SelectList)viewData["LabelId"];
            var returnedLabels = (List<Label>)returnedLabelsList.Items;
            var returnedArtistsList = (SelectList)viewData["ArtistId"];
            var returnedArtists = (List<Artist>)returnedArtistsList.Items;

            // Assert
            _mockReleaseRepository.Verify(mr => mr.GetById(_existingRelease.ReleaseId), Times.Once);
            Assert.Null(viewResult.ViewName);
            Assert.NotNull(viewResult.ViewData);
            Assert.Equal(_existingRelease, model);
            Assert.Equal(TestDataGraph.Labels.LabelsAlphabetizedByLabelName, returnedLabels);
            Assert.Equal(JsonConvert.SerializeObject(expectedArtistListForDisplay), JsonConvert.SerializeObject(returnedArtists));
        }

        [Theory]
        [InlineData(null)]
        [InlineData(TestDataGraph.Releases.NonExistentReleaseId)]
        public async Task EditGet_ShouldCallRepositoryToGetEntityAndReturnNotFoundResultWhenCalledWithBadData(int? value)
        {
            // Arrange

            // Act
            var result = await _controller.Edit(value);

            // Assert
            if (value != null) _mockReleaseRepository.Verify(mr => mr.GetById(It.IsAny<int>()));
            Assert.IsType<NotFoundResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, ((NotFoundResult)result).StatusCode);
        }

        [Fact]
        public async Task EditPost_ShouldCallRepositoryToUpdateEntityAndRedirectToIndex()
        {
            // Arrange

            //Act
            var result = await _controller.Edit(_existingReleaseUpdated.ReleaseId, _existingReleaseUpdated);
            var redirectToActionResult = (RedirectToActionResult)result;

            //Assert
            _mockReleaseRepository.Verify(mr => mr.Update(_existingReleaseUpdated.ReleaseId, _existingReleaseUpdated), Times.Once);
            Assert.NotNull(redirectToActionResult);
            Assert.Equal(DefaultViewActionName, redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task EditPost_ShouldReturnNotFoundResultWhenCalledWithNonMatchingIdData()
        {
            // Arrange

            // Act
            var result = await _controller.Edit(TestDataGraph.Releases.NonExistentReleaseId, _existingReleaseUpdated);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, ((NotFoundResult)result).StatusCode);
        }

        [Fact]
        public async Task EditPost_ShouldOnlyReturnViewWithSameEntityWhenModelStateIsNotValid()
        {
            // Arrange
            _controller.ModelState.AddModelError("", "Error");

            // Act
            var result = await _controller.Edit(_existingReleaseUpdated.ReleaseId, _existingReleaseUpdated);
            var viewResult = (ViewResult)result;
            var model = (Release)viewResult.Model;

            // Assert
            Assert.Null(viewResult.ViewName);
            Assert.NotNull(viewResult.ViewData);
            Assert.Equal(_existingReleaseUpdated, model);
        }

        [Fact]
        public async Task DeleteGet_ShouldCallRepositoryToGetEntityAndReturnViewWithData()
        {
            // Arrange

            // Act
            var result = await _controller.Delete(_existingRelease.ReleaseId);
            var viewResult = (ViewResult)result;
            var model = (Release)viewResult.Model;

            // Assert
            _mockReleaseRepository.Verify(mr => mr.GetById(_existingRelease.ReleaseId), Times.Once);
            Assert.Null(viewResult.ViewName);
            Assert.NotNull(viewResult.ViewData);
            Assert.Equal(_existingRelease, model);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(TestDataGraph.Releases.NonExistentReleaseId)]
        public async Task DeleteGet_ShouldReturnNotFoundResultWhenCalledWithBadData(int? value)
        {
            // Arrange

            // Act
            var result = await _controller.Delete(value);

            // Assert
            if (value != null) _mockReleaseRepository.Verify(mr => mr.GetById(It.IsAny<int>()));
            Assert.IsType<NotFoundResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, ((NotFoundResult)result).StatusCode);
        }

        [Fact]
        public async Task DeleteConfirmedPost_ShouldCallRepositoryToFindAndRemoveExistingEntity()
        {
            // Arrange

            // Act
            var result = await _controller.DeleteConfirmed(_existingRelease.ReleaseId);
            var redirectToActionResult = (RedirectToActionResult)result;

            // Assert
            _mockReleaseRepository.Verify(mr => mr.GetById(_existingRelease.ReleaseId), Times.Once);
            _mockReleaseRepository.Verify(mr => mr.Delete(_existingRelease.ReleaseId), Times.Once);
            Assert.NotNull(redirectToActionResult);
            Assert.Equal(DefaultViewActionName, redirectToActionResult.ActionName);
        }
    }
}
