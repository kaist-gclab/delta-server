using System;
using System.Collections.Generic;
using System.Linq;
using Delta.AppServer.Assets;
using Delta.AppServer.ObjectStorage;
using Delta.AppServer.Processors;
using Delta.AppServer.Security;
using Delta.AppServer.Test.Infrastructure;
using NodaTime;
using NodaTime.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Delta.AppServer.Test.Processors
{
    public class ProcessorServiceTest : ServiceTest
    {
        public ProcessorServiceTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void AddNodeStatus()
        {
            var context = CreateDbContext();
            var clock = new FakeClock(Instant.FromUtc(2010, 8, 15, 23, 30));
            var encryptionService = new EncryptionService(context, Output.ToLogger<EncryptionService>());
            var assetService = new AssetService(context, clock, new MemoryObjectStorageService(), encryptionService);
            var service = new ProcessorService(context, clock, assetService);
            var a = context.Add(new ProcessorNode()).Entity;
            var b = context.Add(new ProcessorNode()).Entity;
            context.SaveChanges();
            Assert.Empty(a.ProcessorNodeStatuses);
            Assert.Empty(b.ProcessorNodeStatuses);
            service.AddNodeStatus(a, PredefinedProcessorNodeStatuses.Available);

            var statuses = a.ProcessorNodeStatuses.OrderBy(s => s.Id).ToList();
            Assert.Single(statuses);
            Assert.Equal("Available", statuses[0].Status);
            Assert.Equal(clock.GetCurrentInstant(), statuses[0].Timestamp);
            clock.AdvanceDays(1);
            Assert.Empty(b.ProcessorNodeStatuses);

            service.AddNodeStatus(a, PredefinedProcessorNodeStatuses.Busy);
            statuses = a.ProcessorNodeStatuses.OrderBy(s => s.Id).ToList();
            Assert.Equal(2, statuses.Count);
            Assert.Equal("Busy", statuses[1].Status);
            Assert.Equal(clock.GetCurrentInstant(), statuses[1].Timestamp);
            Assert.Empty(b.ProcessorNodeStatuses);

            Assert.NotEqual(a.Id, b.Id);
            service.AddNodeStatus(service.GetNode(b.Id), PredefinedProcessorNodeStatuses.Down);
            Assert.Single(b.ProcessorNodeStatuses);
            Assert.Equal("Down", b.ProcessorNodeStatuses.First().Status);
        }

        [Fact]
        public void AddNode()
        {
            var context = CreateDbContext();
            var clock = new FakeClock(Instant.FromUtc(2010, 8, 15, 23, 30));
            var encryptionService = new EncryptionService(context, Output.ToLogger<EncryptionService>());
            var assetService = new AssetService(context, clock, new MemoryObjectStorageService(), encryptionService);
            var service = new ProcessorService(context, clock, assetService);

            var assetFormat = context.Add(new AssetFormat
            {
                Key = "format-a",
                Name = "",
                Description = ""
            }).Entity;
            var assetType = context.Add(new AssetType
            {
                Key = "type-a",
                Name = ""
            }).Entity;
            var processorType = context.Add(new ProcessorType
            {
                Key = "type-a",
                Name = "Type a"
            }).Entity;
            context.SaveChanges();

            var request = new RegisterProcessorNodeRequest
            {
                ProcessorTypeKey = processorType.Key,
                ProcessorVersionKey = "version-a",
                ProcessorVersionDescription = "Version a.",
                ProcessorNodeKey = "node-a",
                ProcessorNodeName = null,
                InputCapabilities =
                    new List<RegisterProcessorNodeRequest.InputCapability>
                    {
                        new RegisterProcessorNodeRequest.InputCapability
                        {
                            AssetFormatKey = assetFormat.Key,
                            AssetTypeKey = assetType.Key
                        }
                    },
            };
            service.AddNode(request);
            Assert.Single(context.ProcessorNodeStatuses);
            Assert.Equal(PredefinedProcessorNodeStatuses.Available, context.ProcessorNodeStatuses.First().Status);
            Assert.Equal(clock.GetCurrentInstant(), context.ProcessorNodeStatuses.First().Timestamp);
        }

        [Fact]
        public void RegisterProcessorVersion()
        {
            var context = CreateDbContext();
            var clock = new FakeClock(Instant.FromUtc(2010, 8, 15, 23, 30));
            var encryptionService = new EncryptionService(context, Output.ToLogger<EncryptionService>());
            var assetService = new AssetService(context, clock, new MemoryObjectStorageService(), encryptionService);
            var service = new ProcessorService(context, clock, assetService);

            var processorType = context.Add(new ProcessorType
            {
                Key = "type-a",
                Name = "Type a"
            }).Entity;
            context.SaveChanges();

            Assert.Single(context.ProcessorTypes);
            Assert.Empty(context.ProcessorVersions);
            service.RegisterProcessorVersion(processorType, "version-key", "version-desc");
            Assert.Single(context.ProcessorTypes);
            Assert.Single(context.ProcessorVersions);
            service.RegisterProcessorVersion(processorType, "version-key", "version-desc");
            Assert.Single(context.ProcessorTypes);
            Assert.Single(context.ProcessorVersions);
            service.RegisterProcessorVersion(processorType, "version-key-2", "version-desc");
            Assert.Single(context.ProcessorTypes);
            Assert.Equal(2, context.ProcessorVersions.Count());
        }
    }
}