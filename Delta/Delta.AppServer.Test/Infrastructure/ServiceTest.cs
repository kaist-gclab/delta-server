using Xunit.Abstractions;

namespace Delta.AppServer.Test.Infrastructure;

public abstract class ServiceTest
{
    protected ITestOutputHelper Output { get; }

    protected ServiceTest(ITestOutputHelper output)
    {
        Output = output;
    }

    protected DeltaContext CreateDbContext()
    {
        return new DeltaContext(new TestDeltaDbContextOptionsBuilder(Output).Build());
    }
}