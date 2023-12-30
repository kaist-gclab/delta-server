using System;
using System.Linq;
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

    /*
    [Fact]
    public void UpdateAssetTag()
    {
        var context = CreateDbContext();
        var service = new AssetMetadataService(context);
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
    */
    
    [Fact]
    public void FindByAssetTag()
    {
        var context = CreateDbContext();
        var service = new AssetMetadataService(context);
        var assetA = context.Add(new Asset()).Entity;
        var assetB = context.Add(new Asset()).Entity;
        context.SaveChanges();
        service.UpdateAssetTag(assetA, "a", "b");
        service.UpdateAssetTag(assetA, "z", "b");
        service.UpdateAssetTag(assetA, "c", "d");
        service.UpdateAssetTag(assetB, "a", "b");
        service.UpdateAssetTag(assetB, "e", "f");
        service.UpdateAssetTag(assetB, "z", "p");
    
        Assert.Equal(6, context.AssetTags.Count());
        Assert.Equal(2, service.FindByAssetTag("a", null).Count());
        Assert.Equal(2, service.FindByAssetTag("a", "b").Count());
        Assert.Equal(2, service.FindByAssetTag(null, "b").Count());
        Assert.Equal(2, service.FindByAssetTag("z", null).Count());
        Assert.Single(service.FindByAssetTag("z", "b"));
        Assert.Single(service.FindByAssetTag("z", "p"));
        Assert.Single(service.FindByAssetTag("e", "f"));
        Assert.Empty(service.FindByAssetTag("k", "f"));
        Assert.Throws<ArgumentNullException>(() => service.FindByAssetTag(null, null));
    }
}