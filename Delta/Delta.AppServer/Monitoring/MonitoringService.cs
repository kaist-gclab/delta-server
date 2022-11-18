using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Core.Flux.Domain;
using NodaTime;

namespace Delta.AppServer.Monitoring;

public class MonitoringService
{
    private static readonly List<MonitoringServiceEvent> InMemoryStore = new();
    private readonly MonitoringConfig _monitoringConfig;
    private readonly IClock _clock;

    public MonitoringService(IClock clock, MonitoringConfig monitoringConfig)
    {
        _clock = clock;
        _monitoringConfig = monitoringConfig;
    }

    public void AddProcessorNodeEvent(string content)
    {
    }

    public async Task AddObjectStorageEvent(ObjectStorageEvent objectStorageEvent)
    {
        using var client = InfluxDBClientFactory.Create(_monitoringConfig.Endpoint, _monitoringConfig.Token);
        var write = client.GetWriteApiAsync();
        await write.WriteMeasurementAsync(
            objectStorageEvent, WritePrecision.Ns,
            _monitoringConfig.Bucket, _monitoringConfig.Organization);
    }

    public async Task<List<FluxTable>> GetObjectStorageEvents()
    {
        using var client = InfluxDBClientFactory.Create(_monitoringConfig.Endpoint, _monitoringConfig.Token);
        var query = client.GetQueryApi();
        return await query.QueryAsync(
            "from(bucket: \"" + _monitoringConfig.Bucket + "\")" +
            "|> range(start: -1d)" +
            "|> aggregateWindow(every: 15m, fn: mean)", _monitoringConfig.Organization);
    }

    public void AddEvent(Instant eventTimestamp, string content)
    {
        lock (typeof(MonitoringService))
        {
            InMemoryStore.Add(new MonitoringServiceEvent(eventTimestamp, _clock.GetCurrentInstant(), content));
        }
    }


    public IEnumerable<MonitoringServiceEvent> GetEvents()
    {
        lock (typeof(MonitoringService))
        {
            return InMemoryStore.OrderBy(o => o.ToString()).ToList();
        }
    }
}