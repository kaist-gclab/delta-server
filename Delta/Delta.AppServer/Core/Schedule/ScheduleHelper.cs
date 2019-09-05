using NodaTime;

namespace Delta.AppServer.Core.Schedule
{
    public class ScheduleHelper
    {
        private readonly IClock _clock;
        private readonly DateTimeZone _dateTimeZone;

        public ScheduleHelper(IClock clock, DateTimeZone dateTimeZone)
        {
            _clock = clock;
            _dateTimeZone = dateTimeZone;
        }

        public Instant ComputeNext(Duration interval)
        {
            if (interval == Duration.MaxValue)
            {
                return Instant.MaxValue;
            }

            var current = _clock.GetCurrentInstant();
            current += Duration.FromSeconds(1);

            var next = current.InZone(_dateTimeZone).Date.AtMidnight()
                .InZoneLeniently(_dateTimeZone).ToInstant();

            for (;;)
            {
                if (next >= current)
                {
                    return next;
                }

                next += interval;
            }
        }
    }
}