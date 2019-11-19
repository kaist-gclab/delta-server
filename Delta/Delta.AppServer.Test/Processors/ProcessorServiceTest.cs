using System;
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
    }
}