using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using Tuneage.Data.Orm.EF.DataContexts;
using Tuneage.Data.Repositories.Sql.EfCore;
using Tuneage.Data.TestData;
using Tuneage.Domain.Entities;
using Xunit;

namespace Tuneage.WebApi.Tests.Unit.Repositories.Sql.EfCore
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

            _existingLabel = TestDataGraph.Labels.LabelExisting;
            _existingLabelUpdated = TestDataGraph.Labels.LabelUpdated;
            _newLabel = TestDataGraph.Labels.LabelNew;
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
            _mockLabelSet.Setup(mls => mls.AddAsync(_newLabel, It.IsAny<CancellationToken>()))
                .Returns((Label model, CancellationToken token) => Task.FromResult((EntityEntry<Label>)null));
            _mockLabelSet.Setup(mls => mls.Update(_existingLabelUpdated)).Returns((EntityEntry<Label>)null);
            _mockLabelSet.Setup(mls => mls.Find(_existingLabel.LabelId)).Returns(_existingLabel);

            _mockContext.Setup(mc => mc.Labels).Returns(_mockLabelSet.Object);
            _mockContext.Setup(mc => mc.Set<Label>()).Returns(_mockLabelSet.Object);

            _repository = new EfCoreMsSqlRepository<Label>(_mockContext.Object);
        }

        [Fact]
        public void GetAll_ShouldCallContextToGetAllAndReturnListOfEntityType()
        {
            // Arrange

            // Act
            var result = _repository.GetAll();

            // Assert
            Assert.IsAssignableFrom<IQueryable>(result);
            Assert.Single(result);
            Assert.Equal(result.First(), _existingLabel);
        }

        [Fact]
        public async void GetById_ShouldCallContextToFindEntityAndReturnItWhenItExists()
        {
            // Arrange

            // Act
            var result = await _repository.GetById(_existingLabel.LabelId);

            // Assert
            _mockLabelSet.Verify(mls => mls.FindAsync(_existingLabel.LabelId), Times.Once);
            Assert.IsType<Label>(result);
            Assert.Equal(result, _existingLabel);
        }

        [Fact]
        public async void GetById_ShouldCallContextToFindEntityAndReturnNullWhenItDoesNotExist()
        {
            // Arrange

            // Act
            var result = await _repository.GetById(NonExistentLabelId);

            // Assert
            _mockLabelSet.Verify(mls => mls.FindAsync(NonExistentLabelId), Times.Once);
            Assert.Null(result);
        }

        [Fact]
        public async void Create_ShouldCallContextToAddAndSave()
        {
            // Arrange

            // Act
            await _repository.Create(_newLabel);

            // Assert
            _mockLabelSet.Verify(mls => mls.AddAsync(_newLabel, It.IsAny<CancellationToken>()), Times.Once);
            _mockContext.Verify(mc => mc.SaveChangesAsync(It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async void Update_ShouldCallContextToUpdateAndSave()
        {
            // Arrange

            // Act
            await _repository.Update(_existingLabelUpdated.LabelId, _existingLabelUpdated);

            // Assert
            _mockLabelSet.Verify(mls => mls.Update(_existingLabelUpdated), Times.Once);
            _mockContext.Verify(mc => mc.SaveChangesAsync(It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async void Delete_ShouldCallContextToRemoveAndSave()
        {
            // Arrange

            // Act
            await _repository.Delete(_existingLabel.LabelId);

            // Assert
            _mockLabelSet.Verify(mls => mls.Remove(_existingLabel), Times.Once);
            _mockContext.Verify(mc => mc.SaveChangesAsync(It.IsAny<CancellationToken>()));
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
        public async void SaveChangesAsync_ShouldCallContextToSaveChangesAsynchronously()
        {
            // Arrange

            // Act
            await _repository.SaveChangesAsync();

            // Assert
            _mockContext.Verify(mc => mc.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void Any_ShouldReturnTrueForExistingEntityId()
        {
            // Arrange

            // Act
            var result = _repository.Any(_existingLabel.LabelId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Any_ShouldReturnFalseForNonExistentEntityId()
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
