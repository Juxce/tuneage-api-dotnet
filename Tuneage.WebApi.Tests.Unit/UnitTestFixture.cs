using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Moq;
using Tuneage.Data.Orm.EF.DataContexts;

namespace Tuneage.WebApi.Tests.Unit
{
    public class UnitTestFixture : IDisposable
    {
        protected readonly Mock<TuneageDataContext> MockContext;

        public UnitTestFixture()
        {
            MockContext = new Mock<TuneageDataContext>(new DbContextOptions<TuneageDataContext>());
        }

        protected void SetupMockDbSet<T>(Mock<DbSet<T>> mockSet, IQueryable<T> data) where T : class
        {
            mockSet.As<IAsyncEnumerable<T>>().Setup(mls => mls.GetEnumerator()).Returns(
                new TestAsyncEnumerator<T>(data.GetEnumerator())
            );
            mockSet.As<IQueryable<T>>().Setup(mls => mls.Provider).Returns(
                new TestAsyncQueryProvider<T>(data.Provider)
            );
            mockSet.As<IQueryable<T>>().Setup(mls => mls.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<T>>().Setup(mls => mls.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<T>>().Setup(mls => mls.GetEnumerator()).Returns(data.GetEnumerator);
        }

        protected void SetupMockSetOnMockContext<T>(Mock<DbSet<T>> mockSet) where T : class
        {
            MockContext.Setup(mc => mc.Set<T>()).Returns(mockSet.Object);
        }

        public void Dispose()
        {
        }
    }
}
