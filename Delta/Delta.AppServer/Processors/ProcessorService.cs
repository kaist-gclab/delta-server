using System;
using NodaTime;

namespace Delta.AppServer.Processors
{
    public class ProcessorService
    {
        private readonly DeltaContext _context;
        private readonly IClock _clock;

        public ProcessorService(DeltaContext context, IClock clock)
        {
            _context = context;
            _clock = clock;
        }

        public ProcessorNode GetNode(long id)
        {
            return _context.ProcessorNodes.Find(id);
        }

        public ProcessorNodeStatus AddNodeStatus(ProcessorNode processorNode, string status)
        {
            var nodeStatus = new ProcessorNodeStatus
            {
                Timestamp = _clock.GetCurrentInstant(),
                Status = status,
            };
            processorNode.ProcessorNodeStatuses.Add(nodeStatus);
            _context.SaveChanges();
            return nodeStatus;
        }
    }
}