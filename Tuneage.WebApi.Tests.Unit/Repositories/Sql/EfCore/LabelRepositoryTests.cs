using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using Tuneage.Data.Repositories.Sql.EfCore;
using Tuneage.Data.TestData;
using Tuneage.Domain.Entities;
using Xunit;

namespace Tuneage.WebApi.Tests.Unit.Repositories.Sql.EfCore
{
    public class LabelRepositoryTests : UnitTestFixture
    {
        private readonly LabelRepository _repository;

        public LabelRepositoryTests()
        {
            var mockLabelSet = new Mock<DbSet<Label>>();

            var existingLabel = TestDataGraph.Labels.LabelExisting;
            var existingLabelUpdated = TestDataGraph.Labels.LabelUpdated;
            var newLabel = TestDataGraph.Labels.LabelNew;
            var labels = TestDataGraph.Labels.LabelsRaw;
            var data = labels.AsQueryable();

            SetupMockDbSet(mockLabelSet, data);
            mockLabelSet.Setup(mls => mls.FindAsync(existingLabel.LabelId)).Returns(Task.FromResult(existingLabel));
            mockLabelSet.Setup(mls => mls.AddAsync(newLabel, It.IsAny<CancellationToken>()))
                .Returns((Label model, CancellationToken token) => Task.FromResult((EntityEntry<Label>)null));
            mockLabelSet.Setup(mls => mls.Update(existingLabelUpdated)).Returns((EntityEntry<Label>)null);
            mockLabelSet.Setup(mls => mls.Find(existingLabel.LabelId)).Returns(existingLabel);

            SetupMockSetOnMockContext(mockLabelSet);

            _repository = new LabelRepository(MockContext.Object);
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
            Assert.Equal(TestDataGraph.Labels.LabelsAlphabetizedByLabelName, model);
        }
    }
}
