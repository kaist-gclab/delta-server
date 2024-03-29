using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Xunit;
using Xunit.Abstractions;

namespace Delta.AppServer.Test.Infrastructure;

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
#pragma warning disable EF1001
        var nameA = a.GetExtension<InMemoryOptionsExtension>().StoreName;
        var nameB = b.GetExtension<InMemoryOptionsExtension>().StoreName;
#pragma warning restore EF1001
        Assert.NotEqual(nameA, nameB);

        var root = new InMemoryDatabaseRoot();
        var c = new TestDeltaDbContextOptionsBuilder(_testOutputHelper, "database-name", root).Build();
        var d = new TestDeltaDbContextOptionsBuilder(_testOutputHelper, "database-name", root).Build();
#pragma warning disable EF1001
        var nameC = c.GetExtension<InMemoryOptionsExtension>().StoreName;
        var nameD = d.GetExtension<InMemoryOptionsExtension>().StoreName;
#pragma warning restore EF1001
        Assert.Equal(nameC, nameD);
    }
}