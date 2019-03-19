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
        private readonly Mock<DbSet<Artist>> _mockArtistSet;
        private readonly ArtistRepository _repository;
        private readonly Artist _newBand, _newSoloArtist, _newAlias;

        public ArtistRepositoryTests()
        {
            _mockArtistSet = new Mock<DbSet<Artist>>();

            _newBand = TestDataGraph.Artists.NewBand;
            _newSoloArtist = TestDataGraph.Artists.NewSoloArtist;
            _newAlias = TestDataGraph.Artists.NewAliasedArtist;
            var artists = TestDataGraph.Artists.ArtistsRaw;
            var data = artists.AsQueryable();

            SetupMockDbSet(_mockArtistSet, data);

            SetupMockSetOnMockContext(_mockArtistSet);

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

        [Fact]
        public async Task Create_WhenBand_ShouldCallContextToAddAndSave()
        {
            // Arrange
            var newBand = new Band { ArtistId = _newBand.ArtistId, Name = _newBand.Name };

            // Act
            await _repository.Create(newBand);

            // Assert
            _mockArtistSet.Verify(mas => mas.AddAsync(newBand, It.IsAny<CancellationToken>()), Times.Once);
            MockContext.Verify(mc => mc.SaveChangesAsync(It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task Create_WhenSoloArtist_ShouldCallContextToAddAndSave()
        {
            // Arrange
            var newSoloArtist = new SoloArtist { ArtistId = _newSoloArtist.ArtistId, Name = _newSoloArtist.Name };

            // Act
            await _repository.Create(newSoloArtist);

            // Assert
            _mockArtistSet.Verify(mas => mas.AddAsync(newSoloArtist, It.IsAny<CancellationToken>()), Times.Once);
            MockContext.Verify(mc => mc.SaveChangesAsync(It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task Create_WhenAlias_ShouldCallContextToAddAndSave()
        {
            // Arrange
            var newAliasedArtist = new AliasedArtist() { ArtistId = _newSoloArtist.ArtistId, Name = _newSoloArtist.Name, PrincipalArtistId = _newAlias.PrincipalArtistId };

            // Act
            await _repository.Create(newAliasedArtist);

            // Assert
            _mockArtistSet.Verify(mas => mas.AddAsync(newAliasedArtist, It.IsAny<CancellationToken>()), Times.Once);
            MockContext.Verify(mc => mc.SaveChangesAsync(It.IsAny<CancellationToken>()));
        }
    }
}
