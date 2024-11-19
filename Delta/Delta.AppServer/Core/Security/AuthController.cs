using System.Threading.Tasks;
using Delta.AppServer.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Delta.AppServer.Core.Security;

[Route("/auth/1/")]
[ApiController]
public class AuthController(TokenService tokenService, UserService userService) : ControllerBase
{
    [HttpPost]
    [Route("login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest loginRequest)
    {
        var user = await userService.Login(loginRequest.Username, loginRequest.Password);
        if (user == null)
        {
            return Unauthorized();
        }

        return Ok(new LoginResponse(tokenService.BuildToken(user)));
    }
}