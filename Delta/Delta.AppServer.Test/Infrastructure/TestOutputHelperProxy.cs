using Xunit.Abstractions;

namespace Delta.AppServer.Test.Infrastructure
{
    internal class TestOutputHelperProxy : ITestOutputHelper
    {
        private readonly TestWebApplicationFactory _factory;

        public TestOutputHelperProxy(TestWebApplicationFactory factory)
        {
            _factory = factory;
        }

        public void WriteLine(string message)
        {
            _factory.TestOutputHelper.WriteLine(message);
        }

        public void WriteLine(string format, params object[] args)
        {
            _factory.TestOutputHelper.WriteLine(format, args);
        }
    }
}