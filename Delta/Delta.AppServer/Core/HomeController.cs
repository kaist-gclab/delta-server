using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Delta.AppServer.Core
{
    [Route("/")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [AllowAnonymous]
        public string Home()
        {
            return "Delta";
        }

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
}