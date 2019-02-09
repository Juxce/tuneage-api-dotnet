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
    public class ReleasesControllerTests : UnitTestFixture
    {
        private readonly Mock<ReleaseRepository> _mockReleaseRepository;
        private readonly ReleasesController _controller;
        private readonly Release _existingRelease, _existingReleaseUpdated, _newSingleArtistRelease, _newVariousArtistsRelease;

        public ReleasesControllerTests()
        {
            var mockReleaseSet = new Mock<DbSet<Release>>();

            _existingRelease = TestDataGraph.Releases.ExistingRelease;
            _existingReleaseUpdated = TestDataGraph.Releases.UpdatedSingleArtistRelease;
            _newSingleArtistRelease = TestDataGraph.Releases.NewSingleArtistRelease;
            _newVariousArtistsRelease = TestDataGraph.Releases.NewVariousArtistsRelease;
            var releasesData = TestDataGraph.Releases.ReleasesRaw.AsQueryable();

            SetupMockDbSet(mockReleaseSet, releasesData);

            SetupMockSetOnMockContext(mockReleaseSet);

            _mockReleaseRepository = new Mock<ReleaseRepository>(MockContext.Object);
            _mockReleaseRepository.Setup(mrr => mrr.GetAllAlphabetical()).Returns(Task.FromResult(TestDataGraph.Releases.ReleasesAlphabetizedByTitle));
            _mockReleaseRepository.Setup(mrr => mrr.GetById(_existingRelease.ReleaseId)).Returns(Task.FromResult(_existingRelease));

            _controller = new ReleasesController(_mockReleaseRepository.Object);
        }

        [Fact]
        public async Task GetReleases_ShouldCallRepositoryToGetAllEntitiesAndReturnDataSortedAlphabeticallyByName()
        {
            // Arrange

            // Act
            var result = await _controller.GetReleases();
            var model = (List<Release>)result.Value;

            // Assert
            _mockReleaseRepository.Verify(mr => mr.GetAllAlphabetical(), Times.Once);
            Assert.IsType<ActionResult<IEnumerable<Release>>>(result);
            Assert.Equal(TestDataGraph.Releases.ReleasesAlphabetizedByTitle, model);
        }

        [Fact]
        public async Task GetReleases_ShouldCallRepositoryToGetDetailsForExistingEntityAndReturnData()
        {
            // Arrange

            // Act
            var result = await _controller.GetRelease(_existingRelease.ReleaseId);
            var model = result.Value;

            // Assert
            _mockReleaseRepository.Verify(mr => mr.GetById(_existingRelease.ReleaseId), Times.Once);
            Assert.IsType<ActionResult<Release>>(result);
            Assert.Equal(model, _existingRelease);
        }

        [Fact]
        public async Task GetRelease_ShouldReturnActionResultWithNotFoundResultAttachedWhenCalledWithBadId()
        {
            // Arrange

            // Act
            var result = await _controller.GetRelease(TestDataGraph.Releases.NonExistentReleaseId);

            // Assert
            _mockReleaseRepository.Verify(mr => mr.GetById(TestDataGraph.Releases.NonExistentReleaseId), Times.Once);
            Assert.IsType<ActionResult<Release>>(result);
            Assert.IsType<NotFoundResult>(result.Result);
            Assert.Null(result.Value);
            Assert.Equal((int)HttpStatusCode.NotFound, ((NotFoundResult)result.Result).StatusCode);
        }

        [Fact]
        public async Task PutRelease_ShouldCallRepositoryToUpdateEntityAndReturnNoContentResult()
        {
            // Arrange

            // Act
            var result = await _controller.PutRelease(_existingReleaseUpdated.ReleaseId, _existingReleaseUpdated);

            // Assert
            _mockReleaseRepository.Verify(mr => mr.SetModified(_existingReleaseUpdated), Times.Once);
            _mockReleaseRepository.Verify(mr => mr.SaveChangesAsync(), Times.Once);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PutRelease_ShouldReturnBadRequestResultWhenCalledWithNonMatchingIdData()
        {
            // Arrange

            // Act
            var result = await _controller.PutRelease(TestDataGraph.Artists.NonExistentArtistId, _existingReleaseUpdated);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task PostRelease_WhenSingleArtist_ShouldCallRepositoryToAddEntityAndReturnNewEntityData()
        {
            // Arrange

            // Act
            var result = await _controller.PostRelease(_newSingleArtistRelease); 
            var createdAtActionResult = (CreatedAtActionResult)result.Result;
            var model = createdAtActionResult.Value;

            // Assert
            _mockReleaseRepository.Verify(mr => mr.Create(It.IsAny<SingleArtistRelease>()), Times.Once);
            Assert.IsType<ActionResult<Release>>(result);
            Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(_newSingleArtistRelease, model);
        }

        [Fact]
        public async Task PostRelease_WhenVariousArtists_ShouldCallRepositoryToAddEntityAndReturnNewEntityData()
        {
            // Arrange

            // Act
            var result = await _controller.PostRelease(_newVariousArtistsRelease);
            var createdAtActionResult = (CreatedAtActionResult)result.Result;
            var model = createdAtActionResult.Value;

            // Assert
            _mockReleaseRepository.Verify(mr => mr.Create(It.IsAny<VariousArtistsRelease>()), Times.Once);
            Assert.IsType<ActionResult<Release>>(result);
            Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(_newVariousArtistsRelease, model);
        }

        [Fact]
        public async Task DeleteRelease_ShouldCallRepositoryToFindAndRemoveExistingEntityAndReturnDeletedEntityData()
        {
            // Arrange

            // Act
            var result = await _controller.DeleteRelease(_existingRelease.ReleaseId);
            var model = result.Value;

            // Assert
            _mockReleaseRepository.Verify(mr => mr.GetById(_existingRelease.ReleaseId), Times.Once);
            _mockReleaseRepository.Verify(mr => mr.Delete(_existingRelease.ReleaseId), Times.Once);
            Assert.Equal(_existingRelease, model);
        }

        [Fact]
        public async Task DeleteRelease_ShouldCallRepositoryToGetEntityAndReturnActionResultWithNotFoundResultAttachedWhenCalledWithBadId()
        {
            // Arrange

            // Act
            var result = await _controller.DeleteRelease(TestDataGraph.Releases.NonExistentReleaseId);

            // Assert
            _mockReleaseRepository.Verify(mr => mr.GetById(TestDataGraph.Releases.NonExistentReleaseId), Times.Once);
            Assert.IsType<ActionResult<Release>>(result);
            Assert.IsType<NotFoundResult>(result.Result);
            Assert.Null(result.Value);
            Assert.Equal((int)HttpStatusCode.NotFound, ((NotFoundResult)result.Result).StatusCode);
        }
    }
}
