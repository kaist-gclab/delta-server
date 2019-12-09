using Microsoft.AspNetCore.Mvc;

namespace Delta.AppServer.Processors
{
    [ApiController]
    [Route(Delta.ApiRoot + "processors")]
    public class ProcessorsController : ControllerBase
    {
        private readonly ProcessorService _processorService;

        public ProcessorsController(ProcessorService processorService)
        {
            _processorService = processorService;
        }

        [HttpGet("nodes")]
        public IActionResult GetNodes()
        {
            return Ok(_processorService.GetProcessorNodes());
        }
    }
}