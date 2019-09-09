using NodaTime;

namespace Delta.AppServer.Assets
{
    public class AssetService
    {
        private readonly DeltaContext _context;
        private readonly IClock _clock;

        public AssetService(DeltaContext context, IClock clock)
        {
            _context = context;
            _clock = clock;
        }
    }
}