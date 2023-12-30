using System.Collections.Generic;
using System.Threading.Tasks;
using CodeGen.Analysis;
using Microsoft.AspNetCore.Mvc;

namespace Delta.AppServer.Jobs;

[ApiController]
[Route(Delta.ApiRoot + "jobs")]
public class JobsController : ControllerBase
{
    private readonly JobService _jobService;

    public JobsController(JobService jobService)
    {
        _jobService = jobService;
    }

    [HttpGet]
    public async Task<IEnumerable<Job>> GetJobs()
    {
        return await _jobService.GetJobs();
    }

    [HttpPost("schedule")]
    public async Task<ActionResult<JobScheduleResponse>> Schedule(JobScheduleRequest jobScheduleRequest)
    {
        var response = await _jobService.ScheduleNextJob(jobScheduleRequest);
        if (response == null)
        {
            return NoContent();
        }

        return Ok(response);
    }

    [HttpPost]
    public ActionResult<Job> CreateJob(CreateJobRequest createJobRequest)
    {
        return Ok(_jobService.CreateJob(createJobRequest));
    }

    [HttpPost("executions/{jobExecutionId:long}/statuses")]
    [Command]
    public IActionResult AddJobExecutionStatus(long jobExecutionId,
        AddJobExecutionStatusRequest addJobExecutionStatusRequest)
    {
        _jobService.AddJobExecutionStatus(jobExecutionId, addJobExecutionStatusRequest);
        return Ok();
    }
}