using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

            var labels = TestDataGraph.Labels.LabelsRaw;
            var data = labels.AsQueryable();

            SetupMockDbSet(mockLabelSet, data);

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
