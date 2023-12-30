using System.Threading.Tasks;
using Delta.AppServer.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Delta.AppServer.Core.Security;

[Route("/auth/1/")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserService _userService;
    private readonly TokenService _tokenService;

    public AuthController(TokenService tokenService, UserService userService)
    {
        _tokenService = tokenService;
        _userService = userService;
    }

    [HttpPost]
    [Route("login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest loginRequest)
    {
        var user = await _userService.Login(loginRequest.Username, loginRequest.Password);
        if (user == null)
        {
            return Unauthorized();
        }

        return Ok(new LoginResponse(_tokenService.BuildToken(user)));
    }
}