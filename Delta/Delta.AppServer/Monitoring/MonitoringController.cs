using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Delta.AppServer.Monitoring;

[ApiController]
[Route(Delta.ApiRoot + "monitoring")]
public class MonitoringController
{
    private readonly MonitoringService _monitoringService;

    public MonitoringController(MonitoringService monitoringService)
    {
        _monitoringService = monitoringService;
    }

    [HttpGet]
    public IEnumerable<MonitoringServiceEvent> GetStats()
    {
        return _monitoringService.GetEvents().TakeLast(20).ToList();
    }
}