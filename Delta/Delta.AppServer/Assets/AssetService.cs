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
        var asset = new Asset
        {
            AssetTypeId = createAssetRequest.AssetTypeId,
            StoreKey = createAssetRequest.StoreKey, //
            EncryptionKeyId = createAssetRequest.EncryptionKeyId,
            ParentJobExecutionId = createAssetRequest.ParentJobExecutionId,
            MediaType = createAssetRequest.MediaType,
            CreatedAt = _clock.GetCurrentInstant()
        };

        foreach (var t in createAssetRequest.CreateAssetTagRequest)
        {
            asset.UpdateAssetTag(t.Key, t.Value);
        }

        asset = (await _context.AddAsync(asset)).Entity;
        await _context.SaveChangesAsync();
        return asset;
    }

    private async Task<string> GetPresignedDownloadUrl(Asset asset)
    {
        return await _objectStorageService.GetPresignedDownloadUrl(asset.StoreKey);
    }

    public async Task<GetAssetResponse> GetAsset(long id)
    {
        var asset = _context.Assets.Find(id);
        if (asset == null)
        {
            return null;
        }

        return new GetAssetResponse(asset, await GetPresignedDownloadUrl(asset));
    }
}