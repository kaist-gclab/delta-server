using System.Collections.Generic;
using System.Linq;
using Delta.AppServer.Assets;
using Delta.AppServer.Processors;
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

        /*
        [Fact]
        public void AddNodeStatus()
        {
            var context = CreateDbContext();
            var clock = new FakeClock(Instant.FromUtc(2010, 8, 15, 23, 30));
            var assetMetadataService = new AssetMetadataService(context);
            var service = new ProcessorService(context, clock, assetMetadataService);
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
        
            var s0 = service.AddNodeStatus(a, PredefinedProcessorNodeStatuses.Busy);
            statuses = a.ProcessorNodeStatuses.OrderBy(s => s.Id).ToList();
            Assert.Equal(2, statuses.Count);
            Assert.Equal("Busy", statuses[1].Status);
            Assert.Equal(clock.GetCurrentInstant(), statuses[1].Timestamp);
            Assert.Empty(b.ProcessorNodeStatuses);
        
            Assert.NotEqual(a.Id, b.Id);
            var status = service.AddNodeStatus(service.GetNode(b.Id), PredefinedProcessorNodeStatuses.Down);
            Assert.NotEqual(s0.Id, status.Id);
            Assert.Single(b.ProcessorNodeStatuses);
            Assert.Equal("Down", b.ProcessorNodeStatuses.First().Status);
            Assert.Equal("Down", status.Status);
        }
        */
        
        /*
        [Fact]
        public void AddNode()
        {
            var context = CreateDbContext();
            var clock = new FakeClock(Instant.FromUtc(2010, 8, 15, 23, 30));
            var assetMetadataService = new AssetMetadataService(context);
            var service = new ProcessorService(context, clock, assetMetadataService);
        
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
                    }
            };
            Assert.Empty(service.GetProcessorNodes());
            var node = service.RegisterProcessorNode(request);
            Assert.Single(service.GetProcessorNodes());
            Assert.Single(context.ProcessorNodeStatuses);
            Assert.Equal(PredefinedProcessorNodeStatuses.Available, context.ProcessorNodeStatuses.First().Status);
            Assert.Equal(clock.GetCurrentInstant(), context.ProcessorNodeStatuses.First().Timestamp);
            Assert.Null(node.Name);
            Assert.Equal("node-a", node.Key);
            Assert.Equal(node.Id, service.GetNode(node.Id).Id);
        
            var badRequestA = new RegisterProcessorNodeRequest
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
                            AssetFormatKey = "BAD-ASSET-FORMAT-KEY",
                            AssetTypeKey = assetType.Key
                        }
                    }
            };
            Assert.Null(service.RegisterProcessorNode(badRequestA));
        
            var badRequestB = new RegisterProcessorNodeRequest
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
                            AssetTypeKey = "BAD-ASSET-TYPE-KEY"
                        }
                    }
            };
            Assert.Null(service.RegisterProcessorNode(badRequestB));
        }
        */
        
        /*
        [Fact]
        public void GetProcessorType()
        {
            var context = CreateDbContext();
            var clock = new FakeClock(Instant.FromUtc(2010, 8, 15, 23, 30));
            var assetMetadataService = new AssetMetadataService(context);
            var service = new ProcessorService(context, clock, assetMetadataService);
        
            var processorType = context.Add(new ProcessorType
            {
                Key = "type-a",
                Name = "Type a"
            }).Entity;
            context.SaveChanges();
        
            Assert.Equal("type-a", processorType.Key);
            Assert.Equal(processorType.Key, service.GetProcessorType(processorType.Id).Key);
            Assert.Equal(processorType.Id, service.GetProcessorType(processorType.Key).Id);
        }
        */
        
        /*
        [Fact]
        public void RegisterProcessorVersion()
        {
            var context = CreateDbContext();
            var clock = new FakeClock(Instant.FromUtc(2010, 8, 15, 23, 30));
            var assetMetadataService = new AssetMetadataService(context);
            var service = new ProcessorService(context, clock, assetMetadataService);
        
            var processorType = context.Add(new ProcessorType
            {
                Key = "type-a",
                Name = "Type a"
            }).Entity;
            context.SaveChanges();
        
            Assert.Single(context.ProcessorTypes);
            Assert.Empty(context.ProcessorVersions);
            var processorVersion = service.RegisterProcessorVersion(processorType, "version-key", "version-desc");
            Assert.Equal("version-key", processorVersion.Key);
            Assert.Equal("version-desc", processorVersion.Description);
            Assert.Equal("Type a", processorVersion.ProcessorType.Name);
            Assert.Single(context.ProcessorTypes);
            Assert.Single(context.ProcessorVersions);
            service.RegisterProcessorVersion(processorType, "version-key", "version-desc");
            Assert.Single(context.ProcessorTypes);
            Assert.Single(context.ProcessorVersions);
            service.RegisterProcessorVersion(processorType, "version-key-2", "version-desc");
            Assert.Single(context.ProcessorTypes);
            Assert.Equal(2, context.ProcessorVersions.Count());
        }
        */
        
        /*
        [Fact]
        public void RegisterProcessorVersionInputCapability()
        {
            var context = CreateDbContext();
            var clock = new FakeClock(Instant.FromUtc(2010, 8, 15, 23, 30));
            var assetMetadataService = new AssetMetadataService(context);
            var service = new ProcessorService(context, clock, assetMetadataService);
        
            var processorType = context.Add(new ProcessorType
            {
                Key = "type-a",
                Name = "Type a"
            }).Entity;
            context.SaveChanges();
        
            var processorVersion = service.RegisterProcessorVersion(processorType, "version-key", "version-desc");
            Assert.Single(context.ProcessorTypes);
            Assert.Single(context.ProcessorVersions);
            Assert.Empty(context.ProcessorVersionInputCapabilities);
        
            var assetFormat = new AssetFormat
            {
                Key = "format-a",
                Name = "",
                Description = ""
            };
            var assetType = new AssetType
            {
                Key = "type-a",
                Name = ""
            };
        
            Assert.Single(context.ProcessorTypes);
            Assert.Single(context.ProcessorVersions);
            Assert.Empty(context.ProcessorVersionInputCapabilities);
            Assert.Empty(context.AssetFormats);
            Assert.Empty(context.AssetTypes);
        
            var cap = service.RegisterProcessorVersionInputCapability(processorVersion, assetFormat, assetType);
            Assert.Equal("version-key", cap.ProcessorVersion.Key);
        
            Assert.Single(context.ProcessorTypes);
            Assert.Single(context.ProcessorVersions);
            Assert.Single(context.ProcessorVersionInputCapabilities);
            Assert.Single(context.AssetFormats);
            Assert.Single(context.AssetTypes);
        
            service.RegisterProcessorVersionInputCapability(processorVersion, assetFormat, assetType);
            service.RegisterProcessorVersionInputCapability(processorVersion, assetFormat, assetType);
        
            Assert.Single(context.ProcessorTypes);
            Assert.Single(context.ProcessorVersions);
            Assert.Single(context.ProcessorVersionInputCapabilities);
            Assert.Single(context.AssetFormats);
            Assert.Single(context.AssetTypes);
        
            service.RegisterProcessorVersionInputCapability(processorVersion, null, null);
            service.RegisterProcessorVersionInputCapability(processorVersion, assetFormat, null);
            service.RegisterProcessorVersionInputCapability(processorVersion, null, assetType);
            service.RegisterProcessorVersionInputCapability(processorVersion, null, null);
            service.RegisterProcessorVersionInputCapability(processorVersion, assetFormat, null);
            service.RegisterProcessorVersionInputCapability(processorVersion, null, assetType);
        
            Assert.Single(context.ProcessorTypes);
            Assert.Single(context.ProcessorVersions);
            Assert.Equal(4, context.ProcessorVersionInputCapabilities.Count());
            Assert.Single(context.AssetFormats);
            Assert.Single(context.AssetTypes);
        }
        */
    }
}