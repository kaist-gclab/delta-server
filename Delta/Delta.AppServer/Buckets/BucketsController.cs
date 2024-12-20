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
    public async Task<IEnumerable<BucketSummary>> GetBuckets()
    {
        return await bucketService.GetBuckets();
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<BucketView>> GetBucket(long id)
    {
        var bucket = await bucketService.GetBucket(id);
        if (bucket == null)
        {
            return NotFound();
        }

        return Ok(bucket);
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

    [HttpPut("{id:long}")]
    [Command]
    public async Task<ActionResult> Update(long id, [FromBody] UpdateBucketRequest updateBucketRequest)
    {
        await bucketService.UpdateBucket(id, updateBucketRequest);
        return Ok();
    }
}