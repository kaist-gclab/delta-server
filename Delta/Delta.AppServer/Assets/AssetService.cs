using System;
using System.Linq;
using System.Threading.Tasks;
using Delta.AppServer.Jobs;
using Delta.AppServer.ObjectStorage;
using Delta.AppServer.Security;
using NodaTime;

namespace Delta.AppServer.Assets
{
    public class AssetService
    {
        private readonly DeltaContext _context;
        private readonly IClock _clock;
        private readonly IObjectStorageService _objectStorageService;
        private readonly EncryptionService _encryptionService;

        public AssetService(DeltaContext context, IClock clock,
            IObjectStorageService objectStorageService,
            EncryptionService encryptionService)
        {
            _context = context;
            _clock = clock;
            _objectStorageService = objectStorageService;
            _encryptionService = encryptionService;
        }

        public async Task<Asset> AddAsset(
            AssetFormat assetFormat,
            AssetType assetType,
            byte[] content,
            EncryptionKey encryptionKey,
            JobExecution parentJobExecution)
        {
            if (encryptionKey != null)
            {
                content = _encryptionService.Encrypt(encryptionKey, content);
            }

            var storeKey = Guid.NewGuid().ToString();
            await _objectStorageService.Write(storeKey, content);
            var asset = new Asset
            {
                AssetFormat = assetFormat,
                AssetType = assetType,
                StoreKey = storeKey,
                EncryptionKey = encryptionKey,
                ParentJobExecution = parentJobExecution,
                CreatedAt = _clock.GetCurrentInstant()
            };
            asset = (await _context.AddAsync(asset)).Entity;
            await _context.SaveChangesAsync();
            return asset;
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
                    select f).First();
        }

        public AssetType GetAssetType(string key)
        {
            if (key == null)
            {
                return null;
            }

            return (from t in _context.AssetTypes
                    where t.Key == key
                    select t).First();
        }
    }
}