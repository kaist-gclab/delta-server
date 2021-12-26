using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Delta.AppServer.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Delta.AppServer.Core.Security
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;
        private readonly UserService _userService;

        public TokenService(IConfiguration configuration, UserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        private static string GenerateJwtId()
        {
            var randomBytes = new byte[32];
            RandomNumberGenerator.Create().GetNonZeroBytes(randomBytes);
            return randomBytes.ToHexString();
        }

        public string BuildToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var authInfo = BuildAuthInfo(user);
            var claims = new[]
            {
                new Claim("authInfo",
                    JsonConvert.SerializeObject(authInfo,
                        new JsonSerializerSettings
                            { ContractResolver = new CamelCasePropertyNamesContractResolver() })),
                new Claim("jti", GenerateJwtId())
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"], _configuration["Jwt:Issuer"],
                claims, signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private AuthInfo BuildAuthInfo(User user)
        {
            return new AuthInfo(user, _userService.GetRole(user));
        }
    }
}