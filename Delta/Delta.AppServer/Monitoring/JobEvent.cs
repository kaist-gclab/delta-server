using NodaTime;

namespace Delta.AppServer.Monitoring;

public record JobEvent(long Id, Instant Timestamp, string JobArguments);