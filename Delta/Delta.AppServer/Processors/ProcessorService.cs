namespace Delta.AppServer.Processors
{
    public class ProcessorService
    {
        private readonly DeltaContext _context;

        public ProcessorService(DeltaContext context)
        {
            _context = context;
        }
    }
}