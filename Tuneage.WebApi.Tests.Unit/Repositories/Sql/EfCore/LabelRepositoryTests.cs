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
    public class LabelRepositoryTests : IDisposable
    {
        private readonly LabelRepository _repository;

        public LabelRepositoryTests()
        {
            var mockLabelSet = new Mock<DbSet<Label>>();
            var mockContext = new Mock<TuneageDataContext>(new DbContextOptions<TuneageDataContext>());

            var existingLabel = TestDataGraph.LabelExisting;
            var existingLabelUpdated = TestDataGraph.LabelUpdated;
            var newLabel = TestDataGraph.LabelNew;
            var labels = TestDataGraph.LabelsRaw;
            var data = labels.AsQueryable();

            mockLabelSet.As<IAsyncEnumerable<Label>>().Setup(mls => mls.GetEnumerator()).Returns(
                new TestAsyncEnumerator<Label>(data.GetEnumerator())
            );
            mockLabelSet.As<IQueryable<Label>>().Setup(mls => mls.Provider).Returns(
                new TestAsyncQueryProvider<Label>(data.Provider)
            );
            mockLabelSet.As<IQueryable<Label>>().Setup(mls => mls.Expression).Returns(data.Expression);
            mockLabelSet.As<IQueryable<Label>>().Setup(mls => mls.ElementType).Returns(data.ElementType);
            mockLabelSet.As<IQueryable<Label>>().Setup(mls => mls.GetEnumerator()).Returns(() => data.GetEnumerator());
            mockLabelSet.Setup(mls => mls.FindAsync(existingLabel.LabelId)).Returns(Task.FromResult(existingLabel));
            mockLabelSet.Setup(mls => mls.AddAsync(newLabel, It.IsAny<CancellationToken>()))
                .Returns((Label model, CancellationToken token) => Task.FromResult((EntityEntry<Label>)null));
            mockLabelSet.Setup(mls => mls.Update(existingLabelUpdated)).Returns((EntityEntry<Label>)null);
            mockLabelSet.Setup(mls => mls.Find(existingLabel.LabelId)).Returns(existingLabel);

            mockContext.Setup(mc => mc.Labels).Returns(mockLabelSet.Object);
            mockContext.Setup(mc => mc.Set<Label>()).Returns(mockLabelSet.Object);

            _repository = new LabelRepository(mockContext.Object);
        }

        [Fact]
        public void GetAllAlphabetical_ShouldCallContextToGetAllAndReturnDataSortedAlphabeticallyByName()
        {
            // Arrange

            // Act
            var result = _repository.GetAllAlphabetical();
            var model = result.Result;

            // Assert
            Assert.IsType<Task<List<Label>>>(result);
            Assert.Equal(TestDataGraph.LabelsAlphabetizedByLabelName, model);
        }

        public void Dispose()
        {
        }
    }
}
