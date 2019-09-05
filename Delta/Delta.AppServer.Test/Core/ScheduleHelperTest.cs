using Delta.AppServer.Core.Schedule;
using NodaTime;
using NodaTime.Testing;
using Xunit;

namespace Delta.AppServer.Test.Core
{
    public class ScheduleHelperTest
    {
        [Fact]
        public void ComputeNext()
        {
            TestSeconds(1540221473, Duration.FromHours(1), 1540224000);
            TestSeconds(1540221473, Duration.FromMinutes(30), 1540222200);
            TestSeconds(1540221473, Duration.FromSeconds(1), 1540221474);
        }

        private static void TestSeconds(long current, Duration interval, long next)
        {
            TestComputeNext(Instant.FromUnixTimeSeconds(current), interval, Instant.FromUnixTimeSeconds(next));
        }

        private static void TestComputeNext(Instant current, Duration interval, Instant next)
        {
            var helper = new ScheduleHelper(new FakeClock(current), DateTimeZoneProviders.Tzdb["Asia/Seoul"]);
            var actual = helper.ComputeNext(interval);
            Assert.Equal(next, actual);
        }
    }
}