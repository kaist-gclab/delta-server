using System.Threading.Tasks;
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

    public async Task<Asset> AddAsset(CreateAssetRequest createAssetRequest)
    {
        var assetType = await _context.FindAsync<AssetType>(createAssetRequest.AssetTypeId);
        if (assetType == null)
        {
            return;
        }
        var asset = new Asset
        {
            StoreKey = createAssetRequest.StoreKey, //
            AssetType = assetType,
            EncryptionKeyId = createAssetRequest.EncryptionKeyId,
            ParentJobExecutionId = createAssetRequest.ParentJobExecutionId,
            MediaType = createAssetRequest.MediaType,
            CreatedAt = _clock.GetCurrentInstant()
        };

        foreach (var (key, value) in createAssetRequest.CreateAssetTagRequest)
        {
            asset.UpdateAssetTag(key, value);
        }

        asset = (await _context.AddAsync(asset)).Entity;
        await _context.SaveChangesAsync();
        return asset;
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