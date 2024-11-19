using System.Collections.Generic;
using System.Threading.Tasks;
using Delta.AppServer.Jobs;
using Microsoft.AspNetCore.Mvc;

namespace Delta.AppServer.JobTypes;

[ApiController]
[Route(Delta.ApiRoot + "job-types")]
public class JobTypesController(JobService jobService) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<JobTypeView>> GetJobTypes()
    {
        return await jobService.GetJobTypeViews();
    }
}