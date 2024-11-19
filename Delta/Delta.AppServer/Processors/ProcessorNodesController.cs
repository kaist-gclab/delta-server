using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Delta.AppServer.Processors;

[ApiController]
[Route(Delta.ApiRoot + "processor-nodes")]
public class ProcessorNodesController(ProcessorService processorService) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<ProcessorNode>> GetNodes()
    {
        return await processorService.GetProcessorNodes();
    }

    [HttpPost("register")]
    public async Task<ActionResult<ProcessorNode>> RegisterProcessorNode(
        RegisterProcessorNodeRequest registerProcessorNodeRequest)
    {
        var node = await processorService.RegisterProcessorNode(registerProcessorNodeRequest);
        if (node == null)
        {
            return BadRequest();
        }

        return Ok(node);
    }
}