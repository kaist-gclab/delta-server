using System;
using Delta.AppServer.Jobs;
using Delta.AppServer.Processors;
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
            var service = new JobService(context, clock);

            var processorType = context.Add(new ProcessorType {Key = "type-key", Name = "type-name",}).Entity;
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
            var service = new JobService(context, clock);

            var processorType = context.Add(new ProcessorType {Key = "type-key", Name = "type-name",}).Entity;
            context.SaveChanges();
            var processorService = new ProcessorService(context, clock, null);
            var processorVersion = processorService.RegisterProcessorVersion(processorType, "key", "");

            var node = processorService.AddNode(processorVersion, "a", "");
            var job = service.AddJob(null, processorVersion, "");
            var execution = service.ScheduleNextJob(node);
            Assert.Equal(job, execution.Job);
            Assert.Equal(node, execution.ProcessorNode);
        }

        [Fact]
        public void AddJobResult()
        {
            var context = CreateDbContext();
            var clock = new FakeClock(Instant.FromUtc(2010, 5, 15, 23, 30));
            var service = new JobService(context, clock);

            var processorType = context.Add(new ProcessorType {Key = "type-key", Name = "type-name",}).Entity;
            context.SaveChanges();
            var processorService = new ProcessorService(context, clock, null);
            var processorVersion = processorService.RegisterProcessorVersion(processorType, "key", "");

            var node = processorService.AddNode(processorVersion, "a", "");
            service.AddJob(null, processorVersion, "");
            var execution = service.ScheduleNextJob(node);
            // TODO
            // format, type, content
            // 1. 완전한 새 에셋 추가 시나리오
            // 2. 결과 에셋 추가 및 암호화 키 상속 시나리오
//            service.AddJobResult(execution, )
        }
    }
}