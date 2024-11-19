using System.Collections.Generic;
using System.Threading.Tasks;
using CodeGen.Analysis;
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
    
    [HttpPost]
    [Command]
    public async Task<ActionResult> Create([FromBody] CreateBucketRequest createBucketRequest)
    {
        await bucketService.AddBucket(createBucketRequest);
        return Ok();
    }
    
    [HttpDelete("{id:long}")]
    [Command]
    public async Task<ActionResult> Delete(long id)
    {
        await bucketService.DeleteBucket(id);
        return Ok();
    }
    
}