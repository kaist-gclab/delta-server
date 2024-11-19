using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Delta.AppServer.Buckets;

[ApiController]
[Route(Delta.ApiRoot + "buckets")]
public class BucketsController(BucketService bucketService) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<BucketView>> GetBuckets()
    {
        return await bucketService.GetBuckets();
    }
}