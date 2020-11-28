using System;
using System.Threading.Tasks;
using Delta.AppServer.Jobs;
using Delta.AppServer.ObjectStorage;
using Delta.AppServer.Encryption;
using NodaTime;

namespace Delta.AppServer.Assets
{
    public class AssetService
    {
        private readonly DeltaContext _context;
        private readonly IClock _clock;
        private readonly IObjectStorageService _objectStorageService;
        private readonly EncryptionService _encryptionService;
        private readonly CompressionService _compressionService;

        public AssetService(DeltaContext context, IClock clock,
            IObjectStorageService objectStorageService,
            EncryptionService encryptionService,
            CompressionService compressionService)
        {
            _context = context;
            _clock = clock;
            _objectStorageService = objectStorageService;
            _encryptionService = encryptionService;
            _compressionService = compressionService;
        }

        public async Task<Asset> AddAsset(
            AssetType assetType,
            byte[] content,
            EncryptionKey encryptionKey,
            JobExecution parentJobExecution)
        {
            content = _compressionService.Compress(content);
            if (encryptionKey != null)
            {
                content = _encryptionService.Encrypt(encryptionKey, content);
            }

            var storeKey = Guid.NewGuid().ToString();
            await _objectStorageService.Write(storeKey, content);
            var asset = new Asset
            {
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

        public async Task<byte[]> ReadAssetContent(Asset asset)
        {
            var content = await _objectStorageService.Read(asset.StoreKey);
            if (asset.EncryptionKey != null)
            {
                content = _encryptionService.Decrypt(asset.EncryptionKey, content);
            }

            content = _compressionService.Decompress(content);

            return content;
        }
    }
}