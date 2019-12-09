using System.Linq;
using Delta.AppServer.Assets;
using NodaTime;

namespace Delta.AppServer.Processors
{
    public class ProcessorService
    {
        private readonly DeltaContext _context;
        private readonly IClock _clock;
        private readonly AssetMetadataService _assetMetadataService;

        public ProcessorService(DeltaContext context, IClock clock, AssetMetadataService assetMetadataService)
        {
            _context = context;
            _clock = clock;
            _assetMetadataService = assetMetadataService;
        }

        public ProcessorNode GetNode(long id) => _context.ProcessorNodes.Find(id);

        public ProcessorNode AddNode(ProcessorVersion processorVersion,
            string processorNodeKey, string processorNodeName)
        {
            var node = new ProcessorNode
            {
                Key = processorNodeKey,
                Name = processorNodeName,
                ProcessorVersion = processorVersion
            };
            node = _context.Add(node).Entity;
            AddNodeStatus(node, PredefinedProcessorNodeStatuses.Available);
            return node;
        }

        public ProcessorNode AddNode(RegisterProcessorNodeRequest request)
        {
            var processorType = GetProcessorType(request.ProcessorTypeKey);
            var processorVersion = RegisterProcessorVersion(processorType, request.ProcessorVersionKey,
                request.ProcessorVersionDescription);
            foreach (var c in request.InputCapabilities)
            {
                var assetFormat = _assetMetadataService.GetAssetFormat(c.AssetFormatKey);
                var assetType = _assetMetadataService.GetAssetType(c.AssetTypeKey);
                RegisterProcessorVersionInputCapability(processorVersion, assetFormat, assetType);
            }

            return AddNode(processorVersion, request.ProcessorNodeKey, request.ProcessorNodeName);
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

        public ProcessorType GetProcessorType(long id) => _context.ProcessorTypes.Find(id);

        public ProcessorType GetProcessorType(string key)
        {
            return (from t in _context.ProcessorTypes
                    where t.Key == key
                    select t).First();
        }

        public ProcessorVersionInputCapability RegisterProcessorVersionInputCapability(
            ProcessorVersion processorVersion, AssetFormat assetFormat, AssetType assetType)
        {
            using var trx = _context.Database.BeginTransaction();
            var q = from c in _context.ProcessorVersionInputCapabilities
                    where c.ProcessorVersion == processorVersion &&
                          c.AssetFormat == assetFormat && c.AssetType == assetType
                    select c;
            if (q.Any())
            {
                return q.First();
            }

            var processorVersionInputCapability = new ProcessorVersionInputCapability
            {
                ProcessorVersion = processorVersion,
                AssetFormat = assetFormat,
                AssetType = assetType
            };
            processorVersionInputCapability = _context.Add(processorVersionInputCapability).Entity;
            _context.SaveChanges();
            trx.Commit();
            return processorVersionInputCapability;
        }
    }
}