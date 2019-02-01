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
using Tuneage.WebApi.Controllers.Api;
using Xunit;

namespace Tuneage.WebApi.Tests.Unit.Controllers.Api
{
    public class ArtistsControllerTests : UnitTestFixture
    {
        private readonly Mock<ArtistRepository> _mockRepository;
        private readonly ArtistsController _controller;
        private readonly Artist _existingArtist, _existingArtistUpdated, _newBand, _newSoloArtist, _newAlias;

        public ArtistsControllerTests()
        {
            var mockArtistSet = new Mock<DbSet<Artist>>();

            _existingArtist = TestDataGraph.Artists.ExistingArtist;
            _existingArtistUpdated = TestDataGraph.Artists.UpdatedArtist;
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

            _controller = new ArtistsController(_mockRepository.Object);
        }

        [Fact]
        public async Task GetArtists_ShouldCallRepositoryToGetAllEntitiesAndReturnDataSortedAlphabeticallyByName()
        {
            // Arrange

            // Act
            var result = await _controller.GetArtists();
            var model = (List<Artist>)result.Value;

            // Assert
            _mockRepository.Verify(mr => mr.GetAllAlphabetical(), Times.Once);
            Assert.IsType<ActionResult<IEnumerable<Artist>>>(result);
            Assert.Equal(TestDataGraph.Artists.ArtistsAlphabetizedByArtistName, model);
        }

        [Fact]
        public async Task GetArtist_ShouldCallRepositoryToGetDetailsForExistingEntityAndReturnData()
        {
            // Arrange

            // Act
            var result = await _controller.GetArtist(_existingArtist.ArtistId);
            var model = result.Value;

            // Assert
            _mockRepository.Verify(mr => mr.GetById(_existingArtist.ArtistId), Times.Once);
            Assert.IsType<ActionResult<Artist>>(result);
            Assert.Equal(model, _existingArtist);
        }

        [Fact]
        public async Task GetArtist_ShouldReturnActionResultWithNotFoundResultAttachedWhenCalledWithBadId()
        {
            // Arrange

            // Act
            var result = await _controller.GetArtist(TestDataGraph.Artists.NonExistentArtistId);

            // Assert
            _mockRepository.Verify(mr => mr.GetById(TestDataGraph.Artists.NonExistentArtistId), Times.Once);
            Assert.IsType<ActionResult<Artist>>(result);
            Assert.IsType<NotFoundResult>(result.Result);
            Assert.Null(result.Value);
            Assert.Equal((int)HttpStatusCode.NotFound, ((NotFoundResult)result.Result).StatusCode);
        }

        [Fact]
        public async Task PutArtist_ShouldCallRepositoryToUpdateEntityAndReturnNoContentResult()
        {
            // Arrange

            // Act
            var result = await _controller.PutArtist(_existingArtistUpdated.ArtistId, _existingArtistUpdated);

            // Assert
            _mockRepository.Verify(mr => mr.SetModified(_existingArtistUpdated), Times.Once);
            _mockRepository.Verify(mr => mr.SaveChangesAsync(), Times.Once);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PutArtist_ShouldReturnBadRequestResultWhenCalledWithNonMatchingIdData()
        {
            // Arrange

            // Act
            var result = await _controller.PutArtist(TestDataGraph.Artists.NonExistentArtistId, _existingArtistUpdated);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task PostArtist_WhenBand_ShouldCallRepositoryToAddEntityAndReturnNewEntityData()
        {
            // Arrange

            // Act
            var result = await _controller.PostArtist(_newBand); 
            var createdAtActionResult = (CreatedAtActionResult)result.Result;
            var model = createdAtActionResult.Value;

            // Assert
            _mockRepository.Verify(mr => mr.Create(It.IsAny<Band>()), Times.Once);
            Assert.IsType<ActionResult<Artist>>(result);
            Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(_newBand, model);
        }

        [Fact]
        public async Task PostArtist_WhenSoloArtist_ShouldCallRepositoryToAddEntityAndReturnNewEntityData()
        {
            // Arrange

            // Act
            var result = await _controller.PostArtist(_newSoloArtist);
            var createdAtActionResult = (CreatedAtActionResult)result.Result;
            var model = createdAtActionResult.Value;

            // Assert
            _mockRepository.Verify(mr => mr.Create(It.IsAny<SoloArtist>()), Times.Once);
            Assert.IsType<ActionResult<Artist>>(result);
            Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(_newSoloArtist, model);
        }

        [Fact]
        public async Task PostArtist_WhenAlias_ShouldCallRepositoryToAddEntityAndReturnNewEntityData()
        {
            // Arrange

            // Act
            var result = await _controller.PostArtist(_newAlias);
            var createdAtActionResult = (CreatedAtActionResult)result.Result;
            var model = createdAtActionResult.Value;

            // Assert
            _mockRepository.Verify(mr => mr.Create(It.IsAny<AliasedArtist>()), Times.Once);
            Assert.IsType<ActionResult<Artist>>(result);
            Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(_newAlias, model);
        }

        [Fact]
        public async Task DeleteArtist_ShouldCallRepositoryToFindAndRemoveExistingEntityAndReturnDeletedEntityData()
        {
            // Arrange

            // Act
            var result = await _controller.DeleteArtist(_existingArtist.ArtistId);
            var model = result.Value;

            // Assert
            _mockRepository.Verify(mr => mr.GetById(_existingArtist.ArtistId), Times.Once);
            _mockRepository.Verify(mr => mr.Delete(_existingArtist.ArtistId), Times.Once);
            Assert.Equal(_existingArtist, model);
        }

        [Fact]
        public async Task DeleteArtist_ShouldCallRepositoryToGetEntityAndReturnActionResultWithNotFoundResultAttachedWhenCalledWithBadId()
        {
            // Arrange

            // Act
            var result = await _controller.DeleteArtist(TestDataGraph.Artists.NonExistentArtistId);

            // Assert
            _mockRepository.Verify(mr => mr.GetById(TestDataGraph.Artists.NonExistentArtistId), Times.Once);
            Assert.IsType<ActionResult<Artist>>(result);
            Assert.IsType<NotFoundResult>(result.Result);
            Assert.Null(result.Value);
            Assert.Equal((int)HttpStatusCode.NotFound, ((NotFoundResult)result.Result).StatusCode);
        }
    }
}
