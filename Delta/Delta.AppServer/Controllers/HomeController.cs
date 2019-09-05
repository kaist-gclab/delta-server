using Microsoft.AspNetCore.Mvc;

namespace Delta.AppServer.Controllers
{
    [Route("/")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        public string Home()
        {
            return "Delta";
        }
    }
}