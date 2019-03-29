using System.Collections.Generic;
using System.Linq;
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
        private readonly ReleaseRepository _repository;

        public ReleaseRepositoryTests()
        {
            var mockLabelSet = new Mock<DbSet<Release>>();

            var releases = TestDataGraph.Releases.ReleasesRaw;
            var data = releases.AsQueryable();

            SetupMockDbSet(mockLabelSet, data);

            SetupMockSetOnMockContext(mockLabelSet);

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
    }
}
