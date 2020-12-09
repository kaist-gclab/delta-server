using Microsoft.AspNetCore.Mvc;

namespace Delta.AppServer.Jobs
{
    [ApiController]
    [Route(Delta.ApiRoot + "jobs")]
    public class JobsController : ControllerBase
    {
        private readonly JobService _jobService;

        public JobsController(JobService jobService)
        {
            _jobService = jobService;
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