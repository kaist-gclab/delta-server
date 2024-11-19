using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Delta.AppServer.Jobs;

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