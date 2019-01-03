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
using Tuneage.WebApi.Controllers.Api;
using Xunit;

namespace Tuneage.WebApi.Tests.Unit.Controllers.Api
{
    public class LabelsControllerTests : IDisposable
    {
        private readonly Mock<DbSet<Label>> _mockLabelSet;
        private readonly Mock<TuneageDataContext> _mockContext;
        private readonly LabelsController _controller;
        private readonly Label _existingLabel, _existingLabelUpdated, _newLabel;
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
        public async Task GetLabels_ShouldGetLabelsData()
        {
            // Arrange

            // Act
            var result = await _controller.GetLabels();
            var model = (List<Label>)result.Value;

            // Assert
            Assert.IsType<ActionResult<IEnumerable<Label>>>(result);
            Assert.Single(model);
            Assert.Equal(model[0], _existingLabel);
        }

        [Fact]
        public async Task GetLabel_ShouldGetExistingLabelData()
        {
            // Arrange

            // Act
            var result = await _controller.GetLabel(_existingLabel.LabelId);
            var model = result.Value;

            // Assert
            Assert.IsType<ActionResult<Label>>(result);
            Assert.Equal(model, _existingLabel);
        }

        [Fact]
        public async Task GetLabel_ShouldReturnActionResultWithNotFoundResultAttachedWhenCalledWithBadData()
        {
            // Arrange

            // Act
            var result = await _controller.GetLabel(NonExistentLabelId);

            // Assert
            Assert.IsType<ActionResult<Label>>(result);
            Assert.IsType<NotFoundResult>(result.Result);
            Assert.Null(result.Value);
            Assert.Equal((int)HttpStatusCode.NotFound, ((NotFoundResult)result.Result).StatusCode);
        }

        [Fact]
        public async Task PutLabel_ShouldCallContextToUpdateAndReturnNoContentResult()
        {
            // Arrange

            // Act
            var result = await _controller.PutLabel(_existingLabelUpdated.LabelId, _existingLabelUpdated);

            // Assert
            _mockContext.Verify(mc => mc.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PutLabel_ShouldReturnBadRequestResultWhenCalledWithNonMatchingIdData()
        {
            // Arrange

            // Act
            var result = await _controller.PutLabel(NonExistentLabelId, _existingLabelUpdated);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task PostLabel_ShouldCallContextToAddAndReturnNewLabel()
        {
            // Arrange

            // Act
            var result = await _controller.PostLabel(_newLabel);
            var createdAtActionResult = (CreatedAtActionResult)result.Result;
            var model = (Label)createdAtActionResult.Value;

            // Assert
            _mockLabelSet.Verify(mls => mls.Add(_newLabel), Times.Once);
            Assert.IsType<ActionResult<Label>>(result);
            Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(_newLabel, model);
        }

        [Fact]
        public async Task DeleteLabel_ShouldCallContextToDeleteAndReturnDeletedLabel()
        {
            // Arrange

            // Act
            var result = await _controller.DeleteLabel(_existingLabel.LabelId);
            var model = result.Value;

            // Assert
            _mockLabelSet.Verify(mls => mls.FindAsync(_existingLabel.LabelId), Times.Once);
            _mockLabelSet.Verify(mls => mls.Remove(_existingLabel), Times.Once);
            Assert.Equal(_existingLabel, model);
        }

        [Fact]
        public async Task DeleteLabel_ShouldReturnActionResultWithNotFoundResultAttachedWhenCalledWithBadData()
        {
            // Arrange

            // Act
            var result = await _controller.DeleteLabel(NonExistentLabelId);

            // Assert
            Assert.IsType<ActionResult<Label>>(result);
            Assert.IsType<NotFoundResult>(result.Result);
            Assert.Null(result.Value);
            Assert.Equal((int)HttpStatusCode.NotFound, ((NotFoundResult)result.Result).StatusCode);
        }



        public void Dispose()
        {
        }
    }
}
