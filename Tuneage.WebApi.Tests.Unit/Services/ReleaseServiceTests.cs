using Tuneage.Data.TestData;
using Tuneage.Domain.Entities;
using Tuneage.Domain.Services;
using Xunit;

namespace Tuneage.WebApi.Tests.Unit.Services
{
    public class ReleaseServiceTests : UnitTestFixture
    {
        private readonly IReleaseService _service;

        public ReleaseServiceTests()
        {
            _service = new ReleaseService();
        }

        [Fact]
        public void ShouldTransformNewSingleArtistReleaseForCreation()
        {
            // Arrange
            var newRelease = TestDataGraph.Releases.NewSingleArtistRelease;

            // Act
            var transformedRelease = _service.TransformReleaseForCreation(newRelease);

            // Assert
            Assert.IsType<SingleArtistRelease>(transformedRelease);
            Assert.Equal(newRelease.ReleaseId, transformedRelease.ReleaseId);
            Assert.Equal(newRelease.Title, transformedRelease.Title);
            Assert.Equal(newRelease.LabelId, transformedRelease.LabelId);
            Assert.Equal(newRelease.YearReleased, transformedRelease.YearReleased);
            Assert.Equal(newRelease.ArtistId, transformedRelease.ArtistId);
            Assert.Equal(newRelease.IsByVariousArtists, transformedRelease.IsByVariousArtists);
        }

        [Fact]
        public void ShouldTransformNewVariousArtistsReleaseForCreation()
        {
            // Arrange
            var newRelease = TestDataGraph.Releases.NewVariousArtistsRelease;

            // Act
            var transformedRelease = _service.TransformReleaseForCreation(newRelease);

            // Assert
            Assert.IsType<VariousArtistsRelease>(transformedRelease);
            Assert.Equal(newRelease.ReleaseId, transformedRelease.ReleaseId);
            Assert.Equal(newRelease.Title, transformedRelease.Title);
            Assert.Equal(newRelease.LabelId, transformedRelease.LabelId);
            Assert.Equal(newRelease.YearReleased, transformedRelease.YearReleased);
            Assert.Equal(newRelease.ArtistId, transformedRelease.ArtistId);
            Assert.Equal(newRelease.IsByVariousArtists, transformedRelease.IsByVariousArtists);
        }

        [Fact]
        public void ShouldTransformSingleArtistReleaseForUpdate()
        {
            // Arrange
            var preExistingSingleArtistRelease = TestDataGraph.Releases.Release09;
            var updatedSingleArtistRelease = TestDataGraph.Releases.UpdatedSingleArtistRelease;

            // Act
            var transformedRelease = _service.TransformSingleArtistReleaseForUpdate(preExistingSingleArtistRelease, updatedSingleArtistRelease);

            // Assert
            Assert.IsType<SingleArtistRelease>(transformedRelease);
            Assert.Equal(updatedSingleArtistRelease.ReleaseId, transformedRelease.ReleaseId);
            Assert.Equal(updatedSingleArtistRelease.Title, transformedRelease.Title);
            Assert.Equal(updatedSingleArtistRelease.LabelId, transformedRelease.LabelId);
            Assert.Equal(updatedSingleArtistRelease.YearReleased, transformedRelease.YearReleased);
            Assert.Equal(updatedSingleArtistRelease.ArtistId, transformedRelease.ArtistId);
            Assert.Equal(updatedSingleArtistRelease.IsByVariousArtists, transformedRelease.IsByVariousArtists);
        }

        [Fact]
        public void ShouldTransformVariousArtistsReleaseForUpdate()
        {
            // Arrange
            var preExistingVariousArtistsRelease = TestDataGraph.Releases.Release04;
            var updatedVariousArtistsRelease = TestDataGraph.Releases.UpdatedVariousArtistsRelease;

            // Act
            var transformedRelease = _service.TransformVariousArtistsReleaseForUpdate(preExistingVariousArtistsRelease, updatedVariousArtistsRelease);

            // Assert
            Assert.IsType<VariousArtistsRelease>(transformedRelease);
            Assert.Equal(updatedVariousArtistsRelease.ReleaseId, transformedRelease.ReleaseId);
            Assert.Equal(updatedVariousArtistsRelease.Title, transformedRelease.Title);
            Assert.Equal(updatedVariousArtistsRelease.LabelId, transformedRelease.LabelId);
            Assert.Equal(updatedVariousArtistsRelease.YearReleased, transformedRelease.YearReleased);
            Assert.Equal(updatedVariousArtistsRelease.ArtistId, transformedRelease.ArtistId);
            Assert.Equal(updatedVariousArtistsRelease.IsByVariousArtists, transformedRelease.IsByVariousArtists);
        }
    }
}
