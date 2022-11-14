using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NodaTime;

namespace Delta.AppServer.Core.Schedule;

public class ScopedScheduledHostedService<T> : HostedService
    where T : IScheduledTask
{
    private readonly ILogger<ScopedScheduledHostedService<T>> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly ScheduleHelper _scheduleHelper;
    private readonly IClock _clock;

    public ScopedScheduledHostedService(ILogger<ScopedScheduledHostedService<T>> logger,
        IServiceProvider serviceProvider, ScheduleHelper scheduleHelper,
        IClock clock)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _scheduleHelper = scheduleHelper;
        _clock = clock;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var next = Instant.MinValue;
        while (!cancellationToken.IsCancellationRequested)
        {
            if (next <= _clock.GetCurrentInstant())
            {
                using var scope = _serviceProvider.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<T>();

                try
                {
                    await service.DoWorkAsync();
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                }

                next = _scheduleHelper.ComputeNext(service.Interval);
            }

            if (next == Instant.MaxValue)
            {
                await Task.Delay(-1, cancellationToken);
                continue;
            }

            var delay = next.Minus(_clock.GetCurrentInstant());
            delay = Duration.Max(delay, Duration.Zero);

            if (delay >= Duration.FromMinutes(5))
            {
                delay -= Duration.FromMinutes(2);
            }
            else
            {
                delay = Duration.Min(delay, Duration.FromSeconds(10));
            }

            await Task.Delay(delay.ToTimeSpan(), cancellationToken);
        }
    }
}