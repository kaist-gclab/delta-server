using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Delta.AppServer.Buckets;

public class BucketService(DeltaContext context)
{
    public async Task<IEnumerable<BucketView>> GetBuckets()
    {
        var q = from b in context.Bucket
            select new BucketView();
        return await q.ToListAsync();
    }
}