using System.Collections.Generic;
using System.Threading.Tasks;
using CodeGen.Analysis;
using Microsoft.AspNetCore.Mvc;

namespace Delta.AppServer.Jobs;

[ApiController]
[Route(Delta.ApiRoot + "jobs")]
public class JobsController(JobService jobService) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<Job>> GetJobs()
    {
        return await jobService.GetJobs();
    }

    [HttpPost("schedule")]
    public async Task<ActionResult<JobScheduleResponse>> Schedule(JobScheduleRequest jobScheduleRequest)
    {
        var response = await jobService.ScheduleNextJob(jobScheduleRequest);
        if (response == null)
        {
            return NoContent();
        }

        return Ok(response);
    }

    [HttpPost]
    public ActionResult<Job> CreateJob(CreateJobRequest createJobRequest)
    {
        return Ok(jobService.CreateJob(createJobRequest));
    }

    [HttpPost("executions/{jobExecutionId:long}/statuses")]
    [Command]
    public async Task<IActionResult> AddJobExecutionStatus(long jobExecutionId,
        AddJobExecutionStatusRequest addJobExecutionStatusRequest)
    {
        await jobService.AddJobExecutionStatus(jobExecutionId, addJobExecutionStatusRequest);
        return Ok();
    }
}