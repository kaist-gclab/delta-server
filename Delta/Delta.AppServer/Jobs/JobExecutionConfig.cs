using NodaTime;

namespace Delta.AppServer.Jobs
{
    public class JobExecutionConfig
    {
        public Duration JobExecutionTimeout => Duration.FromSeconds(30);
    }
}