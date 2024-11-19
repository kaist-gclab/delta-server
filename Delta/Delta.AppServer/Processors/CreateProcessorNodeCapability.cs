using Delta.AppServer.JobTypes;

namespace Delta.AppServer.Processors;

public record CreateProcessorNodeCapability(JobType JobType, string MediaType);