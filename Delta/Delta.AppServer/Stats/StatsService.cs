using System.Collections.Generic;
using System.Linq;
using NodaTime;

namespace Delta.AppServer.Stats
{
    public class StatsService
    {
        private static readonly List<Stats> InMemoryStore = new List<Stats>();

        private readonly IClock _clock;

        public StatsService(IClock clock)
        {
            _clock = clock;
        }

        public void AddEvent(Instant eventTimestamp, string content)
        {
            lock (typeof(StatsService))
            {
                InMemoryStore.Add(new Stats
                {
                    EventTimestamp = eventTimestamp,
                    StatsTimestamp = _clock.GetCurrentInstant(),
                    Content = content
                });
            }
        }

        public List<Stats> GetEvents()
        {
            lock (typeof(StatsService))
            {
                return InMemoryStore.OrderBy(o => o.StatsTimestamp).ToList();
            }
        }
    }
}