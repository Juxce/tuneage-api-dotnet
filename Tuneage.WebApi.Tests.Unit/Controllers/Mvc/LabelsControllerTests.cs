using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Tuneage.Data.Orm.EF.DataContexts;
using Tuneage.Data.Repositories.Sql.EfCore;
using Tuneage.Data.TestData;
using Tuneage.Domain.Entities;
using Tuneage.WebApi.Controllers.Mvc;
using Xunit;

namespace Tuneage.WebApi.Tests.Unit.Controllers.Mvc
{
    public class LabelsControllerTests : IDisposable
    {
        private readonly Mock<LabelRepository> _mockRepository;
        private readonly LabelsController _controller;
        private readonly Label _existingLabel, _existingLabelUpdated, _newLabel;
        protected const string DefaultViewActionName = "Index";

        public LabelsControllerTests()
        {
            var mockLabelSet = new Mock<DbSet<Label>>();
            var mockContext = new Mock<TuneageDataContext>(new DbContextOptions<TuneageDataContext>());

            _existingLabel = TestDataGraph.LabelExisting;
            _existingLabelUpdated = TestDataGraph.LabelUpdated;
            _newLabel = TestDataGraph.LabelNew;
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
            mockContext.Setup(mc => mc.Labels).Returns(mockLabelSet.Object);

            _mockRepository = new Mock<LabelRepository>(mockContext.Object);
            _mockRepository.Setup(mr => mr.GetAllAlphabetical()).Returns(Task.FromResult(TestDataGraph.LabelsAlphabetizedByLabelName));
            _mockRepository.Setup(mr => mr.GetById(_existingLabel.LabelId)).Returns(Task.FromResult(_existingLabel));

            _controller = new LabelsController(_mockRepository.Object);
        }

        [Fact]
        public async void IndexGet_ShouldCallRepositoryToGetAllEntitiesAndReturnViewWithDataSortedAlphabeticallyByName()
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
            Assert.Equal(model, TestDataGraph.LabelsAlphabetizedByLabelName);
        }

        [Fact]
        public async void DetailsGet_ShouldCallRepositoryToGetDetailsForExistingEntityAndReturnViewWithData()
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
        [InlineData(TestDataGraph.LabelIdNonExistent)]
        public async void DetailsGet_ShouldReturnNotFoundResultWhenCalledWithBadData(int? value)
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
        public async void CreatePost_ShouldCallRepositoryToToAddEntityAndRedirectToIndex()
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
        public async void CreatePost_ShouldOnlyReturnViewWithSameEntityWhenModelStateIsNotValid()
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
        public async void EditGet_ShouldCallRepositoryToGetEntityAndReturnViewWithData()
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
        [InlineData(TestDataGraph.LabelIdNonExistent)]
        public async void EditGet_ShouldCallRepositoryToGetEntityAndReturnNotFoundResultWhenCalledWithBadData(int? value)
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
        public async void EditPost_ShouldCallRepositoryToUpdateEntityAndRedirectToIndex()
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
        public async void EditPost_ShouldReturnNotFoundResultWhenCalledWithNonMatchingIdData()
        {
            // Arrange

            // Act
            var result = await _controller.Edit(TestDataGraph.LabelIdNonExistent, _existingLabelUpdated);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, ((NotFoundResult)result).StatusCode);
        }

        [Fact]
        public async void EditPost_ShouldOnlyReturnViewWithSameEntityWhenModelStateIsNotValid()
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
        public async void DeleteGet_ShouldCallRepositoryToGetEntityAndReturnViewWithData()
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
        [InlineData(TestDataGraph.LabelIdNonExistent)]
        public async void DeleteGet_ShouldReturnNotFoundResultWhenCalledWithBadData(int? value)
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
        public async void DeleteConfirmedPost_ShouldCallRepositoryToFindAndRemoveExistingEntity()
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



        public void Dispose()
        {
        }
    }
}
