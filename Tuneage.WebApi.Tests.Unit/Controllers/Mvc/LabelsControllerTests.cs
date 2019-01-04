using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Tuneage.Data.Orm.EF.DataContexts;
using Tuneage.Data.Repositories.Sql;
using Tuneage.Domain.Entities;
using Tuneage.WebApi.Controllers.Mvc;
using Xunit;

namespace Tuneage.WebApi.Tests.Unit.Controllers.Mvc
{
    public class LabelsControllerTests : IDisposable
    {
        private readonly Mock<EfCoreMsSqlRepository<Label>> _mockRepository;
        private readonly LabelsController _controller;
        private readonly Label _existingLabel, _existingLabelUpdated, _newLabel;
        protected const string DefaultViewActionName = "Index";
        private const int NonExistentLabelId = 99;

        public LabelsControllerTests()
        {
            var mockLabelSet = new Mock<DbSet<Label>>();
            var mockContext = new Mock<TuneageDataContext>(new DbContextOptions<TuneageDataContext>());

            _existingLabel = new Label() { LabelId = 9, Name = "Fat Wreck Chords", WebsiteUrl = "www.fatwreck.com" };
            _existingLabelUpdated = new Label() { LabelId = 9, Name = "Fat Wreck PARTY", WebsiteUrl = "www.fatwreck.com" };
            _newLabel = new Label() { LabelId = 10, Name = "Learning Curve", WebsiteUrl = "http://learningcurverecords.com/" };
            var labels = new List<Label> { _existingLabel };
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
            mockLabelSet.Setup(mls => mls.FindAsync(_existingLabel.LabelId)).Returns(Task.FromResult(_existingLabel));
            mockContext.Setup(mc => mc.Labels).Returns(mockLabelSet.Object);

            _mockRepository = new Mock<EfCoreMsSqlRepository<Label>>(mockContext.Object);
            _mockRepository.Setup(mr => mr.GetAll()).Returns(Task.FromResult(labels));
            _mockRepository.Setup(mr => mr.Get(_existingLabel.LabelId)).Returns(Task.FromResult(_existingLabel));

            _controller = new LabelsController(_mockRepository.Object);
        }

        [Fact]
        public async Task IndexGet_ShouldCallRepositoryToGetAllEntitiesAndReturnViewWithData()
        {
            // Arrange

            // Act
            var result = await _controller.Index();
            var viewResult = (ViewResult)result;
            var model = (List<Label>)viewResult.Model;

            // Assert
            _mockRepository.Verify(mr => mr.GetAll(), Times.Once);
            Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
            Assert.Single(model);
            Assert.Equal(model[0], _existingLabel);
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
            _mockRepository.Verify(mr => mr.Get(_existingLabel.LabelId), Times.Once);
            Assert.Null(viewResult.ViewName);
            Assert.Equal(_existingLabel, model);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(NonExistentLabelId)]
        public async Task DetailsGet_ShouldReturnNotFoundResultWhenCalledWithBadData(int? value)
        {
            // Arrange

            // Act
            var result = await _controller.Details(value);

            // Assert
            if (value != null) _mockRepository.Verify(mr => mr.Get(It.IsAny<int>()));
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
            _mockRepository.Verify(mr => mr.Add(_newLabel), Times.Once);
            _mockRepository.Verify(mr => mr.SaveChangesAsync(), Times.Once);
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
            _mockRepository.Verify(mr => mr.Get(_existingLabel.LabelId), Times.Once);
            Assert.Null(viewResult.ViewName);
            Assert.NotNull(viewResult.ViewData);
            Assert.Equal(_existingLabel, model);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(NonExistentLabelId)]
        public async Task EditGet_ShouldCallRepositoryToGetEntityAndReturnNotFoundResultWhenCalledWithBadData(int? value)
        {
            // Arrange

            // Act
            var result = await _controller.Edit(value);

            // Assert
            if (value != null) _mockRepository.Verify(mr => mr.Get(It.IsAny<int>()));
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
            _mockRepository.Verify(mr => mr.Update(_existingLabelUpdated), Times.Once);
            _mockRepository.Verify(mr => mr.SaveChangesAsync(), Times.Once);
            Assert.NotNull(redirectToActionResult);
            Assert.Equal(DefaultViewActionName, redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task EditPost_ShouldReturnNotFoundResultWhenCalledWithNonMatchingIdData()
        {
            // Arrange
            
            // Act
            var result = await _controller.Edit(NonExistentLabelId, _existingLabelUpdated);

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
            _mockRepository.Verify(mr => mr.Get(_existingLabel.LabelId), Times.Once);
            Assert.Null(viewResult.ViewName);
            Assert.NotNull(viewResult.ViewData);
            Assert.Equal(_existingLabel, model);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(NonExistentLabelId)]
        public async Task DeleteGet_ShouldReturnNotFoundResultWhenCalledWithBadData(int? value)
        {
            // Arrange

            // Act
            var result = await _controller.Delete(value);

            // Assert
            if (value != null) _mockRepository.Verify(mr => mr.Get(It.IsAny<int>()));
            Assert.IsType<NotFoundResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, ((NotFoundResult)result).StatusCode);
        }

        [Fact]
        public async Task DeleteConfirmedPost_ShouldCallRepositoryToFindAndRemoveExistingEntityAndThenSave()
        {
            // Arrange

            // Act
            var result = await _controller.DeleteConfirmed(_existingLabel.LabelId);
            var redirectToActionResult = (RedirectToActionResult)result;

            // Assert
            _mockRepository.Verify(mr => mr.Get(_existingLabel.LabelId), Times.Once);
            _mockRepository.Verify(mr => mr.Remove(_existingLabel), Times.Once);
            _mockRepository.Verify(mr => mr.SaveChangesAsync(), Times.Once);
            Assert.NotNull(redirectToActionResult);
            Assert.Equal(DefaultViewActionName, redirectToActionResult.ActionName);
        }



        public void Dispose()
        {
        }
    }
}
