namespace Delta.AppServer.Jobs
{
    public class JobService
    {
        private readonly DeltaContext _context;

        public JobService(DeltaContext context)
        {
            _context = context;
        }
    }
}