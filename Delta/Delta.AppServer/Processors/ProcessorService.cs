using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NodaTime;

namespace Delta.AppServer.Processors;

public class ProcessorService
{
    private readonly DeltaContext _context;
    private readonly IClock _clock;

    public ProcessorService(DeltaContext context, IClock clock)
    {
        _context = context;
        _clock = clock;
    }

    public async Task<ProcessorNode?> GetNode(long id) => await _context.ProcessorNodes.FindAsync(id);

    public async Task<ProcessorNode> RegisterProcessorNode(RegisterProcessorNodeRequest request)
    {
        using var trx = _context.Database.BeginTransaction();
        var node = (from n in _context.ProcessorNodes
                       where n.Key == request.Key
                       select n).FirstOrDefault() ??
                   _context.Add(new ProcessorNode { Key = request.Key }).Entity;

        node.UpdateCapabilities(request.Capabilities.Select(cap =>
        {
            var (jobTypeId, assetTypeId, mediaType) = cap;
            var jobType = _context.JobTypes.Find(jobTypeId);
            var assetType = _context.AssetTypes.Find(assetTypeId);
            if (jobType == null)
            {
                throw new ArgumentException();
            }

            return new CreateProcessorNodeCapability(jobType, assetType, mediaType);
        }));
        node.AddNodeStatus(_clock.GetCurrentInstant(),
            PredefinedProcessorNodeStatuses.Available);
        _context.SaveChanges();
        trx.Commit();
        return node;
    }

    public IEnumerable<ProcessorNode> GetProcessorNodes() => _context.ProcessorNodes.ToList();
}