using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Core.Flux.Domain;
using NodaTime;

namespace Delta.AppServer.Monitoring;

public class MonitoringService(IClock clock, MonitoringConfig monitoringConfig)
{
    private static readonly List<MonitoringServiceEvent> InMemoryStore = new();

    public void AddProcessorNodeEvent(string content)
    {
    }

    public async Task AddObjectStorageEvent(ObjectStorageEvent objectStorageEvent)
    {
        using var client = InfluxDBClientFactory.Create(monitoringConfig.Endpoint, monitoringConfig.Token);
        var write = client.GetWriteApiAsync();
        await write.WriteMeasurementAsync(
            objectStorageEvent, WritePrecision.Ns,
            monitoringConfig.Bucket, monitoringConfig.Organization);
    }

    public async Task<List<FluxTable>> GetObjectStorageEvents()
    {
        using var client = InfluxDBClientFactory.Create(monitoringConfig.Endpoint, monitoringConfig.Token);
        var query = client.GetQueryApi();
        return await query.QueryAsync(
            "from(bucket: \"" + monitoringConfig.Bucket + "\")" +
            "|> range(start: -1d)" +
            "|> aggregateWindow(every: 15m, fn: mean)", monitoringConfig.Organization);
    }

    public void AddEvent(Instant eventTimestamp, string content)
    {
        lock (typeof(MonitoringService))
        {
            InMemoryStore.Add(new MonitoringServiceEvent(eventTimestamp, clock.GetCurrentInstant(), content));
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