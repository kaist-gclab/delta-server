using NodaTime;

namespace Delta.AppServer.Monitoring;

public record MonitoringServiceEvent(Instant EventTimestamp, Instant StatsTimestamp, string Content);