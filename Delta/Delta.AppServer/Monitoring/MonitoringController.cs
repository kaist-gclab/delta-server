using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Delta.AppServer.Monitoring;

[ApiController]
[Route(Delta.ApiRoot + "monitoring")]
public class MonitoringController(MonitoringService monitoringService)
{
    private readonly MonitoringService _monitoringService = monitoringService;

    [HttpGet("object-storage")]
    public IEnumerable<ObjectStorageEvent> GetObjectStorageEvents()
    {
        return new List<ObjectStorageEvent>();
    }

    [HttpGet("processor-node")]
    public IEnumerable<ProcessorNodeEvent> GetProcessorNodeEvents()
    {
        return new List<ProcessorNodeEvent>();
    }

    [HttpGet("job")]
    public IEnumerable<JobEvent> GetJobEvents()
    {
        return new List<JobEvent>();
    }
}