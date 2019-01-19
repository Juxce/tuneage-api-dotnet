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
using Tuneage.WebApi.Controllers.Mvc;
using Xunit;

namespace Tuneage.WebApi.Tests.Unit.Controllers.Mvc
{
    public class LabelsControllerTests : UnitTestFixture
    {
        private readonly Mock<LabelRepository> _mockRepository;
        private readonly LabelsController _controller;
        private readonly Label _existingLabel, _existingLabelUpdated, _newLabel;
        protected const string DefaultViewActionName = "Index";

        public LabelsControllerTests()
        {
            var mockLabelSet = new Mock<DbSet<Label>>();

            _existingLabel = TestDataGraph.Labels.LabelExisting;
            _existingLabelUpdated = TestDataGraph.Labels.LabelUpdated;
            _newLabel = TestDataGraph.Labels.LabelNew;
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
        public async Task IndexGet_ShouldCallRepositoryToGetAllEntitiesAndReturnViewWithDataSortedAlphabeticallyByName()
        {
            // Arrange

            // Act
            var result = await _controller.Index();
            var viewResult = (ViewResult)result;
            var model = (List<Label>)viewResult.Model;

            // Assert
            _mockRepository.Verify(mr => mr.GetAllAlphabetical(), Times.Once);
            Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
            Assert.Equal(model, TestDataGraph.Labels.LabelsAlphabetizedByLabelName);
        }

        [Fact]
        public async Task DetailsGet_ShouldCallRepositoryToGetDetailsForExistingEntityAndReturnViewWithData()
        {
            // Arrange

            // Act
            var result = await _controller.Details(_existingLabel.LabelId);
            var viewResult = (ViewResult)result;
            var model = (Label)viewResult.Model;

            // Assert
            _mockRepository.Verify(mr => mr.GetById(_existingLabel.LabelId), Times.Once);
            Assert.Null(viewResult.ViewName);
            Assert.Equal(_existingLabel, model);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(TestDataGraph.Labels.LabelIdNonExistent)]
        public async Task DetailsGet_ShouldReturnNotFoundResultWhenCalledWithBadData(int? value)
        {
            // Arrange

            // Act
            var result = await _controller.Details(value);

            // Assert
            if (value != null) _mockRepository.Verify(mr => mr.GetById(It.IsAny<int>()));
            Assert.IsType<NotFoundResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, ((NotFoundResult)result).StatusCode);
        }

        [Fact]
        public void CreateGet_ShouldReturnView()
        {
            // Arrange

            // Act
            var result = _controller.Create();
            var viewResult = (ViewResult)result;

            // Assert
            Assert.Null(viewResult.ViewName);
            Assert.NotNull(viewResult.ViewData);
            Assert.Null(viewResult.ViewData.Model);
        }

        [Fact]
        public async Task CreatePost_ShouldCallRepositoryToToAddEntityAndRedirectToIndex()
        {
            // Arrange

            //Act
            var result = await _controller.Create(_newLabel);
            var redirectToActionResult = (RedirectToActionResult)result;

            //Assert
            _mockRepository.Verify(mr => mr.Create(_newLabel), Times.Once);
            Assert.NotNull(redirectToActionResult);
            Assert.Equal(DefaultViewActionName, redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task CreatePost_ShouldOnlyReturnViewWithSameEntityWhenModelStateIsNotValid()
        {
            // Arrange
            _controller.ModelState.AddModelError("", "Error");

            // Act
            var result = await _controller.Create(_newLabel);
            var viewResult = (ViewResult)result;
            var model = (Label)viewResult.Model;

            // Assert
            Assert.Null(viewResult.ViewName);
            Assert.NotNull(viewResult.ViewData);
            Assert.Equal(_newLabel, model);
        }

        [Fact]
        public async Task EditGet_ShouldCallRepositoryToGetEntityAndReturnViewWithData()
        {
            // Arrange

            // Act
            var result = await _controller.Edit(_existingLabel.LabelId);
            var viewResult = (ViewResult)result;
            var model = (Label)viewResult.Model;

            // Assert
            _mockRepository.Verify(mr => mr.GetById(_existingLabel.LabelId), Times.Once);
            Assert.Null(viewResult.ViewName);
            Assert.NotNull(viewResult.ViewData);
            Assert.Equal(_existingLabel, model);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(TestDataGraph.Labels.LabelIdNonExistent)]
        public async Task EditGet_ShouldCallRepositoryToGetEntityAndReturnNotFoundResultWhenCalledWithBadData(int? value)
        {
            // Arrange

            // Act
            var result = await _controller.Edit(value);

            // Assert
            if (value != null) _mockRepository.Verify(mr => mr.GetById(It.IsAny<int>()));
            Assert.IsType<NotFoundResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, ((NotFoundResult)result).StatusCode);
        }

        [Fact]
        public async Task EditPost_ShouldCallRepositoryToUpdateEntityAndRedirectToIndex()
        {
            // Arrange

            //Act
            var result = await _controller.Edit(_existingLabelUpdated.LabelId, _existingLabelUpdated);
            var redirectToActionResult = (RedirectToActionResult)result;

            //Assert
            _mockRepository.Verify(mr => mr.Update(_existingLabelUpdated.LabelId, _existingLabelUpdated), Times.Once);
            Assert.NotNull(redirectToActionResult);
            Assert.Equal(DefaultViewActionName, redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task EditPost_ShouldReturnNotFoundResultWhenCalledWithNonMatchingIdData()
        {
            // Arrange

            // Act
            var result = await _controller.Edit(TestDataGraph.Labels.LabelIdNonExistent, _existingLabelUpdated);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, ((NotFoundResult)result).StatusCode);
        }

        [Fact]
        public async Task EditPost_ShouldOnlyReturnViewWithSameEntityWhenModelStateIsNotValid()
        {
            // Arrange
            _controller.ModelState.AddModelError("", "Error");

            // Act
            var result = await _controller.Edit(_existingLabelUpdated.LabelId, _existingLabelUpdated);
            var viewResult = (ViewResult)result;
            var model = (Label)viewResult.Model;

            // Assert
            Assert.Null(viewResult.ViewName);
            Assert.NotNull(viewResult.ViewData);
            Assert.Equal(_existingLabelUpdated, model);
        }

        [Fact]
        public async Task DeleteGet_ShouldCallRepositoryToGetEntityAndReturnViewWithData()
        {
            // Arrange

            // Act
            var result = await _controller.Delete(_existingLabel.LabelId);
            var viewResult = (ViewResult)result;
            var model = (Label)viewResult.Model;

            // Assert
            _mockRepository.Verify(mr => mr.GetById(_existingLabel.LabelId), Times.Once);
            Assert.Null(viewResult.ViewName);
            Assert.NotNull(viewResult.ViewData);
            Assert.Equal(_existingLabel, model);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(TestDataGraph.Labels.LabelIdNonExistent)]
        public async Task DeleteGet_ShouldReturnNotFoundResultWhenCalledWithBadData(int? value)
        {
            // Arrange

            // Act
            var result = await _controller.Delete(value);

            // Assert
            if (value != null) _mockRepository.Verify(mr => mr.GetById(It.IsAny<int>()));
            Assert.IsType<NotFoundResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, ((NotFoundResult)result).StatusCode);
        }

        [Fact]
        public async Task DeleteConfirmedPost_ShouldCallRepositoryToFindAndRemoveExistingEntity()
        {
            // Arrange

            // Act
            var result = await _controller.DeleteConfirmed(_existingLabel.LabelId);
            var redirectToActionResult = (RedirectToActionResult)result;

            // Assert
            _mockRepository.Verify(mr => mr.GetById(_existingLabel.LabelId), Times.Once);
            _mockRepository.Verify(mr => mr.Delete(_existingLabel.LabelId), Times.Once);
            Assert.NotNull(redirectToActionResult);
            Assert.Equal(DefaultViewActionName, redirectToActionResult.ActionName);
        }
    }
}
