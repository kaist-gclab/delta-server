using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace Delta.AppServer.Buckets;

public class BucketService(DeltaContext context, IClock clock)
{
    public async Task<IEnumerable<BucketSummary>> GetBuckets()
    {
        var q = from b in context.Bucket
            let tags = from t in b.Tags
                select new BucketTagView(t.Id, t.Key, t.Value)
            let nameTags = from t in b.Tags
                where t.Key == "Name"
                select t.Value
            select new BucketSummary(
                b.Id,
                b.EncryptionKey != null ? b.EncryptionKey.Name : null,
                b.CreatedAt,
                b.BucketGroup != null ? b.BucketGroup.Name : null,
                nameTags.FirstOrDefault(),
                tags.Count());
        return await q.ToListAsync();
    }

    public async Task AddBucket(CreateBucketRequest request)
    {
        var enc = (from e in context.EncryptionKey
            where e.Name == request.EncryptionKeyName
            select e).FirstOrDefault();
        if (!string.IsNullOrWhiteSpace(request.EncryptionKeyName) && enc == null)
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

    public async Task DeleteBucket(long id)
    {
        var bucket = await context.Bucket.FindAsync(id);
        if (bucket == null)
        {
            return;
        }

        context.Remove(bucket);
        await context.SaveChangesAsync();
    }

    public async Task UpdateBucket(long id, UpdateBucketRequest updateBucketRequest)
    {
        var bucket = await context.Bucket.FindAsync(id);
        if (bucket == null)
        {
            return;
        }

        var tags = bucket.Tags.ToList();

        // 새로운 태그를 추가하거나 기존 태그를 업데이트합니다.
        foreach (var tag in updateBucketRequest.Tags)
        {
            var t = tags.FirstOrDefault(t =>
                t.Bucket == bucket && t.Key == tag.Key);
            if (t == null)
            {
                t = new BucketTag
                {
                    Bucket = bucket,
                    Key = tag.Key,
                    Value = tag.Value,
                };
                await context.AddAsync(t);
            }
            else
            {
                t.Value = tag.Value;
            }
        }

        // 기존 태그를 삭제합니다.
        foreach (var tag in tags)
        {
            if (updateBucketRequest.Tags.All(t => t.Key != tag.Key))
            {
                context.Remove(tag);
            }
        }

        await context.SaveChangesAsync();
    }

    public async Task<BucketView?> GetBucket(long id)
    {
        var q = from b in context.Bucket
            where b.Id == id
            let tags = from t in b.Tags
                select new BucketTagView(t.Id, t.Key, t.Value)
            select new BucketView(
                b.Id,
                b.EncryptionKey != null ? b.EncryptionKey.Name : null,
                b.CreatedAt,
                b.BucketGroup != null ? b.BucketGroup.Name : null,
                tags);

        return await q.FirstOrDefaultAsync();
    }
}