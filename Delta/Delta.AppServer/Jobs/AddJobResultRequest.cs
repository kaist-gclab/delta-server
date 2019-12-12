using System.Collections.Generic;

namespace Delta.AppServer.Jobs
{
    public class AddJobResultRequest
    {
        public long JobExecutionId { get; set; }
        public IEnumerable<RequestResultAsset> ResultAssets { get; set; }
    }
}