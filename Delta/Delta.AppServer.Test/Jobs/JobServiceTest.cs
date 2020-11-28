using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delta.AppServer.Assets;
using Delta.AppServer.Jobs;
using Delta.AppServer.ObjectStorage;
using Delta.AppServer.Processors;
using Delta.AppServer.Encryption;
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

        /*
        [Fact]
        public void AddJob()
        {
            var context = CreateDbContext();
            var clock = new FakeClock(Instant.FromUtc(2010, 5, 15, 23, 30));
            var service = new JobService(context, clock);
        
            var processorType = context.Add(new ProcessorType {Key = "type-key", Name = "type-name"}).Entity;
            context.SaveChanges();
            var processorService = new ProcessorService(context, clock, null);
            var processorVersion = processorService.RegisterProcessorVersion(processorType, "key", "");
            processorVersion.ProcessorVersionInputCapabilities.Add(new ProcessorVersionInputCapability());
        
            Assert.Throws<Exception>(() => service.CreateJob(null, null, null));
            Assert.Throws<Exception>(() => service.CreateJob(null, processorVersion, null));
            service.CreateJob(null, processorVersion, "");
            Assert.Single(context.Jobs);
        }
        */
        
        /*
        [Theory]
        [InlineData("nn", "null", true)]
        [InlineData("nn", "ab", true)]
        [InlineData("nn", "ax", true)]
        [InlineData("nn", "xb", true)]
        [InlineData("nn", "xx", true)]
        [InlineData("nb", "null", false)]
        [InlineData("nb", "ab", true)]
        [InlineData("nb", "ax", false)]
        [InlineData("nb", "xb", true)]
        [InlineData("nb", "xx", false)]
        [InlineData("an", "null", false)]
        [InlineData("an", "ab", true)]
        [InlineData("an", "ax", true)]
        [InlineData("an", "xb", false)]
        [InlineData("an", "xx", false)]
        [InlineData("ab", "null", false)]
        [InlineData("ab", "ab", true)]
        [InlineData("ab", "ax", false)]
        [InlineData("ab", "xb", false)]
        [InlineData("ab", "xx", false)]
        public void ValidateCompatibility(string versionFormatType, string assetFormatType, bool expect)
        {
            var versionFormat = versionFormatType.Substring(0, 1);
            var versionType = versionFormatType.Substring(1, 1);
        
            var isAssetNull = assetFormatType == "null";
            var assetFormat = assetFormatType.Substring(0, 1);
            var assetType = assetFormatType.Substring(1, 1);
        
            var context = CreateDbContext();
            var clock = new FakeClock(Instant.FromUtc(2010, 5, 15, 23, 30));
            var assetMetadataService = new AssetMetadataService(context);
            var service = new JobService(context, clock);
        
            AssetFormat CreateAssetFormatIfNotExists(string key)
            {
                if (key == "n")
                {
                    return null;
                }
        
                var f = assetMetadataService.GetAssetFormat(key);
                if (f != null)
                {
                    return f;
                }
        
                context.Add(new AssetFormat {Key = key});
                context.SaveChanges();
                return assetMetadataService.GetAssetFormat(key);
            }
        
            AssetType CreateAssetTypeIfNotExists(string key)
            {
                if (key == "n")
                {
                    return null;
                }
        
                var t = assetMetadataService.GetAssetType(key);
                if (t != null)
                {
                    return t;
                }
        
                context.Add(new AssetType {Key = key});
                context.SaveChanges();
                return assetMetadataService.GetAssetType(key);
            }
        
            var version = new ProcessorVersion
            {
                ProcessorType = null,
                ProcessorVersionInputCapabilities = new[]
                {
                    new ProcessorVersionInputCapability
                    {
                        AssetFormat = CreateAssetFormatIfNotExists(versionFormat),
                        AssetType = CreateAssetTypeIfNotExists(versionType)
                    }
                }
            };
            if (isAssetNull)
            {
                Assert.Equal(expect, service.ValidateCompatibility(null, version));
            }
            else
            {
                var asset = new Asset
                {
                    AssetFormat = CreateAssetFormatIfNotExists(assetFormat),
                    AssetType = CreateAssetTypeIfNotExists(assetType)
                };
                Assert.Equal(expect, service.ValidateCompatibility(asset, version));
            }
        }
        */
        
        /*
        [Fact]
        public void ScheduleNextJob()
        {
            var context = CreateDbContext();
            var clock = new FakeClock(Instant.FromUtc(2010, 5, 15, 23, 30));
            var service = new JobService(context, clock);
        
            var processorType = context.Add(new ProcessorType {Key = "type-key", Name = "type-name"}).Entity;
            context.SaveChanges();
            var processorService = new ProcessorService(context, clock, null);
            var processorVersion = processorService.RegisterProcessorVersion(processorType, "key", "");
        
            var node = processorService.AddNode(processorVersion, "a", "");
            processorVersion.ProcessorVersionInputCapabilities.Add(new ProcessorVersionInputCapability());
            var job = service.CreateJob(null, processorVersion, "JOB_ARGUMENTS");
            Assert.Empty(job.JobExecutions);
            var execution = service.ScheduleNextJob(node);
            Assert.Single(job.JobExecutions);
            Assert.Equal(job, execution.Job);
            Assert.Equal(node, execution.ProcessorNode);
            Assert.Equal("JOB_ARGUMENTS", job.JobArguments);
        }
        */
        
        /*
        [Fact]
        public async void AddJobResult()
        {
            var context = CreateDbContext();
            var clock = new FakeClock(Instant.FromUtc(2010, 5, 15, 23, 30));
            var objectStorageService = new MemoryObjectStorageService();
            var encryptionService = new EncryptionService(context);
            var assetService = new AssetService(context, clock, objectStorageService, encryptionService,
                new CompressionService());
            var assetMetadataService = new AssetMetadataService(context);
            var service = new JobService(context, clock);
        
            var processorType = context.Add(new ProcessorType {Key = "type-key", Name = "type-name"}).Entity;
            context.SaveChanges();
            var processorService = new ProcessorService(context, clock, null);
            var processorVersion = processorService.RegisterProcessorVersion(processorType, "key", "");
            processorVersion.ProcessorVersionInputCapabilities.Add(new ProcessorVersionInputCapability());
        
            var node = processorService.AddNode(processorVersion, "a", "");
            var job = service.CreateJob(null, processorVersion, "");
            Assert.Equal(job.Id, service.GetJob(job.Id).Id);
        
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
            Assert.NotNull(assetMetadataService.GetAsset(1));
            Assert.NotNull(assetMetadataService.GetAsset(2));
            Assert.Equal(2, context.Assets.Count());
        
            // TODO type, format 조합
            // TODO 암호화 키 상속
        }
        */
    }
}