using Xunit;
using Xunit.Abstractions;

namespace Delta.AppServer.Test.Infrastructure
{
    public abstract class ControllerTest : IClassFixture<TestWebApplicationFactory>
    {
        protected readonly TestWebApplicationFactory Factory;
        protected readonly ITestOutputHelper Output;

        protected ControllerTest(TestWebApplicationFactory factory, ITestOutputHelper output)
        {
            Factory = factory;
            Output = output;
            Factory.TestOutputHelper = Output;
        }

        protected DeltaContext CreateDbContext()
        {
            return Factory.GetService<DeltaContext>();
        }
    }
}