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

    public IEnumerable<Asset> GetAssets() => _context.Assets.ToList();
    public Asset? GetAsset(long id) => _context.Assets.Find(id);

    public async Task<IEnumerable<AssetTypeView>> GetAssetTypeViews()
    {
        var q = from t in _context.AssetTypes
            select new AssetTypeView(t.Id, t.Name, t.Name);

        return await q.ToListAsync();
    }

    public async Task<AssetType?> GetAssetType(string key)
    {
        var q = from t in _context.AssetTypes
            where t.Key == key
            select t;

        return await q.FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Asset>> FindByAssetTag(string? key, string? value)
    {
        if (key == null && value == null)
        {
            return new List<Asset>();
        }

        if (key == null)
        {
            var valueQuery = from a in _context.Assets
                where (from t in a.AssetTags
                    where t.Value == value
                    select t).Any()
                select a;

            return await valueQuery.ToListAsync();
        }

        if (value == null)
        {
            var keyQuery = from a in _context.Assets
                where (from t in a.AssetTags
                    where t.Key == key
                    select t).Any()
                select a;

            return await keyQuery.ToListAsync();
        }

        var keyValueQuery = from a in _context.Assets
            where (from t in a.AssetTags
                where t.Key == key && t.Value == value
                select t).Any()
            select a;

        return await keyValueQuery.ToListAsync();
    }

    public async Task AddAssetType(string key, string name)
    {
        if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(name))
        {
            return;
        }

        await using var trx = await _context.Database.BeginTransactionAsync();
        var q = from t in _context.AssetTypes
            where t.Key == key
            select t;

        if (q.Any())
        {
            return;
        }

        var assetType = new AssetType
        {
            Key = key,
            Name = name
        };

        await _context.AddAsync(assetType);
        await _context.SaveChangesAsync();
        await trx.CommitAsync();
    }

    public async Task UpdateAssetTag(long assetId, string key, string value)
    {
        var asset = await _context.Assets.FindAsync(assetId);
        if (asset == null)
        {
            return;
        }

        asset.UpdateAssetTag(key, value);
        await _context.SaveChangesAsync();
    }
}