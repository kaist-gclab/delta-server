namespace Delta.AppServer.Assets
{
    public class AssetService
    {
        private readonly DeltaContext _context;

        public AssetService(DeltaContext context)
        {
            _context = context;
        }
    }
}