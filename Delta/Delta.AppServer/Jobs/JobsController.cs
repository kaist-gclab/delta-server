using Delta.AppServer.Processors;
using Microsoft.AspNetCore.Mvc;

namespace Delta.AppServer.Jobs
{
    [ApiController]
    [Route(Delta.ApiRoot + "jobs")]
    public class JobsController : ControllerBase
    {
        private readonly JobService _jobService;
        private readonly ProcessorService _processorService;

        public JobsController(JobService jobService, ProcessorService processorService)
        {
            _jobService = jobService;
            _processorService = processorService;
        }

        [HttpGet]
        public IActionResult Schedule(long processorNodeId)
        {
            var processorNode = _processorService.GetNode(processorNodeId);
            if (processorNode == null)
            {
                return NotFound();
            }

            return Ok(_jobService.ScheduleNextJob(processorNode));
        }
    }
}