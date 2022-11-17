using System.Net;
using Delta.AppServer.Core;
using Delta.AppServer.Test.Infrastructure;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace Delta.AppServer.Test.Core;

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

    [Fact]
    public async void ApiHomeUnauthorized()
    {
        var client = Factory.CreateClient();
        var response = await client.GetAsync("/api/1");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.Empty(await response.Content.ReadAsByteArrayAsync());
    }

    [Fact]
    public async void ApiHomeAuth()
    {
        var client = Factory.CreateAdminClient();
        var response = await client.GetAsync("/api/1");
        response.EnsureSuccessStatusCode();
        var text = await response.Content.ReadAsStringAsync();
        var actual = JsonConvert.DeserializeObject<ApiHomeResponse>(text);
        Assert.NotNull(actual);
        Assert.Equal(Delta.ServiceName, actual.ServiceName);
        Assert.Equal(Delta.ApiVersion, actual.ApiVersion);
    }
}