using System;
using System.Collections.Generic;
using System.Linq;

namespace Delta.AppServer.Assets
{
    public class AssetMetadataService
    {
        private readonly DeltaContext _context;

        public AssetMetadataService(DeltaContext context)
        {
            _context = context;
        }

        public IEnumerable<Asset> GetAssets() => _context.Assets.ToList();
        public Asset GetAsset(long id) => _context.Assets.Find(id);
        public AssetFormat GetAssetFormat(long id) => _context.AssetFormats.Find(id);
        public AssetType GetAssetType(long id) => _context.AssetTypes.Find(id);

        public AssetFormat GetAssetFormat(string key)
        {
            if (key == null)
            {
                return null;
            }

            return (from f in _context.AssetFormats
                    where f.Key == key
                    select f).FirstOrDefault();
        }

        public AssetType GetAssetType(string key)
        {
            if (key == null)
            {
                return null;
            }

            return (from t in _context.AssetTypes
                    where t.Key == key
                    select t).FirstOrDefault();
        }

        public AssetTag UpdateAssetTag(Asset asset, string key, string value)
        {
            if (key == null)
            {
                throw new ArgumentNullException();
            }

            var tag = (from t in asset.AssetTags
                       where t.Key == key
                       select t).FirstOrDefault();

            if (tag != null)
            {
                asset.AssetTags.Remove(tag);
            }

            if (value == null)
            {
                _context.SaveChanges();
                return null;
            }

            var assetTag = new AssetTag
            {
                Key = key,
                Value = value
            };
            asset.AssetTags.Add(assetTag);
            _context.SaveChanges();
            return assetTag;
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

        public AssetFormat AddAssetFormat(string key, string name, string description)
        {
            using var trx = _context.Database.BeginTransaction();
            var q = from f in _context.AssetFormats
                    where f.Key == key
                    select f;
            if (q.Any())
            {
                throw new ArgumentException();
            }

            var assetFormat = new AssetFormat
            {
                Key = key,
                Name = name,
                Description = description,
            };
            assetFormat = _context.Add(assetFormat).Entity;
            _context.SaveChanges();
            trx.Commit();
            return assetFormat;
        }
    }
}