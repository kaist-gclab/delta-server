using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Xunit;
using Xunit.Abstractions;

namespace Delta.AppServer.Test.Infrastructure
{
    public class TestDeltaDbContextOptionsBuilderTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public TestDeltaDbContextOptionsBuilderTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void ConfigureDbContextOptionsBuilder()
        {
            var builder = new DbContextOptionsBuilder<DeltaContext>();
            new TestDeltaDbContextOptionsBuilder(_testOutputHelper).ConfigureDbContextOptionsBuilder(builder);
            Assert.True(builder.IsConfigured);
        }

        [Fact]
        public void Build()
        {
            var a = new TestDeltaDbContextOptionsBuilder(_testOutputHelper).Build();
            var b = new TestDeltaDbContextOptionsBuilder(_testOutputHelper).Build();
            var nameA = a.GetExtension<InMemoryOptionsExtension>().StoreName;
            var nameB = b.GetExtension<InMemoryOptionsExtension>().StoreName;
            Assert.NotEqual(nameA, nameB);

            var root = new InMemoryDatabaseRoot();
            var c = new TestDeltaDbContextOptionsBuilder(_testOutputHelper, "database-name", root).Build();
            var d = new TestDeltaDbContextOptionsBuilder(_testOutputHelper, "database-name", root).Build();
            var nameC = c.GetExtension<InMemoryOptionsExtension>().StoreName;
            var nameD = d.GetExtension<InMemoryOptionsExtension>().StoreName;
            Assert.Equal(nameC, nameD);
        }
    }
}