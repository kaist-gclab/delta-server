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

        [HttpPost("nodes/register")]
        public IActionResult RegisterProcessorNode(RegisterProcessorNodeRequest registerProcessorNodeRequest)
        {
            return Ok(_processorService.AddNode(registerProcessorNodeRequest));
        }

        [HttpPost("types")]
        public IActionResult CreateProcessorType([FromBody] CreateProcessorTypeRequest createProcessorTypeRequest)
        {
            return Ok(_processorService.AddProcessorType(
                createProcessorTypeRequest.Key,
                createProcessorTypeRequest.Name));
        }
    }
}