using System.Threading.Tasks;
using NodaTime;

namespace Delta.AppServer.Core.Schedule;

public interface IScheduledTask
{
    Duration Interval { get; }

    LocalTime Offset { get; }

    bool RunAtStartup { get; }

    Task DoWorkAsync();
}