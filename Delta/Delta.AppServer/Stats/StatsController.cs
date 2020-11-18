using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Delta.AppServer.Stats
{
    [ApiController]
    [Route(Delta.ApiRoot + "stats")]
    public class StatsController
    {
        private readonly StatsService _statsService;

        public StatsController(StatsService statsService)
        {
            _statsService = statsService;
        }

        [HttpGet]
        public List<Stats> GetStats()
        {
            return _statsService.GetEvents().TakeLast(20).ToList();
        }
    }
}