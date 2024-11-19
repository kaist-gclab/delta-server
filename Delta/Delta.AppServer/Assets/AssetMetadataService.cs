using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Delta.AppServer.Assets;

public class AssetMetadataService(DeltaContext context)
{
    public IEnumerable<Asset> GetAssets() => context.Asset.ToList();
    public Asset? GetAsset(long id) => context.Asset.Find(id);

    public async Task<IEnumerable<Asset>> FindByAssetTag(string? key, string? value)
    {
        if (key == null && value == null)
        {
            return new List<Asset>();
        }

        if (key == null)
        {
            var valueQuery = from a in context.Asset
                where (from t in a.AssetTags
                    where t.Value == value
                    select t).Any()
                select a;

            return await valueQuery.ToListAsync();
        }

        if (value == null)
        {
            var keyQuery = from a in context.Asset
                where (from t in a.AssetTags
                    where t.Key == key
                    select t).Any()
                select a;

            return await keyQuery.ToListAsync();
        }

        var keyValueQuery = from a in context.Asset
            where (from t in a.AssetTags
                where t.Key == key && t.Value == value
                select t).Any()
            select a;

        return await keyValueQuery.ToListAsync();
    }

    public async Task UpdateAssetTag(long assetId, string key, string value)
    {
        var asset = await context.Asset.FindAsync(assetId);
        if (asset == null)
        {
            return;
        }

        asset.UpdateAssetTag(key, value);
        await context.SaveChangesAsync();
    }
}