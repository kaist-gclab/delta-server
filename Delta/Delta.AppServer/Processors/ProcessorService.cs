using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

    public async Task<ProcessorNode?> RegisterProcessorNode(RegisterProcessorNodeRequest request)
    {
        await using var trx = await _context.Database.BeginTransactionAsync();

        var node = await (from n in _context.ProcessorNodes
            where n.Key == request.Key
            select n).FirstOrDefaultAsync();

        if (node == null)
        {
            node = new ProcessorNode { Key = request.Key };
            await _context.AddAsync(node);
        }

        var capabilities = new List<CreateProcessorNodeCapability>();
        foreach (var cap in request.Capabilities)
        {
            var (jobTypeId, assetTypeId, mediaType) = cap;
            var jobType = await _context.JobTypes.FindAsync(jobTypeId);
            var assetType = assetTypeId == null ? null : await _context.AssetTypes.FindAsync(assetTypeId);
            if (jobType == null)
            {
                return null;
            }

            capabilities.Add(new CreateProcessorNodeCapability(jobType, assetType, mediaType));
        }

        node.UpdateCapabilities(capabilities);

        node.AddNodeStatus(_clock.GetCurrentInstant(),
            PredefinedProcessorNodeStatuses.Available);

        await _context.SaveChangesAsync();
        await trx.CommitAsync();
        return node;
    }

    public async Task<IEnumerable<ProcessorNode>> GetProcessorNodes() => await _context.ProcessorNodes.ToListAsync();
}