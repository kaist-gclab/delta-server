using System.Linq;
using Delta.AppServer.Assets;
using NodaTime;

namespace Delta.AppServer.Processors
{
    public class ProcessorService
    {
        private readonly DeltaContext _context;
        private readonly IClock _clock;
        private readonly AssetService _assetService;

        public ProcessorService(DeltaContext context, IClock clock, AssetService assetService)
        {
            _context = context;
            _clock = clock;
            _assetService = assetService;
        }

        public ProcessorNode GetNode(long id) => _context.ProcessorNodes.Find(id);

        public ProcessorNode AddNode(RegisterProcessorNodeRequest request)
        {
            var processorType = GetProcessorType(request.ProcessorTypeKey);
            var processorVersion = RegisterProcessorVersion(processorType, request.ProcessorVersionKey,
                request.ProcessorVersionDescription);
            foreach (var c in request.InputCapabilities)
            {
                var assetFormat = _assetService.GetAssetFormat(c.AssetFormatKey);
                var assetType = _assetService.GetAssetType(c.AssetTypeKey);
                RegisterProcessorVersionInputCapability(processorVersion, assetFormat, assetType);
            }

            var node = new ProcessorNode
            {
                Key = request.ProcessorNodeKey,
                Name = request.ProcessorNodeName,
                ProcessorVersion = processorVersion
            };
            node = _context.Add(node).Entity;
            AddNodeStatus(node, PredefinedProcessorNodeStatuses.Available);
            return node;
        }

        public ProcessorNodeStatus AddNodeStatus(ProcessorNode processorNode, string status)
        {
            var nodeStatus = new ProcessorNodeStatus
            {
                Timestamp = _clock.GetCurrentInstant(),
                Status = status
            };
            processorNode.ProcessorNodeStatuses.Add(nodeStatus);
            _context.SaveChanges();
            return nodeStatus;
        }

        public ProcessorVersion RegisterProcessorVersion(
            ProcessorType processorType, string key, string description)
        {
            using var trx = _context.Database.BeginTransaction();
            var q = from v in _context.ProcessorVersions
                    where v.ProcessorType == processorType && v.Key == key
                    select v;
            if (q.Any())
            {
                return q.First();
            }

            var processorVersion = new ProcessorVersion
            {
                ProcessorType = processorType,
                Key = key,
                Description = description,
                CreatedAt = _clock.GetCurrentInstant()
            };
            processorVersion = _context.Add(processorVersion).Entity;
            _context.SaveChanges();
            trx.Commit();
            return processorVersion;
        }
        public ProcessorVersionInputCapability RegisterProcessorVersionInputCapability(
            ProcessorVersion processorVersion, AssetFormat assetFormat, AssetType assetType)
        {
            using var trx = _context.Database.BeginTransaction();
            var q = from c in _context.ProcessorVersionInputCapabilities
                    where c.ProcessorVersion == processorVersion &&
                          c.AssertFormat == assetFormat && c.AssertType == assetType
                    select c;
            if (q.Any())
            {
                return q.First();
            }

            var processorVersionInputCapability = new ProcessorVersionInputCapability
            {
                ProcessorVersion = processorVersion,
                AssertFormat = assetFormat,
                AssertType = assetType
            };
            processorVersionInputCapability = _context.Add(processorVersionInputCapability).Entity;
            _context.SaveChanges();
            trx.Commit();
            return processorVersionInputCapability;
        }
    }
}