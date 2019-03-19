using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Tuneage.Data.Repositories.Sql.EfCore;
using Tuneage.Data.TestData;
using Tuneage.Domain.Entities;
using Tuneage.Domain.Services;
using Tuneage.WebApi.Controllers.Mvc;
using Xunit;

namespace Tuneage.WebApi.Tests.Unit.Controllers.Mvc
{
    public class ArtistsControllerTests : UnitTestFixture
    {
        private readonly Mock<ArtistRepository> _mockRepository;
        private readonly Mock<ArtistService> _mockService;
        private readonly ArtistsController _controller;
        private readonly Artist _existingArtist, _existingArtistUpdated, _newBand, _newSoloArtist, _newAlias;
        protected const string DefaultViewActionName = "Index";

        public ArtistsControllerTests()
        {
            var mockArtistSet = new Mock<DbSet<Artist>>();

            _existingArtist = TestDataGraph.Artists.ExistingArtist;
            _existingArtistUpdated = TestDataGraph.Artists.UpdatedBand;
            _newBand = TestDataGraph.Artists.NewBand;
            _newSoloArtist = TestDataGraph.Artists.NewSoloArtist;
            _newAlias = TestDataGraph.Artists.NewAliasedArtist;
            var artists = TestDataGraph.Artists.ArtistsRaw;
            var data = artists.AsQueryable();

            SetupMockDbSet(mockArtistSet, data);

            SetupMockSetOnMockContext(mockArtistSet);

            _mockRepository = new Mock<ArtistRepository>(MockContext.Object);
            _mockRepository.Setup(mr => mr.GetAllAlphabetical()).Returns(Task.FromResult(TestDataGraph.Artists.ArtistsAlphabetizedByArtistName));
            _mockRepository.Setup(mr => mr.GetById(_existingArtist.ArtistId)).Returns(Task.FromResult(_existingArtist));
            _mockRepository.Setup(mr => mr.GetById(_existingArtistUpdated.ArtistId)).Returns(Task.FromResult(_existingArtistUpdated));

            _mockService = new Mock<ArtistService>();

            _controller = new ArtistsController(_mockRepository.Object, _mockService.Object);
        }

        [Fact]
        public async Task IndexGet_ShouldCallRepositoryToGetAllEntitiesAndReturnViewWithDataSortedAlphabeticallyByName()
        {
            // Arrange

            // Act
            var result = await _controller.Index();
            var viewResult = (ViewResult)result;
            var model = (List<Artist>)viewResult.Model;

            // Assert
            _mockRepository.Verify(mr => mr.GetAllAlphabetical(), Times.Once);
            Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
            Assert.Equal(model, TestDataGraph.Artists.ArtistsAlphabetizedByArtistName);
        }

        [Fact]
        public async Task DetailsGet_ShouldCallRepositoryToGetDetailsForExistingEntityAndReturnViewWithData()
        {
            // Arrange

            // Act
            var result = await _controller.Details(_existingArtist.ArtistId);
            var viewResult = (ViewResult)result;
            var model = (Artist)viewResult.Model;

            // Assert
            _mockRepository.Verify(mr => mr.GetById(_existingArtist.ArtistId), Times.Once);
            Assert.Null(viewResult.ViewName);
            Assert.Equal(_existingArtist, model);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(TestDataGraph.Artists.NonExistentArtistId)]
        public async Task DetailsGet_ShouldReturnNotFoundResultWhenCalledWithBadData(int? value)
        {
            // Arrange

            // Act
            var result = await _controller.Details(value);

            // Assert
            if (value != null) _mockRepository.Verify(mr => mr.GetById(It.IsAny<int>()));
            Assert.IsType<NotFoundResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, ((NotFoundResult)result).StatusCode);
        }

        [Fact]
        public void CreateGet_ShouldReturnView()
        {
            // Arrange

            // Act
            var result = _controller.Create();
            var viewResult = (ViewResult)result;

            // Assert
            Assert.Null(viewResult.ViewName);
            Assert.NotNull(viewResult.ViewData);
            Assert.Null(viewResult.ViewData.Model);
        }

        [Fact]
        public async Task CreatePost_WhenBand_ShouldCallRepositoryToToAddEntityAndRedirectToIndex()
        {
            // Arrange

            //Act
            var result = await _controller.Create(_newBand);
            var redirectToActionResult = (RedirectToActionResult)result;

            //Assert
            _mockRepository.Verify(mr => mr.Create(It.IsAny<Artist>()), Times.Once);
            _mockService.Verify(ms => ms.TransformArtistForCreation(It.IsAny<Artist>()), Times.Once);
            Assert.NotNull(redirectToActionResult);
            Assert.Equal(DefaultViewActionName, redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task CreatePost_WhenSoloArtist_ShouldCallRepositoryToToAddEntityAndRedirectToIndex()
        {
            // Arrange

            //Act
            var result = await _controller.Create(_newSoloArtist);
            var redirectToActionResult = (RedirectToActionResult)result;

            //Assert
            _mockRepository.Verify(mr => mr.Create(It.IsAny<Artist>()), Times.Once);
            _mockService.Verify(ms => ms.TransformArtistForCreation(It.IsAny<Artist>()), Times.Once);
            Assert.NotNull(redirectToActionResult);
            Assert.Equal(DefaultViewActionName, redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task CreatePost_WhenAlias_ShouldCallRepositoryToToAddEntityAndRedirectToIndex()
        {
            // Arrange

            //Act
            var result = await _controller.Create(_newAlias);
            var redirectToActionResult = (RedirectToActionResult)result;

            //Assert
            _mockRepository.Verify(mr => mr.Create(It.IsAny<Artist>()), Times.Once);
            _mockService.Verify(ms => ms.TransformArtistForCreation(It.IsAny<Artist>()), Times.Once);
            Assert.NotNull(redirectToActionResult);
            Assert.Equal(DefaultViewActionName, redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task CreatePost_ShouldOnlyReturnViewWithSameEntityWhenModelStateIsNotValid()
        {
            // Arrange
            _controller.ModelState.AddModelError("", "Error");

            // Act
            var result = await _controller.Create(_newBand);
            var viewResult = (ViewResult)result;
            var model = (Artist)viewResult.Model;

            // Assert
            Assert.Null(viewResult.ViewName);
            Assert.NotNull(viewResult.ViewData);
            Assert.Equal(_newBand, model);
        }

        [Fact]
        public async Task EditGet_ShouldCallRepositoryToGetEntityAndReturnViewWithData()
        {
            // Arrange

            // Act
            var result = await _controller.Edit(_existingArtist.ArtistId);
            var viewResult = (ViewResult)result;
            var model = (Artist)viewResult.Model;

            // Assert
            _mockRepository.Verify(mr => mr.GetById(_existingArtist.ArtistId), Times.Once);
            Assert.Null(viewResult.ViewName);
            Assert.NotNull(viewResult.ViewData);
            Assert.Equal(_existingArtist, model);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(TestDataGraph.Artists.NonExistentArtistId)]
        public async Task EditGet_ShouldCallRepositoryToGetEntityAndReturnNotFoundResultWhenCalledWithBadData(int? value)
        {
            // Arrange

            // Act
            var result = await _controller.Edit(value);

            // Assert
            if (value != null) _mockRepository.Verify(mr => mr.GetById(It.IsAny<int>()));
            Assert.IsType<NotFoundResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, ((NotFoundResult)result).StatusCode);
        }

        [Fact]
        public async Task EditPost_ShouldCallRepositoryToUpdateEntityAndRedirectToIndex()
        {
            // Arrange

            //Act
            var result = await _controller.Edit(_existingArtistUpdated.ArtistId, _existingArtistUpdated);
            var redirectToActionResult = (RedirectToActionResult)result;

            //Assert
            _mockRepository.Verify(mr => mr.Update(_existingArtistUpdated.ArtistId, It.IsAny<Artist>()), Times.Once);
            Assert.NotNull(redirectToActionResult);
            Assert.Equal(DefaultViewActionName, redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task EditPost_ShouldReturnNotFoundResultWhenCalledWithNonMatchingIdData()
        {
            // Arrange

            // Act
            var result = await _controller.Edit(TestDataGraph.Artists.NonExistentArtistId, _existingArtistUpdated);

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
            var result = await _controller.Edit(_existingArtistUpdated.ArtistId, _existingArtistUpdated);
            var viewResult = (ViewResult)result;
            var model = (Artist)viewResult.Model;

            // Assert
            Assert.Null(viewResult.ViewName);
            Assert.NotNull(viewResult.ViewData);
            Assert.Equal(_existingArtistUpdated, model);
        }

        [Fact]
        public async Task DeleteGet_ShouldCallRepositoryToGetEntityAndReturnViewWithData()
        {
            // Arrange

            // Act
            var result = await _controller.Delete(_existingArtist.ArtistId);
            var viewResult = (ViewResult)result;
            var model = (Artist)viewResult.Model;

            // Assert
            _mockRepository.Verify(mr => mr.GetById(_existingArtist.ArtistId), Times.Once);
            Assert.Null(viewResult.ViewName);
            Assert.NotNull(viewResult.ViewData);
            Assert.Equal(_existingArtist, model);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(TestDataGraph.Artists.NonExistentArtistId)]
        public async Task DeleteGet_ShouldReturnNotFoundResultWhenCalledWithBadData(int? value)
        {
            // Arrange

            // Act
            var result = await _controller.Delete(value);

            // Assert
            if (value != null) _mockRepository.Verify(mr => mr.GetById(It.IsAny<int>()));
            Assert.IsType<NotFoundResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, ((NotFoundResult)result).StatusCode);
        }

        [Fact]
        public async Task DeleteConfirmedPost_ShouldCallRepositoryToFindAndRemoveExistingEntity()
        {
            // Arrange

            // Act
            var result = await _controller.DeleteConfirmed(_existingArtist.ArtistId);
            var redirectToActionResult = (RedirectToActionResult)result;

            // Assert
            _mockRepository.Verify(mr => mr.GetById(_existingArtist.ArtistId), Times.Once);
            _mockRepository.Verify(mr => mr.Delete(_existingArtist.ArtistId), Times.Once);
            Assert.NotNull(redirectToActionResult);
            Assert.Equal(DefaultViewActionName, redirectToActionResult.ActionName);
        }
    }
}
