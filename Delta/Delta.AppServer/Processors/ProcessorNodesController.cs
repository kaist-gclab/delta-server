using Microsoft.AspNetCore.Mvc;

namespace Delta.AppServer.Processors
{
    [ApiController]
    [Route(Delta.ApiRoot + "processor-nodes")]
    public class ProcessorNodesController : ControllerBase
    {
        private readonly ProcessorService _processorService;

        public ProcessorNodesController(ProcessorService processorService)
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
            var node = _processorService.AddNode(registerProcessorNodeRequest);
            if (node == null)
            {
                return BadRequest();
            }

            return Ok(node);
        }
    }
}