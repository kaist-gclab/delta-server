using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Delta.AppServer.Assets;

public class AssetMetadataService
{
    private readonly DeltaContext _context;

    public AssetMetadataService(DeltaContext context)
    {
        _context = context;
    }

    public IEnumerable<Asset> GetAssets() => _context.Asset.ToList();
    public Asset? GetAsset(long id) => _context.Asset.Find(id);

    public async Task<IEnumerable<Asset>> FindByAssetTag(string? key, string? value)
    {
        if (key == null && value == null)
        {
            return new List<Asset>();
        }

        if (key == null)
        {
            var valueQuery = from a in _context.Asset
                where (from t in a.AssetTags
                    where t.Value == value
                    select t).Any()
                select a;

            return await valueQuery.ToListAsync();
        }

        if (value == null)
        {
            var keyQuery = from a in _context.Asset
                where (from t in a.AssetTags
                    where t.Key == key
                    select t).Any()
                select a;

            return await keyQuery.ToListAsync();
        }

        var keyValueQuery = from a in _context.Asset
            where (from t in a.AssetTags
                where t.Key == key && t.Value == value
                select t).Any()
            select a;

        return await keyValueQuery.ToListAsync();
    }

    public async Task UpdateAssetTag(long assetId, string key, string value)
    {
        var asset = await _context.Asset.FindAsync(assetId);
        if (asset == null)
        {
            return;
        }

        asset.UpdateAssetTag(key, value);
        await _context.SaveChangesAsync();
    }
}