using System.Net;
using Delta.AppServer.Test.Infrastructure;
using Microsoft.AspNetCore.Http;
using Xunit;
using Xunit.Abstractions;

namespace Delta.AppServer.Test.Core
{
    public class HomeControllerTest : ControllerTest
    {
        public HomeControllerTest(TestWebApplicationFactory factory, ITestOutputHelper output) : base(factory, output)
        {
        }

        [Fact]
        public async void Home()
        {
            var client = Factory.CreateClient();
            Assert.Equal("Delta", await client.GetStringAsync("/"));
        }
    }
}