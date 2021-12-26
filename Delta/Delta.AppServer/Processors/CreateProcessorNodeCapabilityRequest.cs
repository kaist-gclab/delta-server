using Delta.AppServer.Assets;
using Delta.AppServer.Jobs;

namespace Delta.AppServer.Processors;

public record CreateProcessorNodeCapabilityRequest(long JobTypeId, long? AssetTypeId, string MediaType);

public record CreateProcessorNodeCapability(JobType JobType, AssetType? AssetType, string MediaType);