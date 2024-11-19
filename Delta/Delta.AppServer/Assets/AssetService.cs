using System.Threading.Tasks;
using Delta.AppServer.Encryption;
using Delta.AppServer.Jobs;
using Delta.AppServer.ObjectStorage;
using NodaTime;

namespace Delta.AppServer.Assets;

public class AssetService(DeltaContext context, IClock clock, IObjectStorageService objectStorageService)
{
    public async Task AddAsset(CreateAssetRequest createAssetRequest)
    {
        var parentJobExecution = createAssetRequest.ParentJobExecutionId == null
            ? null
            : await context.FindAsync<JobExecution>(createAssetRequest.ParentJobExecutionId);
        if (createAssetRequest.ParentJobExecutionId != null && parentJobExecution == null)
        {
            return;
        }

        var encryptionKey = createAssetRequest.EncryptionKeyId == null
            ? null
            : await context.FindAsync<EncryptionKey>(createAssetRequest.EncryptionKeyId);

        if (createAssetRequest.EncryptionKeyId != null && encryptionKey == null)
        {
            return;
        }

        var asset = new Asset
        {
            StoreKey = createAssetRequest.StoreKey,
            CreatedAt = clock.GetCurrentInstant()
        };

        foreach (var (key, value) in createAssetRequest.CreateAssetTagRequest)
        {
            asset.UpdateAssetTag(key, value);
        }

        await context.AddAsync(asset);
        await context.SaveChangesAsync();
    }

    private async Task<string> GetPresignedDownloadUrl(Asset asset)
    {
        return await objectStorageService.GetPresignedDownloadUrl(asset.StoreKey);
    }

    public async Task<GetAssetResponse?> GetAsset(long id)
    {
        var asset = await context.Asset.FindAsync(id);
        if (asset == null)
        {
            return null;
        }

        return new GetAssetResponse(asset, await GetPresignedDownloadUrl(asset));
    }
}