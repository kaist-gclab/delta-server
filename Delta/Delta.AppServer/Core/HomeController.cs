using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Delta.AppServer.Core;

[Route("/")]
[ApiController]
public class HomeController : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public string Home()
    {
        return "Delta";
    }

    [HttpGet]
    [Route(Delta.ApiRoot)]
    public ApiHomeResponse ApiHome()
    {
        return new ApiHomeResponse
        {
            ServiceName = Delta.ServiceName,
            ApiVersion = Delta.ApiVersion
        };
    }

    public class ApiHomeResponse
    {
        public string ServiceName { get; set; }
        public string ApiVersion { get; set; }
    }
}