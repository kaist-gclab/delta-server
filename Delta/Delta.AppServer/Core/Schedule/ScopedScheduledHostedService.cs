using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NodaTime;

namespace Delta.AppServer.Core.Schedule;

public class ScopedScheduledHostedService<T>(
    ILogger<ScopedScheduledHostedService<T>> logger,
    IServiceProvider serviceProvider,
    ScheduleHelper scheduleHelper,
    IClock clock)
    : BackgroundService
    where T : IScheduledTask
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var next = Instant.MinValue;
        while (!cancellationToken.IsCancellationRequested)
        {
            if (next <= clock.GetCurrentInstant())
            {
                using var scope = serviceProvider.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<T>();

                try
                {
                    if (service.RunAtStartup || next != Instant.MinValue)
                    {
                        await service.DoWorkAsync();
                    }
                }
                catch (Exception e)
                {
                    var typeFullName = typeof(T).FullName;
                    logger.LogError(e, "ExecuteAsync {TypeFullName}", typeFullName);
                }

                next = scheduleHelper.ComputeNext(service.Interval, service.Offset);
            }

            if (next == Instant.MaxValue)
            {
                await Task.Delay(-1, cancellationToken);
                continue;
            }

            var delay = next.Minus(clock.GetCurrentInstant());
            delay = Duration.Max(delay, Duration.Zero);

            if (delay >= Duration.FromMinutes(10))
            {
                delay -= Duration.FromMinutes(5);
            }
            else
            {
                delay = Duration.Min(delay, Duration.FromSeconds(30));
            }

            await Task.Delay(delay.ToTimeSpan(), cancellationToken);
        }
    }
}