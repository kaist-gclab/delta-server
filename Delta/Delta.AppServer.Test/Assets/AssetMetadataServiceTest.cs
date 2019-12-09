using System;
using System.Linq;
using Delta.AppServer.Assets;
using Delta.AppServer.Test.Infrastructure;
using Xunit;
using Xunit.Abstractions;

namespace Delta.AppServer.Test.Assets
{
    public class AssetMetadataServiceTest : ServiceTest
    {
        public AssetMetadataServiceTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void AddAssetTag()
        {
            // TODO 삭제 기능, 이력 관리 기능
            var context = CreateDbContext();
            var service = new AssetMetadataService(context);
            var asset = context.Add(new Asset()).Entity;
            context.SaveChanges();
            Assert.Empty(context.AssetTags);
            Assert.Throws<ArgumentNullException>(() => service.AddAssetTag(asset, null, null));
            Assert.Throws<ArgumentNullException>(() => service.AddAssetTag(asset, "key", null));
            Assert.Throws<ArgumentNullException>(() => service.AddAssetTag(asset, null, "value"));
            Assert.Empty(context.AssetTags);
            var tag = service.AddAssetTag(asset, "key", "value");
            Assert.Single(context.AssetTags);
            Assert.Equal("key", tag.Key);
            Assert.Equal("value", tag.Value);
        }

        [Fact]
        public void FindByAssetTag()
        {
            var context = CreateDbContext();
            var service = new AssetMetadataService(context);
            var assetA = context.Add(new Asset()).Entity;
            var assetB = context.Add(new Asset()).Entity;
            context.SaveChanges();
            service.AddAssetTag(assetA, "a", "b");
            service.AddAssetTag(assetA, "z", "b");
            service.AddAssetTag(assetA, "c", "d");
            service.AddAssetTag(assetB, "a", "b");
            service.AddAssetTag(assetB, "e", "f");
            service.AddAssetTag(assetB, "z", "p");

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
}