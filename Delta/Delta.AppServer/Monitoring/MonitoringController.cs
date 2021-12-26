using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Delta.AppServer.Monitoring
{
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
        public List<object> GetStats()
        {
            return _monitoringService.GetEvents().TakeLast(20).ToList();
        }
    }
}