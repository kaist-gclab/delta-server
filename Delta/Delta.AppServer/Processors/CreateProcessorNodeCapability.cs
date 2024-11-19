using Delta.AppServer.Jobs;

namespace Delta.AppServer.Processors;

public record CreateProcessorNodeCapability(JobType JobType, string MediaType);