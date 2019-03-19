using Tuneage.Data.TestData;
using Tuneage.Domain.Entities;
using Tuneage.Domain.Services;
using Xunit;

namespace Tuneage.WebApi.Tests.Unit.Services
{
    public class ArtistServiceTests : UnitTestFixture
    {
        private readonly ArtistService _service;

        public ArtistServiceTests()
        {
            _service = new ArtistService();
        }

        [Fact]
        public void ShouldTransformNewAliasedArtistForCreation()
        {
            // Arrange
            var newArtist = TestDataGraph.Artists.NewAliasedArtist;

            // Act
            var transformedArtist = _service.TransformArtistForCreation(newArtist);

            // Assert
            Assert.IsType<AliasedArtist>(transformedArtist);
            Assert.Equal(newArtist.ArtistId, transformedArtist.ArtistId);
            Assert.Equal(newArtist.Name, transformedArtist.Name);
            Assert.Equal(newArtist.IsBand, transformedArtist.IsBand);
            Assert.Equal(newArtist.IsPrinciple, transformedArtist.IsPrinciple);
            Assert.Equal(newArtist.PrincipalArtistId, transformedArtist.PrincipalArtistId);
        }

        [Fact]
        public void ShouldTransformNewSoloArtistForCreation()
        {
            // Arrange
            var newArtist = TestDataGraph.Artists.NewSoloArtist;

            // Act
            var transformedArtist = _service.TransformArtistForCreation(newArtist);

            // Assert
            Assert.IsType<SoloArtist>(transformedArtist);
            Assert.Equal(newArtist.ArtistId, transformedArtist.ArtistId);
            Assert.Equal(newArtist.Name, transformedArtist.Name);
            Assert.False(transformedArtist.IsBand);
            Assert.True(transformedArtist.IsPrinciple);
        }

        [Fact]
        public void ShouldTransformNewBandForCreation()
        {
            // Arrange
            var newArtist = TestDataGraph.Artists.NewBand;

            // Act
            var transformedArtist = _service.TransformArtistForCreation(newArtist);

            // Assert
            Assert.IsType<Band>(transformedArtist);
            Assert.Equal(newArtist.ArtistId, transformedArtist.ArtistId);
            Assert.Equal(newArtist.Name, transformedArtist.Name);
            Assert.True(transformedArtist.IsBand);
            Assert.True(transformedArtist.IsPrinciple);
        }

        [Fact]
        public void ShouldTransformSoloArtistForUpdate()
        {
            // Arrange
            var preExistingSoloArtist = TestDataGraph.Artists.Artist18;
            var updatedSoloArtist = TestDataGraph.Artists.UpdatedSoloArtist;

            // Act
            var transformedArtist = _service.TransformSoloArtistForUpdate(preExistingSoloArtist, updatedSoloArtist);

            // Assert
            Assert.IsType<SoloArtist>(transformedArtist);
            Assert.Equal(updatedSoloArtist.ArtistId, transformedArtist.ArtistId);
            Assert.Equal(updatedSoloArtist.Name, transformedArtist.Name);
            Assert.Equal(updatedSoloArtist.IsBand, transformedArtist.IsBand);
            Assert.Equal(updatedSoloArtist.IsPrinciple, transformedArtist.IsPrinciple);
            Assert.Equal(updatedSoloArtist.PrincipalArtistId, transformedArtist.PrincipalArtistId);
        }

        [Fact]
        public void ShouldTransformBandForUpdate()
        {
            // Arrange
            var preExistingBand = TestDataGraph.Artists.Artist17;
            var updatedBand = TestDataGraph.Artists.UpdatedBand;

            // Act
            var transformedArtist = _service.TransformBandForUpdate(preExistingBand, updatedBand);

            // Assert
            Assert.IsType<Band>(transformedArtist);
            Assert.Equal(updatedBand.ArtistId, transformedArtist.ArtistId);
            Assert.Equal(updatedBand.Name, transformedArtist.Name);
            Assert.Equal(updatedBand.IsBand, transformedArtist.IsBand);
            Assert.Equal(updatedBand.IsPrinciple, transformedArtist.IsPrinciple);
            Assert.Equal(updatedBand.PrincipalArtistId, transformedArtist.PrincipalArtistId);
        }

        [Fact]
        public void ShouldTransformAliasForUpdate()
        {
            // Arrange
            var preExistingAlias = TestDataGraph.Artists.Artist26;
            var updatedAlias = TestDataGraph.Artists.UpdatedAlias;

            // Act
            var transformedArtist = _service.TransformAliasForUpdate(preExistingAlias, updatedAlias);

            // Assert
            Assert.IsType<AliasedArtist>(transformedArtist);
            Assert.Equal(updatedAlias.ArtistId, transformedArtist.ArtistId);
            Assert.Equal(updatedAlias.Name, transformedArtist.Name);
            Assert.Equal(updatedAlias.IsBand, transformedArtist.IsBand);
            Assert.Equal(updatedAlias.IsPrinciple, transformedArtist.IsPrinciple);
            Assert.Equal(updatedAlias.PrincipalArtistId, transformedArtist.PrincipalArtistId);
        }
    }
}
