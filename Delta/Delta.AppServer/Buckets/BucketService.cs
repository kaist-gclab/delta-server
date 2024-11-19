using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace Delta.AppServer.Buckets;

public class BucketService(DeltaContext context, IClock clock)
{
    public async Task<IEnumerable<BucketView>> GetBuckets()
    {
        var q = from b in context.Bucket
            let tags = from t in b.Tags
                select new BucketTagView(t.Id, t.Key, t.Value)
            let nameTags = from t in b.Tags
                where t.Key == "Name"
                select t.Value
            select new BucketView(
                b.Id,
                b.EncryptionKey != null ? b.EncryptionKey.Name : null,
                b.CreatedAt,
                b.BucketGroup != null ? b.BucketGroup.Name : null,
                nameTags.FirstOrDefault(),
                tags);
        return await q.ToListAsync();
    }

    public async Task AddBucket(CreateBucketRequest request)
    {
        var enc = await context.EncryptionKey.FindAsync(request.EncryptionKeyId);
        if (enc == null)
        {
            return;
        }

        var now = clock.GetCurrentInstant();
        var bucket = new Bucket
        {
            EncryptionKey = enc,
            CreatedAt = now,
        };
        await context.AddAsync(bucket);

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            var tag = new BucketTag
            {
                Bucket = bucket,
                Key = "Name",
                Value = request.Name,
            };
            await context.AddAsync(tag);
        }

        await context.SaveChangesAsync();
    }
}