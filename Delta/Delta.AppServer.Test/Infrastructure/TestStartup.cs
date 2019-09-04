using Microsoft.Extensions.Configuration;

namespace Delta.AppServer.Test.Infrastructure
{
    public class TestStartup : Startup.Startup
    {
        public TestStartup(IConfiguration configuration) : base(configuration)
        {
        }
    }
}