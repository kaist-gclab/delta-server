using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delta.AppServer.Assets;
using Delta.AppServer.Jobs;
using Delta.AppServer.ObjectStorage;
using Delta.AppServer.Processors;
using Delta.AppServer.Security;
using Delta.AppServer.Test.Infrastructure;
using NodaTime;
using NodaTime.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Delta.AppServer.Test.Jobs
{
    public class JobServiceTest : ServiceTest
    {
        public JobServiceTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void AddJob()
        {
            var context = CreateDbContext();
            var clock = new FakeClock(Instant.FromUtc(2010, 5, 15, 23, 30));
            var service = new JobService(context, clock, null);

            var processorType = context.Add(new ProcessorType {Key = "type-key", Name = "type-name"}).Entity;
            context.SaveChanges();
            var processorService = new ProcessorService(context, clock, null);
            var processorVersion = processorService.RegisterProcessorVersion(processorType, "key", "");

            Assert.Throws<Exception>(() => service.AddJob(null, null, null));
            Assert.Throws<Exception>(() => service.AddJob(null, processorVersion, null));
            service.AddJob(null, processorVersion, "");
            Assert.Single(context.Jobs);
            // TODO 에셋 호환성 검증 동작 시험
        }

        [Fact]
        public void ScheduleNextJob()
        {
            var context = CreateDbContext();
            var clock = new FakeClock(Instant.FromUtc(2010, 5, 15, 23, 30));
            var service = new JobService(context, clock, null);

            var processorType = context.Add(new ProcessorType {Key = "type-key", Name = "type-name"}).Entity;
            context.SaveChanges();
            var processorService = new ProcessorService(context, clock, null);
            var processorVersion = processorService.RegisterProcessorVersion(processorType, "key", "");

            var node = processorService.AddNode(processorVersion, "a", "");
            var job = service.AddJob(null, processorVersion, "JOB_ARGUMENTS");
            Assert.Single(job.JobExecutions);
            var execution = service.ScheduleNextJob(node);
            Assert.Equal(2, job.JobExecutions.Count);
            Assert.Equal(job, execution.Job);
            Assert.Equal(node, execution.ProcessorNode);
            Assert.Equal("JOB_ARGUMENTS", job.JobArguments);
        }

        [Fact]
        public async void AddJobResult()
        {
            var context = CreateDbContext();
            var clock = new FakeClock(Instant.FromUtc(2010, 5, 15, 23, 30));
            var objectStorageService = new MemoryObjectStorageService();
            var encryptionService = new EncryptionService(context, Output.ToLogger<EncryptionService>());
            var assetService = new AssetService(context, clock, objectStorageService, encryptionService);
            var service = new JobService(context, clock, assetService);

            var processorType = context.Add(new ProcessorType {Key = "type-key", Name = "type-name"}).Entity;
            context.SaveChanges();
            var processorService = new ProcessorService(context, clock, null);
            var processorVersion = processorService.RegisterProcessorVersion(processorType, "key", "");

            var node = processorService.AddNode(processorVersion, "a", "");
            service.AddJob(null, processorVersion, "");
            var execution = service.ScheduleNextJob(node);
            Assert.Equal("a", execution.ProcessorNode.Key);

            // TODO assetFormat 및 assetType 유효성 검증
            var status = await service.AddJobResult(execution, new List<ResultAsset>
            {
                new ResultAsset
                {
                    AssetFormat = new AssetFormat
                    {
                        Key = "format-a",
                        Name = "Format A",
                        Description = "Format a."
                    },
                    AssetType = new AssetType
                    {
                        Key = "type-a",
                        Name = "Type A"
                    },
                    Content = Encoding.UTF8.GetBytes("test")
                },
                new ResultAsset
                {
                    AssetFormat = new AssetFormat
                    {
                        Key = "format-b",
                        Name = "Format B",
                        Description = "Format b."
                    },
                    AssetType = new AssetType
                    {
                        Key = "type-b",
                        Name = "Type B"
                    },
                    Content = Encoding.UTF8.GetBytes("테스트")
                }
            });
            Assert.Equal(PredefinedJobExecutionStatuses.Complete, status.Status);
            Assert.NotNull(assetService.GetAsset(1));
            Assert.NotNull(assetService.GetAsset(2));
            Assert.Equal(2, context.Assets.Count());

            // TODO type, format 조합
            // TODO 암호화 키 상속
        }
    }
}