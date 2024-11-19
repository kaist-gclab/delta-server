using System.Linq;
using System.Threading.Tasks;
using Delta.AppServer.Assets;
using Delta.AppServer.Test.Infrastructure;
using Xunit;
using Xunit.Abstractions;

namespace Delta.AppServer.Test.Assets;

public class AssetMetadataServiceTest : ServiceTest
{
    public AssetMetadataServiceTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task UpdateAssetTag()
    {
        var context = CreateDbContext();
        var service = new AssetMetadataService(context);
        var assetType = context.Add(new AssetType
        {
            Key = "k",
            Name = "n"
        }).Entity;
        var asset = context.Add(new Asset
        {
            MediaType = "text/plain",
            StoreKey = "a",
            CreatedAt = default,
            AssetType = assetType,
            ParentJobExecution = null
        }).Entity;
        await context.SaveChangesAsync();

        Assert.Empty(context.AssetTag);
        await service.UpdateAssetTag(asset.Id, "key", "value");
        Assert.Single(context.AssetTag);
        var tag = context.AssetTag.First();
        Assert.Equal("key", tag.Key);
        Assert.Equal("value", tag.Value);
        await service.UpdateAssetTag(asset.Id, "key", "1");
        tag = context.AssetTag.First();
        Assert.Equal("1", tag.Value);
        Assert.Single(context.AssetTag);
        await service.UpdateAssetTag(asset.Id, "key", "2");
        tag = context.AssetTag.First();
        Assert.Equal("2", tag.Value);
        Assert.Single(context.AssetTag);
    }

    [Fact]
    public async Task FindByAssetTag()
    {
        var context = CreateDbContext();
        var service = new AssetMetadataService(context);
        var assetType = context.Add(new AssetType
        {
            Key = "k",
            Name = "n"
        }).Entity;
        var assetA = context.Add(new Asset
        {
            MediaType = "text/plain",
            StoreKey = "a",
            CreatedAt = default,
            AssetType = assetType,
            ParentJobExecution = null
        }).Entity;
        var assetB = context.Add(new Asset
        {
            MediaType = "text/plain",
            StoreKey = "b",
            CreatedAt = default,
            AssetType = assetType,
            ParentJobExecution = null
        }).Entity;
        await context.SaveChangesAsync();

        await service.UpdateAssetTag(assetA.Id, "a", "b");
        await service.UpdateAssetTag(assetA.Id, "z", "b");
        await service.UpdateAssetTag(assetA.Id, "c", "d");
        await service.UpdateAssetTag(assetB.Id, "a", "b");
        await service.UpdateAssetTag(assetB.Id, "e", "f");
        await service.UpdateAssetTag(assetB.Id, "z", "p");

        Assert.Equal(6, context.AssetTag.Count());
        Assert.Equal(2, (await service.FindByAssetTag("a", null)).Count());
        Assert.Equal(2, (await service.FindByAssetTag("a", "b")).Count());
        Assert.Equal(2, (await service.FindByAssetTag(null, "b")).Count());
        Assert.Equal(2, (await service.FindByAssetTag("z", null)).Count());
        Assert.Single(await service.FindByAssetTag("z", "b"));
        Assert.Single(await service.FindByAssetTag("z", "p"));
        Assert.Single(await service.FindByAssetTag("e", "f"));
        Assert.Empty(await service.FindByAssetTag("k", "f"));
        Assert.Empty(await service.FindByAssetTag(null, null));
    }
}