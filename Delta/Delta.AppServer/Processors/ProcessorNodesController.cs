using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Delta.AppServer.Processors;

[ApiController]
[Route(Delta.ApiRoot + "processor-nodes")]
public class ProcessorNodesController : ControllerBase
{
    private readonly ProcessorService _processorService;

    public ProcessorNodesController(ProcessorService processorService)
    {
        _processorService = processorService;
    }

    [HttpGet]
    public async Task<IEnumerable<ProcessorNode>> GetNodes()
    {
        return await _processorService.GetProcessorNodes();
    }

    [HttpPost("register")]
    public async Task<ActionResult<ProcessorNode>> RegisterProcessorNode(
        RegisterProcessorNodeRequest registerProcessorNodeRequest)
    {
        var node = await _processorService.RegisterProcessorNode(registerProcessorNodeRequest);
        if (node == null)
        {
            return BadRequest();
        }

        return Ok(node);
    }
}