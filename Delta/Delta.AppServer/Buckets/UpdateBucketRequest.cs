using System.Collections.Generic;

namespace Delta.AppServer.Buckets;

public record UpdateBucketRequest(IEnumerable<UpdateBucketRequestBucketTag> Tags);