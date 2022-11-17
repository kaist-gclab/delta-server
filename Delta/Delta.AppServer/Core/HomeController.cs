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
        return new ApiHomeResponse(Delta.ServiceName, Delta.ApiVersion);
    }
}