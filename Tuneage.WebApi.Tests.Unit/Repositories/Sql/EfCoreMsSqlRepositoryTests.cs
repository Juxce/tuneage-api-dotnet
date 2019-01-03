using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using Tuneage.Data.Orm.EF.DataContexts;
using Tuneage.Data.Repositories.Sql;
using Tuneage.Domain.Entities;
using Xunit;

namespace Tuneage.WebApi.Tests.Unit.Repositories.Sql
{
    public class EfCoreMsSqlRepositoryTests : IDisposable
    {
        private readonly Mock<DbSet<Label>> _mockLabelSet;
        private readonly Mock<TuneageDataContext> _mockContext;
        private readonly EfCoreMsSqlRepository<Label> _repository;
        private readonly Label _existingLabel, _existingLabelUpdated, _newLabel;
        private const int NonExistentLabelId = 99;

        public EfCoreMsSqlRepositoryTests()
        {
            _mockLabelSet = new Mock<DbSet<Label>>();
            _mockContext = new Mock<TuneageDataContext>(new DbContextOptions<TuneageDataContext>());

            _existingLabel = new Label() { LabelId = 9, Name = "Fat Wreck Chords", WebsiteUrl = "www.fatwreck.com" };
            _existingLabelUpdated = new Label() { LabelId = 9, Name = "Fat Wreck PARTY", WebsiteUrl = "www.fatwreck.com" };
            _newLabel = new Label() { LabelId = 10, Name = "Learning Curve", WebsiteUrl = "http://learningcurverecords.com/" };
            var labels = new List<Label> { _existingLabel };
            var data = labels.AsQueryable();

            _mockLabelSet.As<IAsyncEnumerable<Label>>().Setup(mls => mls.GetEnumerator()).Returns(
                new TestAsyncEnumerator<Label>(data.GetEnumerator())
            );
            _mockLabelSet.As<IQueryable<Label>>().Setup(mls => mls.Provider).Returns(
                new TestAsyncQueryProvider<Label>(data.Provider)
            );
            _mockLabelSet.As<IQueryable<Label>>().Setup(mls => mls.Expression).Returns(data.Expression);
            _mockLabelSet.As<IQueryable<Label>>().Setup(mls => mls.ElementType).Returns(data.ElementType);
            _mockLabelSet.As<IQueryable<Label>>().Setup(mls => mls.GetEnumerator()).Returns(() => data.GetEnumerator());
            _mockLabelSet.Setup(mls => mls.FindAsync(_existingLabel.LabelId)).Returns(Task.FromResult(_existingLabel));
            _mockLabelSet.Setup(mls => mls.Find(_existingLabel.LabelId)).Returns(_existingLabel);
            _mockLabelSet.Setup(mls => mls.Find(NonExistentLabelId)).Returns((Label) null);

            _mockContext.Setup(mc => mc.Labels).Returns(_mockLabelSet.Object);
            _mockContext.Setup(mc => mc.Set<Label>()).Returns(_mockLabelSet.Object);
            _repository = new EfCoreMsSqlRepository<Label>(_mockContext.Object);
        }

        [Fact]
        public async Task GetAll_ShouldCallContextToGetAllAndReturnListOfEntityType()
        {
            // Arrange

            // Act
            var result = await _repository.GetAll();

            // Assert
            Assert.IsType<List<Label>>(result);
            Assert.Single(result);
            Assert.Equal(result[0], _existingLabel);
        }

        [Fact]
        public async Task Get_ShouldCallContextToFindEntityAndReturnItWhenItExists()
        {
            // Arrange

            // Act
            var result = await _repository.Get(_existingLabel.LabelId);

            // Assert
            _mockLabelSet.Verify(mls => mls.FindAsync(_existingLabel.LabelId), Times.Once);
            Assert.IsType<Label>(result);
            Assert.Equal(result, _existingLabel);
        }

        [Fact]
        public void SetModified_ShouldCallSetModifiedOnContext()
        {
            // Arrange

            // Act
            _repository.SetModified(_existingLabelUpdated);

            // Assert
            _mockContext.Verify(mc => mc.SetModified(_existingLabelUpdated), Times.Once);
        }

        [Fact]
        public void Update_ShouldCallContextToUpdate()
        {
            // Arrange

            // Act
            _repository.Update(_existingLabelUpdated);

            // Assert
            _mockContext.Verify(mc => mc.Update(_existingLabelUpdated), Times.Once);
        }

        [Fact]
        public void Add_ShouldCallContextToAdd()
        {
            // Arrange

            // Act
            _repository.Add(_newLabel);

            // Assert
            _mockLabelSet.Verify(mls => mls.Add(_newLabel), Times.Once);
        }

        [Fact]
        public void Remove_ShouldCallContextToRemove()
        {
            // Arrange

            // Act
            _repository.Remove(_existingLabel);

            // Assert
            _mockLabelSet.Verify(mls => mls.Remove(_existingLabel), Times.Once);
        }

        [Fact]
        public void SaveChangesAsync_ShouldCallContextToSaveChangesAsynchronously()
        {
            // Arrange

            // Act
            _repository.SaveChangesAsync();

            // Assert
            _mockContext.Verify(mc => mc.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void Any_ShouldReturnTrueForExistingEntity()
        {
            // Arrange

            // Act
            var result = _repository.Any(_existingLabel.LabelId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Any_ShouldReturnFalseForNonExistentEntity()
        {
            // Arrange

            // Act
            var result = _repository.Any(NonExistentLabelId);

            // Assert
            Assert.False(result);
        }

        public void Dispose()
        {
        }
    }
}
