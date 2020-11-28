using System;
using System.Collections.Generic;
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

        public ProcessorNode RegisterProcessorNode(RegisterProcessorNodeRequest request)
        {
            using var trx = _context.Database.BeginTransaction();
            var node = (from n in _context.ProcessorNodes
                where n.Key == request.Key
                select n).FirstOrDefault();

            if (node == null)
            {
                node = _context.Add(new ProcessorNode {Key = request.Key}).Entity;
            }

            node.UpdateCapabilities(request.Capabilities);
            node.AddNodeStatus(_clock.GetCurrentInstant(),
                PredefinedProcessorNodeStatuses.Available);
            _context.SaveChanges();
            trx.Commit();
            return node;
        }

        public IEnumerable<ProcessorNode> GetProcessorNodes() => _context.ProcessorNodes.ToList();
    }
}