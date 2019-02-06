using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using Tuneage.Data.Repositories.Sql.EfCore;
using Tuneage.Data.TestData;
using Tuneage.Domain.Entities;
using Xunit;
using Tuneage.Data.Constants;

namespace Tuneage.WebApi.Tests.Unit.Repositories.Sql.EfCore
{
    public class ReleaseRepositoryTests : UnitTestFixture
    {
        private readonly Mock<DbSet<Release>> _mockLabelSet;
        private readonly ReleaseRepository _repository;
        private readonly Release _newSingleArtistRelease, _newVariousArtistsRelease;

        public ReleaseRepositoryTests()
        {
            _mockLabelSet = new Mock<DbSet<Release>>();

            _newSingleArtistRelease = TestDataGraph.Releases.NewSingleArtistRelease;
            _newVariousArtistsRelease = TestDataGraph.Releases.NewVariousArtistsRelease;
            var releases = TestDataGraph.Releases.ReleasesRaw;
            var data = releases.AsQueryable();

            SetupMockDbSet(_mockLabelSet, data);

            SetupMockSetOnMockContext(_mockLabelSet);

            _repository = new ReleaseRepository(MockContext.Object);
        }

        [Fact]
        public void GetAllAlphabetical_ShouldCallContextToGetAllAndReturnDataSortedAlphabeticallyByTitle()
        {
            // Arrange

            // Act
            var result = _repository.GetAllAlphabetical();
            var model = result.Result;
            var variousArtistsRelease = model[11];
            var variousArtistsDisplayName = variousArtistsRelease.Artist.Name;

            // Assert
            Assert.IsType<Task<List<Release>>>(result);
            Assert.Equal(TestDataGraph.Releases.ReleasesAlphabetizedByTitle, model);
            Assert.Equal(DefaultValues.VariousArtistsDisplayName, variousArtistsDisplayName);
        }

        [Fact]
        public async void Create_WhenSingleArtistRelease_ShouldCallContextToAddAndSave()
        {
            // Arrange
            var newSingleArtistRelease = new SingleArtistRelease()
            {
                ReleaseId = _newSingleArtistRelease.ReleaseId,
                LabelId = _newSingleArtistRelease.LabelId,
                Title = _newSingleArtistRelease.Title,
                YearReleased = _newSingleArtistRelease.YearReleased,
                ArtistId = _newSingleArtistRelease.ArtistId
            };

            // Act
            await _repository.Create(newSingleArtistRelease);

            // Assert
            _mockLabelSet.Verify(mls => mls.AddAsync(newSingleArtistRelease, It.IsAny<CancellationToken>()), Times.Once);
            MockContext.Verify(mc => mc.SaveChangesAsync(It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async void Create_WhenVariousArtistsRelease_ShouldCallContextToAddAndSave()
        {
            // Arrange
            var newVariousArtistsRelease = new VariousArtistsRelease()
            {
                ReleaseId = _newVariousArtistsRelease.ReleaseId,
                LabelId = _newVariousArtistsRelease.LabelId,
                Title = _newVariousArtistsRelease.Title,
                YearReleased = _newVariousArtistsRelease.YearReleased
            };

            // Act
            await _repository.Create(newVariousArtistsRelease);

            // Assert
            _mockLabelSet.Verify(mls => mls.AddAsync(newVariousArtistsRelease, It.IsAny<CancellationToken>()), Times.Once);
            MockContext.Verify(mc => mc.SaveChangesAsync(It.IsAny<CancellationToken>()));
        }
    }
}
