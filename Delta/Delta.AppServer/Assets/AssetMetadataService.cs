using System;
using System.Collections.Generic;
using System.Linq;

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
    public IQueryable<AssetType> GetAssetTypes() => _context.AssetTypes;

    public AssetType? GetAssetType(string key)
    {
        return (from t in _context.AssetTypes
            where t.Key == key
            select t).FirstOrDefault();
    }

    public IEnumerable<Asset> FindByAssetTag(string key, string value)
    {
        if (key == null && value == null)
        {
            throw new ArgumentNullException();
        }

        if (key == null)
        {
            return (from a in _context.Assets
                where (from t in a.AssetTags
                    where t.Value == value
                    select t).Any()
                select a).ToList();
        }

        if (value == null)
        {
            return (from a in _context.Assets
                where (from t in a.AssetTags
                    where t.Key == key
                    select t).Any()
                select a).ToList();
        }

        return (from a in _context.Assets
            where (from t in a.AssetTags
                where t.Key == key && t.Value == value
                select t).Any()
            select a).ToList();
    }

    public AssetType AddAssetType(string key, string name)
    {
        if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(name))
        {
            throw new Exception();
        }

        using var trx = _context.Database.BeginTransaction();
        var q = from t in _context.AssetTypes
            where t.Key == key
            select t;
        if (q.Any())
        {
            throw new Exception();
        }

        var assetType = new AssetType
        {
            Key = key,
            Name = name
        };
        assetType = _context.Add(assetType).Entity;
        _context.SaveChanges();
        trx.Commit();
        return assetType;
    }
}