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

namespace Tuneage.WebApi.Tests.Unit.Repositories.Sql.EfCore
{
    public class ArtistRepositoryTests : UnitTestFixture
    {
        private readonly ArtistRepository _repository;

        public ArtistRepositoryTests()
        {
            var mockArtistSet = new Mock<DbSet<Artist>>();

            var artists = TestDataGraph.Artists.ArtistsRaw;
            var data = artists.AsQueryable();

            SetupMockDbSet(mockArtistSet, data);

            SetupMockSetOnMockContext(mockArtistSet);

            _repository = new ArtistRepository(MockContext.Object);
        }

        [Fact]
        public void GetAllAlphabetical_ShouldCallContextToGetAllAndReturnDataSortedAlphabeticallyByName()
        {
            // Arrange

            // Act
            var result = _repository.GetAllAlphabetical();
            var model = result.Result;

            // Assert
            Assert.IsType<Task<List<Artist>>>(result);
            Assert.Equal(TestDataGraph.Artists.ArtistsAlphabetizedByArtistName, model);
        }
    }
}
