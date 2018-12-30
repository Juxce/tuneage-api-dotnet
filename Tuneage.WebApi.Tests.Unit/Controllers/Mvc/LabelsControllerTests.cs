using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private Mock<DbSet<Label>> _mockLabelSet;
        private Mock<TuneageDataContext> _mockContext;
        private LabelsController _controller;

        public LabelsControllerTests()
        {
            _mockLabelSet = new Mock<DbSet<Label>>();
            _mockContext = new Mock<TuneageDataContext>(new DbContextOptions<TuneageDataContext>());
        }

        [Fact]
        public async Task DetailsGet_ShouldGetDetailsForExistingAlbum()
        {
            // Arrange
            var existingLabel = new Label() { LabelId = 9, Name = "Fat Wreck Chords", WebsiteUrl = "www.fatwreck.com" };
            var data = new List<Label> { existingLabel }.AsQueryable();
            _mockLabelSet.As<IAsyncEnumerable<Label>>().Setup(mls => mls.GetEnumerator()).Returns(
                    new TestAsyncEnumerator<Label>(data.GetEnumerator())
                );
            _mockLabelSet.As<IQueryable<Label>>().Setup(mls => mls.Provider).Returns(
                    new TestAsyncQueryProvider<Label>(data.Provider)
                );
            _mockLabelSet.As<IQueryable<Label>>().Setup(mls => mls.Expression).Returns(data.Expression);
            _mockLabelSet.As<IQueryable<Label>>().Setup(mls => mls.ElementType).Returns(data.ElementType);
            _mockLabelSet.As<IQueryable<Label>>().Setup(mls => mls.GetEnumerator()).Returns(() => data.GetEnumerator());
            _mockContext.Setup(mc => mc.Labels).Returns(_mockLabelSet.Object);
            _controller = new LabelsController(_mockContext.Object);

            // Act
            var result = await _controller.Details(existingLabel.LabelId);
            var viewResult = (ViewResult)result;
            var model = (Label)viewResult.Model;

            // Assert
            //_mockContext.Verify(c => c.Labels.FirstOrDefaultAsync(9), times: Times.Once);  // HOW TO verify this now? Is it even necessary?
            Assert.Null(viewResult.ViewName);
            Assert.Equal(existingLabel, model);
        }






        public void Dispose()
        {
        }
    }
}
