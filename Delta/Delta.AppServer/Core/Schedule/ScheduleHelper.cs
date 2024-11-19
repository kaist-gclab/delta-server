using NodaTime;

namespace Delta.AppServer.Core.Schedule;

public class ScheduleHelper(IClock clock, DateTimeZone dateTimeZone)
{
    public Instant ComputeNext(Duration interval, LocalTime offset)
    {
        if (interval == Duration.MaxValue)
        {
            return Instant.MaxValue;
        }

        var current = clock.GetCurrentInstant();
        current += Duration.FromSeconds(1);

        var next = current.InZone(dateTimeZone).Date.At(offset)
            .InZoneLeniently(dateTimeZone).ToInstant();

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