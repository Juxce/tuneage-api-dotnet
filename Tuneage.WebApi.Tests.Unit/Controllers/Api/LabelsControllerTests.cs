using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Tuneage.Data.Repositories.Sql.EfCore;
using Tuneage.Data.TestData;
using Tuneage.Domain.Entities;
using Tuneage.WebApi.Controllers.Api;
using Xunit;

namespace Tuneage.WebApi.Tests.Unit.Controllers.Api
{
    public class LabelsControllerTests : UnitTestFixture
    {
        private readonly Mock<LabelRepository> _mockRepository;
        private readonly LabelsController _controller;
        private readonly Label _existingLabel, _existingLabelUpdated, _newLabel;

        public LabelsControllerTests()
        {
            var mockLabelSet = new Mock<DbSet<Label>>();

            _existingLabel = TestDataGraph.Labels.ExistingLabel;
            _existingLabelUpdated = TestDataGraph.Labels.UpdatedLabel;
            _newLabel = TestDataGraph.Labels.NewLabel;
            var labels = TestDataGraph.Labels.LabelsRaw;
            var data = labels.AsQueryable();

            SetupMockDbSet(mockLabelSet, data);

            SetupMockSetOnMockContext(mockLabelSet);
            _mockRepository = new Mock<LabelRepository>(MockContext.Object);
            _mockRepository.Setup(mr => mr.GetAllAlphabetical()).Returns(Task.FromResult(TestDataGraph.Labels.LabelsAlphabetizedByLabelName));
            _mockRepository.Setup(mr => mr.GetById(_existingLabel.LabelId)).Returns(Task.FromResult(_existingLabel));

            _controller = new LabelsController(_mockRepository.Object);
        }

        [Fact]
        public async Task GetLabels_ShouldCallRepositoryToGetAllEntitiesAndReturnDataSortedAlphabeticallyByName()
        {
            // Arrange

            // Act
            var result = await _controller.GetLabels();
            var model = (List<Label>)result.Value;

            // Assert
            _mockRepository.Verify(mr => mr.GetAllAlphabetical(), Times.Once);
            Assert.IsType<ActionResult<IEnumerable<Label>>>(result);
            Assert.Equal(TestDataGraph.Labels.LabelsAlphabetizedByLabelName, model);
        }

        [Fact]
        public async Task GetLabel_ShouldCallRepositoryToGetDetailsForExistingEntityAndReturnData()
        {
            // Arrange

            // Act
            var result = await _controller.GetLabel(_existingLabel.LabelId);
            var model = result.Value;

            // Assert
            _mockRepository.Verify(mr => mr.GetById(_existingLabel.LabelId), Times.Once);
            Assert.IsType<ActionResult<Label>>(result);
            Assert.Equal(model, _existingLabel);
        }

        [Fact]
        public async Task GetLabel_ShouldReturnActionResultWithNotFoundResultAttachedWhenCalledWithBadId()
        {
            // Arrange

            // Act
            var result = await _controller.GetLabel(TestDataGraph.Labels.NonExistentLabelId);

            // Assert
            _mockRepository.Verify(mr => mr.GetById(TestDataGraph.Labels.NonExistentLabelId), Times.Once);
            Assert.IsType<ActionResult<Label>>(result);
            Assert.IsType<NotFoundResult>(result.Result);
            Assert.Null(result.Value);
            Assert.Equal((int)HttpStatusCode.NotFound, ((NotFoundResult)result.Result).StatusCode);
        }

        [Fact]
        public async Task PutLabel_ShouldCallRepositoryToUpdateEntityAndReturnNoContentResult()
        {
            // Arrange

            // Act
            var result = await _controller.PutLabel(_existingLabelUpdated.LabelId, _existingLabelUpdated);

            // Assert
            _mockRepository.Verify(mr => mr.SetModified(_existingLabelUpdated), Times.Once);
            _mockRepository.Verify(mr => mr.SaveChangesAsync(), Times.Once);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PutLabel_ShouldReturnBadRequestResultWhenCalledWithNonMatchingIdData()
        {
            // Arrange

            // Act
            var result = await _controller.PutLabel(TestDataGraph.Labels.NonExistentLabelId, _existingLabelUpdated);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task PostLabel_ShouldCallRepositoryToAddEntityAndReturnNewEntityData()
        {
            // Arrange

            // Act
            var result = await _controller.PostLabel(_newLabel); 
            var createdAtActionResult = (CreatedAtActionResult)result.Result;
            var model = (Label)createdAtActionResult.Value;

            // Assert
            _mockRepository.Verify(mr => mr.Create(_newLabel), Times.Once);
            Assert.IsType<ActionResult<Label>>(result);
            Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(_newLabel, model);
        }

        [Fact]
        public async Task DeleteLabel_ShouldCallRepositoryToFindAndRemoveExistingEntityAndReturnDeletedEntityData()
        {
            // Arrange

            // Act
            var result = await _controller.DeleteLabel(_existingLabel.LabelId);
            var model = result.Value;

            // Assert
            _mockRepository.Verify(mr => mr.GetById(_existingLabel.LabelId), Times.Once);
            _mockRepository.Verify(mr => mr.Delete(_existingLabel.LabelId), Times.Once);
            Assert.Equal(_existingLabel, model);
        }

        [Fact]
        public async Task DeleteLabel_ShouldCallRepositoryToGetEntityAndReturnActionResultWithNotFoundResultAttachedWhenCalledWithBadId()
        {
            // Arrange

            // Act
            var result = await _controller.DeleteLabel(TestDataGraph.Labels.NonExistentLabelId);

            // Assert
            _mockRepository.Verify(mr => mr.GetById(TestDataGraph.Labels.NonExistentLabelId), Times.Once);
            Assert.IsType<ActionResult<Label>>(result);
            Assert.IsType<NotFoundResult>(result.Result);
            Assert.Null(result.Value);
            Assert.Equal((int)HttpStatusCode.NotFound, ((NotFoundResult)result.Result).StatusCode);
        }
    }
}
