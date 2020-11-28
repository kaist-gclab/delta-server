using System.Threading.Tasks;
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

        [HttpPost("schedule")]
        public IActionResult Schedule(JobScheduleRequest jobScheduleRequest)
        {
            var processorNode = _processorService.GetNode(jobScheduleRequest.ProcessorNodeId);
            if (processorNode == null)
            {
                return NotFound();
            }

            return Ok(_jobService.ScheduleNextJob(processorNode));
        }

        [HttpPost("result")]
        public async Task<IActionResult> AddJobResult(AddJobResultRequest addJobResultRequest)
        {
            return Ok(await _jobService.AddJobResult(addJobResultRequest));
        }

        [HttpPost]
        public IActionResult CreateJob(CreateJobRequest createJobRequest)
        {
            return Ok(_jobService.CreateJob(createJobRequest));
        }

        [HttpGet("executions/{jobExecutionId:long}")]
        public IActionResult GetJobExecution(long jobExecutionId)
        {
            return Ok(_jobService.GetJobExecution(jobExecutionId));
        }
    }
}