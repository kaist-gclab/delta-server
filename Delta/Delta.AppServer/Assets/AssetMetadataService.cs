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
        
        public IEnumerable<Asset> GetAssets()
        {
            return _context.Assets.ToList();
        }
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
    }
}