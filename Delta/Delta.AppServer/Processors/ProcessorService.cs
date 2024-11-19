using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace Delta.AppServer.Processors;

public class ProcessorService(DeltaContext context, IClock clock)
{
    public async Task<ProcessorNode?> GetNode(long id) => await context.ProcessorNode.FindAsync(id);

    public async Task<ProcessorNode?> RegisterProcessorNode(RegisterProcessorNodeRequest request)
    {
        await using var trx = await context.Database.BeginTransactionAsync();

        var node = await (from n in context.ProcessorNode
            where n.Key == request.Key
            select n).FirstOrDefaultAsync();

        if (node == null)
        {
            node = new ProcessorNode { Key = request.Key };
            await context.AddAsync(node);
        }

        var capabilities = new List<CreateProcessorNodeCapability>();
        foreach (var cap in request.Capabilities)
        {
            var (jobTypeId, assetTypeId, mediaType) = cap;
            var jobType = await context.JobType.FindAsync(jobTypeId);
            if (jobType == null)
            {
                return null;
            }

            capabilities.Add(new CreateProcessorNodeCapability(jobType, mediaType));
        }

        node.UpdateCapabilities(capabilities);

        node.AddNodeStatus(clock.GetCurrentInstant(),
            PredefinedProcessorNodeStatuses.Available);

        await context.SaveChangesAsync();
        await trx.CommitAsync();
        return node;
    }

    public async Task<IEnumerable<ProcessorNode>> GetProcessorNodes() => await context.ProcessorNode.ToListAsync();
}