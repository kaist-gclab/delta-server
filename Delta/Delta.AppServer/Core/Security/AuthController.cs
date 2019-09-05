using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Delta.AppServer.Core.Security
{
    [Route("/auth/1/")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly TokenService _tokenService;

        public AuthController(AuthService authService, TokenService tokenService)
        {
            _authService = authService;
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            var account = _authService.Login(loginRequest.Username, loginRequest.Password);
            if (account == null)
            {
                return Unauthorized();
            }

            return Ok(new LoginResponse
            {
                Token = _tokenService.BuildToken(account)
            });
        }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}