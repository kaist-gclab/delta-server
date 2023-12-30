using System.Threading.Tasks;
using Delta.AppServer.Encryption;
using Delta.AppServer.Jobs;
using Delta.AppServer.ObjectStorage;
using NodaTime;

namespace Delta.AppServer.Assets;

public class AssetService
{
    private readonly DeltaContext _context;
    private readonly IClock _clock;
    private readonly IObjectStorageService _objectStorageService;

    public AssetService(DeltaContext context, IClock clock, IObjectStorageService objectStorageService)
    {
        _context = context;
        _clock = clock;
        _objectStorageService = objectStorageService;
    }

    public async Task AddAsset(CreateAssetRequest createAssetRequest)
    {
        var assetType = await _context.FindAsync<AssetType>(createAssetRequest.AssetTypeId);
        if (assetType == null)
        {
            return;
        }

        var parentJobExecution = await _context.FindAsync<JobExecution>(createAssetRequest.ParentJobExecutionId);
        if (parentJobExecution == null)
        {
            return;
        }

        var encryptionKey = createAssetRequest.EncryptionKeyId == null
            ? null
            : await _context.FindAsync<EncryptionKey>(createAssetRequest.EncryptionKeyId);

        if (createAssetRequest.EncryptionKeyId != null && encryptionKey == null)
        {
            return;
        }

        var asset = new Asset
        {
            AssetType = assetType,
            StoreKey = createAssetRequest.StoreKey,
            EncryptionKey = encryptionKey,
            ParentJobExecution = parentJobExecution,
            MediaType = createAssetRequest.MediaType,
            CreatedAt = _clock.GetCurrentInstant()
        };

        foreach (var (key, value) in createAssetRequest.CreateAssetTagRequest)
        {
            asset.UpdateAssetTag(key, value);
        }

        await _context.AddAsync(asset);
        await _context.SaveChangesAsync();
    }

    private async Task<string> GetPresignedDownloadUrl(Asset asset)
    {
        return await _objectStorageService.GetPresignedDownloadUrl(asset.StoreKey);
    }

    public async Task<GetAssetResponse?> GetAsset(long id)
    {
        var asset = await _context.Assets.FindAsync(id);
        if (asset == null)
        {
            return null;
        }

        return new GetAssetResponse(asset, await GetPresignedDownloadUrl(asset));
    }
}