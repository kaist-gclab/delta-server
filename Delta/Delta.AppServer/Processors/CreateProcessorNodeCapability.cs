using Delta.AppServer.Assets;
using Delta.AppServer.Jobs;

namespace Delta.AppServer.Processors;

public record CreateProcessorNodeCapability(JobType JobType, AssetType? AssetType, string MediaType);