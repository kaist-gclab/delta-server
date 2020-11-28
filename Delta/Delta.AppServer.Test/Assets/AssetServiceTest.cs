using System;
using Delta.AppServer.Assets;
using Delta.AppServer.ObjectStorage;
using Delta.AppServer.Encryption;
using Delta.AppServer.Test.Infrastructure;
using NodaTime;
using NodaTime.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Delta.AppServer.Test.Assets
{
    public class AssetServiceTest : ServiceTest
    {
        public AssetServiceTest(ITestOutputHelper output) : base(output)
        {
        }

        /*
        [Fact]
        public async void AddAsset()
        {
            var context = CreateDbContext();
            var clock = new FakeClock(Instant.FromUtc(2010, 8, 15, 23, 30));
            var encryptionService = new EncryptionService(context);
            var objectStorage = new MemoryObjectStorageService();
            var service = new AssetService(context, clock, objectStorage, encryptionService, new CompressionService());
        
            var assetFormat = new AssetFormat
            {
                Key = "format-a",
                Name = "Format A",
                Description = "Format a."
            };
            var assetType = new AssetType
            {
                Key = "type-a",
                Name = "Type A"
            };
            var content = new byte[] {0x12, 0x34, 0x56, 0x78, 0xAB};
            var encryptionKey = new EncryptionKey
            {
                Name = "Key a",
                Value = "a",
                Enabled = false
            };
            await Assert.ThrowsAsync<Exception>(async () =>
                await service.AddAsset(assetFormat, assetType, content, encryptionKey, null));
            encryptionKey.Enabled = true;
            await service.AddAsset(assetFormat, assetType, content, encryptionKey, null);
            Assert.Single(objectStorage.Storage);
            var asset = await service.AddAsset(assetFormat, assetType, content, null, null);
            Assert.Equal(2, objectStorage.Storage.Count);
            Assert.Equal("Format A", asset.AssetFormat.Name);
            Assert.Equal("Format a.", asset.AssetFormat.Description);
            Assert.Equal("Type A", asset.AssetType.Name);
            Assert.Equal(clock.GetCurrentInstant(), asset.CreatedAt);
            Assert.Null(asset.ParentJobExecution);
        }
        */
        
        /*
        [Fact]
        public async void ReadAssetContent()
        {
            var context = CreateDbContext();
            var clock = new FakeClock(Instant.FromUtc(2010, 8, 15, 23, 30));
            var encryptionService = new EncryptionService(context);
            var objectStorage = new MemoryObjectStorageService();
            var service = new AssetService(context, clock, objectStorage, encryptionService, new CompressionService());
        
            var assetFormat = new AssetFormat
            {
                Key = "format-a",
                Name = "Format A",
                Description = "Format a."
            };
            var assetType = new AssetType
            {
                Key = "type-a",
                Name = "Type A"
            };
            var content = new byte[] {0x12, 0x34, 0x56, 0x78, 0xAB};
            var encryptionKey = new EncryptionKey
            {
                Name = "Key a",
                Value = "a",
                Enabled = false
            };
            await Assert.ThrowsAsync<Exception>(async () =>
                await service.AddAsset(assetFormat, assetType, content, encryptionKey, null));
            encryptionKey.Enabled = true;
            var asset = await service.AddAsset(assetFormat, assetType, content, encryptionKey, null);
            var contentRead = await service.ReadAssetContent(asset);
            Assert.Equal(5, contentRead.Length);
            Assert.Equal(content, contentRead);
            Assert.NotEqual(content, await objectStorage.Read(asset.StoreKey));
        }
        */
    }
}