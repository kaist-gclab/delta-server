using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Delta.AppServer.Jobs;

[ApiController]
[Route(Delta.ApiRoot + "job-types")]
public class JobTypesController : ControllerBase
{
    private readonly JobService _jobService;

    public JobTypesController(JobService jobService)
    {
        _jobService = jobService;
    }

    [HttpGet]
    public async Task<IEnumerable<JobType>> GetJobTypes()
    {
        return await _jobService.GetJobTypes();
    }
}