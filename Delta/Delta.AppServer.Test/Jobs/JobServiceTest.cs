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
            
            // TODO 에셋 호환성 검증 동작 시험
        }

        [Fact]
        public void ScheduleNextJob()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void AddJobResult()
        {
            throw new NotImplementedException();
        }
    }
}