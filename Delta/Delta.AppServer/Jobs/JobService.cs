using NodaTime;

namespace Delta.AppServer.Jobs
{
    public class JobService
    {
        private readonly DeltaContext _context;
        private readonly IClock _clock;

        public JobService(DeltaContext context, IClock clock)
        {
            _context = context;
            _clock = clock;
        }
        public Job GetJob(long id)
        {
            return _context.Jobs.Find(id);
        }
    }
}