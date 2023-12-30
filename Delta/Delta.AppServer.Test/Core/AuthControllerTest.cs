using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Delta.AppServer.Core.Security;
using Delta.AppServer.Test.Infrastructure;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace Delta.AppServer.Test.Core;

public class AuthControllerTest : ControllerTest
{
    public AuthControllerTest(TestWebApplicationFactory factory, ITestOutputHelper output) : base(factory, output)
    {
    }

    private const string Username = "TEST_ADMIN_USERNAME";
    private const string Password = "TEST_ADMIN_PASSWORD";

    [Theory]
    [InlineData(Username, Password)]
    public async void LoginCorrect(string username, string password)
    {
        Assert.True(await LoginAdmin(username, password));
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("a", "a")]
    [InlineData(Username, "a")]
    [InlineData("a", Password)]
    public async void LoginIncorrect(string username, string password)
    {
        Assert.False(await LoginAdmin(username, password));
    }

    private async Task<bool> LoginAdmin(string username, string password)
    {
        var client = Factory.CreateClient();
        var request = new LoginRequest(username, password);
        var response = await client.PostAsync("/auth/1/login",
            new ObjectContent<LoginRequest>(request, new JsonMediaTypeFormatter()));
        Assert.Contains(response.StatusCode,
            new[] { HttpStatusCode.Unauthorized, HttpStatusCode.OK, HttpStatusCode.BadRequest });

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Unauthorized", content);
            Assert.Contains("401", content);
            return false;
        }

        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("validation errors", content);
            return false;
        }

        var text = await response.Content.ReadAsStringAsync();
        var obj = JsonConvert.DeserializeObject<Dictionary<string, string>>(text);
        Assert.Single(obj);
        Assert.Equal("token", obj.Keys.First());
        var token = obj["token"];
        Output.WriteLine(token);
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
        Assert.Equal("Delta.AppServer.Test", jwt.Issuer);
        Assert.Contains("authInfo", from c in jwt.Claims select c.Type);
        var claim = jwt.Claims.First();
        Assert.Equal("authInfo", claim.Type);
        var authInfo = JsonConvert.DeserializeObject<AuthInfo>(claim.Value);
        Assert.Equal("Admin", authInfo.Role);
        return true;
    }
}