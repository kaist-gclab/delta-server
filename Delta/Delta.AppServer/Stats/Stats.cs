using NodaTime;

namespace Delta.AppServer.Stats
{
    public class Stats
    {
        public Instant EventTimestamp { get; set; }
        public Instant StatsTimestamp { get; set; }

        public string Content { get; set; }
    }
}