using System;
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
    public void UpdateAssetTag()
    {
        var context = CreateDbContext();
        var service = new AssetMetadataService(context);
        var assetType = context.Add(new AssetType
        {
            Key = "k",
            Name = "n"
        }).Entity;
        var asset = context.Add(new Asset()).Entity;
        context.SaveChanges();
        Assert.Empty(context.AssetTags);
        Assert.Throws<ArgumentNullException>(() => service.UpdateAssetTag(asset, null, null));
        Assert.Throws<ArgumentNullException>(() => service.UpdateAssetTag(asset, null, "value"));
        service.UpdateAssetTag(asset, "key", null);
        Assert.Empty(context.AssetTags);
        var tag = service.UpdateAssetTag(asset, "key", "value");
        Assert.Single(context.AssetTags);
        Assert.Equal("key", tag.Key);
        Assert.Equal("value", tag.Value);
        Assert.Null(service.UpdateAssetTag(asset, "key", null));
        Assert.Empty(context.AssetTags);
        Assert.Equal("1", service.UpdateAssetTag(asset, "key", "1").Value);
        Assert.Single(context.AssetTags);
        Assert.Equal("2", service.UpdateAssetTag(asset, "key", "2").Value);
        Assert.Single(context.AssetTags);
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

        Assert.Equal(6, context.AssetTags.Count());
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