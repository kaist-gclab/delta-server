using System.Collections.Generic;
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
    public IEnumerable<ProcessorNode> GetNodes()
    {
        return _processorService.GetProcessorNodes();
    }

    [HttpPost("register")]
    public ActionResult<ProcessorNode> RegisterProcessorNode(
        RegisterProcessorNodeRequest registerProcessorNodeRequest)
    {
        var node = _processorService.RegisterProcessorNode(registerProcessorNodeRequest);
        if (node == null)
        {
            return BadRequest();
        }

        return Ok(node);
    }
}