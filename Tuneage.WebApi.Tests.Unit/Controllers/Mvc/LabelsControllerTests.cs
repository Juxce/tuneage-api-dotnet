using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Tuneage.Data.Orm.EF.DataContexts;
using Tuneage.Domain.Entities;
using Tuneage.WebApi.Controllers.Mvc;
using Xunit;

namespace Tuneage.WebApi.Tests.Unit.Controllers.Mvc
{
    public class LabelsControllerTests : IDisposable
    {
        private readonly Mock<DbSet<Label>> _mockLabelSet;
        private readonly Mock<TuneageDataContext> _mockContext;
        private readonly LabelsController _controller;
        private readonly Label _existingLabel, _existingLabelUpdated, _newLabel;
        protected const string DefaultViewActionName = "Index";
        private const int NonExistentLabelId = 99;

        public LabelsControllerTests()
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

            _mockContext.Setup(mc => mc.Labels).Returns(_mockLabelSet.Object);
            _controller = new LabelsController(_mockContext.Object);
        }

        [Fact]
        public async Task IndexGet_ShouldGetViewWithLabelsData()
        {
            // Arrange

            // Act
            var result = await _controller.Index();
            var viewResult = (ViewResult)result;
            var model = (List<Label>)viewResult.Model;

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
            Assert.Single(model);
            Assert.Equal(model[0], _existingLabel);
        }

        [Fact]
        public async Task DetailsGet_ShouldGetDetailsForExistingLabel()
        {
            // Arrange

            // Act
            var result = await _controller.Details(_existingLabel.LabelId);
            var viewResult = (ViewResult)result;
            var model = (Label)viewResult.Model;

            // Assert
            //_mockLabelSet.Verify(mls => mls.FirstOrDefaultAsync(It.IsAny<CancellationToken>()), Times.Once);  // HOW TO verify this extension method? Need moles?
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
        public async Task CreatePost_ShouldCallContextToAddAndRedirectToIndex()
        {
            // Arrange

            //Act
            var result = await _controller.Create(_newLabel);
            var redirectToActionResult = (RedirectToActionResult)result;

            //Assert
            _mockContext.Verify(mc => mc.Add(_newLabel), Times.Once);
            _mockContext.Verify(mc => mc.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            Assert.NotNull(redirectToActionResult);
            Assert.Equal(DefaultViewActionName, redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task CreatePost_ShouldOnlyReturnViewWithSameLabelWhenModelStateIsNotValid()
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
        public async Task EditGet_ShouldReturnExistingLabel()
        {
            // Arrange

            // Act
            var result = await _controller.Edit(_existingLabel.LabelId);
            var viewResult = (ViewResult)result;
            var model = (Label)viewResult.Model;

            // Assert
            _mockLabelSet.Verify(mls => mls.FindAsync(_existingLabel.LabelId), Times.Once);
            Assert.Null(viewResult.ViewName);
            Assert.NotNull(viewResult.ViewData);
            Assert.Equal(_existingLabel, model);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(NonExistentLabelId)]
        public async Task EditGet_ShouldReturnNotFoundResultWhenCalledWithBadData(int? value)
        {
            // Arrange

            // Act
            var result = await _controller.Edit(value);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, ((NotFoundResult)result).StatusCode);
        }

        [Fact]
        public async Task EditPost_ShouldCallContextToUpdateAndRedirectToIndex()
        {
            // Arrange

            //Act
            var result = await _controller.Edit(_existingLabelUpdated.LabelId, _existingLabelUpdated);
            var redirectToActionResult = (RedirectToActionResult)result;

            //Assert
            _mockContext.Verify(mc => mc.Update(_existingLabelUpdated), Times.Once);
            _mockContext.Verify(mc => mc.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            Assert.NotNull(redirectToActionResult);
            Assert.Equal(DefaultViewActionName, redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task EditPost_ShouldOnlyReturnViewWithSameLabelWhenModelStateIsNotValid()
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
        public async Task DeleteGet_ShouldReturnExistingLabel()
        {
            // Arrange

            // Act
            var result = await _controller.Delete(_existingLabel.LabelId);
            var viewResult = (ViewResult)result;
            var model = (Label)viewResult.Model;

            // Assert
            //_mockLabelSet.Verify(mls => mls.FirstOrDefaultAsync(It.IsAny<CancellationToken>()), Times.Once);  // HOW TO verify this extension method? Need moles?
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
            Assert.IsType<NotFoundResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, ((NotFoundResult)result).StatusCode);
        }

        [Fact]
        public async Task DeleteConfirmedPost_ShouldCallContextToFindRemoveAndRemoveExistingLabelAndThenSave()
        {
            // Arrange

            //Act
            var result = await _controller.DeleteConfirmed(_existingLabel.LabelId);
            var redirectToActionResult = (RedirectToActionResult)result;

            //Assert
            _mockLabelSet.Verify(mls => mls.FindAsync(_existingLabel.LabelId), Times.Once);
            _mockLabelSet.Verify(mls => mls.Remove(_existingLabel), Times.Once);
            _mockContext.Verify(mc => mc.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            Assert.NotNull(redirectToActionResult);
            Assert.Equal(DefaultViewActionName, redirectToActionResult.ActionName);
        }



        public void Dispose()
        {
        }
    }
}
