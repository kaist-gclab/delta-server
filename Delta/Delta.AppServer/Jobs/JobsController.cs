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
        public JobScheduleResponse Schedule(JobScheduleRequest jobScheduleRequest)
        {
            return _jobService.ScheduleNextJob(jobScheduleRequest);
        }

        [HttpPost]
        public IActionResult CreateJob(CreateJobRequest createJobRequest)
        {
            return Ok(_jobService.CreateJob(createJobRequest));
        }

        [HttpPost("executions/{jobExecutionId:long}/statuses")]
        public IActionResult AddJobExecutionStatus(long jobExecutionId,
            AddJobExecutionStatusRequest addJobExecutionStatusRequest)
        {
            _jobService.AddJobExecutionStatus(jobExecutionId, addJobExecutionStatusRequest);
            return Ok();
        }
    }
}