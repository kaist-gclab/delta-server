using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Delta.AppServer.Assets;
using Delta.AppServer.Processors;
using NodaTime;

namespace Delta.AppServer.Jobs
{
    public class JobService
    {
        private readonly DeltaContext _context;
        private readonly IClock _clock;
        private readonly AssetMetadataService _assetMetadataService;

        public JobService(DeltaContext context, IClock clock, AssetMetadataService assetMetadataService)
        {
            _context = context;
            _clock = clock;
            _assetMetadataService = assetMetadataService;
        }

        public IQueryable<JobType> GetJobTypes() => _context.JobTypes;

        public Job CreateJob(CreateJobRequest createJobRequest)
        {
            if (createJobRequest.JobArguments == null)
            {
                throw new Exception();
            }

            var job = new Job
            {
                InputAssetId = createJobRequest.InputAssetId,
                JobTypeId = createJobRequest.JobTypeId,
                JobArguments = createJobRequest.JobArguments,
                AssignedProcessorNodeId = createJobRequest.AssignedProcessorNodeId,
                CreatedAt = _clock.GetCurrentInstant()
            };
            job = _context.Add(job).Entity;
            _context.SaveChanges();
            return job;
        }

        public JobExecution ScheduleNextJob(ProcessorNode processorNode)
        {
            var job = GetAvailableJobs(processorNode).FirstOrDefault();
            if (job == null)
            {
                return null;
            }

            var jobExecution = new JobExecution
            {
                Job = job,
                ProcessorNode = processorNode
            };
            jobExecution = _context.Add(jobExecution).Entity;
            _context.SaveChanges();
            AddJobExecutionStatus(jobExecution, PredefinedJobExecutionStatuses.Assigned);
            return jobExecution;
        }

        public void AddJobExecutionStatus(long jobExecutionId,
            AddJobExecutionStatusRequest addJobExecutionStatusRequest)
        {
            var jobExecution = _context.JobExecutions.Find(jobExecutionId);
            if (jobExecution == null)
            {

        public bool ValidateCompatibility(Asset asset, ProcessorVersion processorVersion)
        {
            if (asset == null)
            {
                return (from c in processorVersion.ProcessorVersionInputCapabilities
                        where c.AssetFormat == null && c.AssetType == null
                        select c).Any();
            }

            return (from c in processorVersion.ProcessorVersionInputCapabilities
                    where (c.AssetFormat == null || c.AssetFormat == asset.AssetFormat) &&
                          (c.AssetType == null || c.AssetType == asset.AssetType)
                    select c).Any();
        }

        private IQueryable<Job> GetAvailableJobs(ProcessorNode node)
        {
            return from j in _context.Jobs
                   where node.ProcessorVersion == j.ProcessorVersion
                   let statuses =
                       from e in j.JobExecutions
                       let latestStatus = (from s in e.JobExecutionStatuses
                                           orderby s.Timestamp descending
                                           select s).First()
                       where latestStatus.Status == PredefinedJobExecutionStatuses.Complete
                       select latestStatus
                   where !statuses.Any()
                   orderby j.CreatedAt
                   select j;
        }

            _context.SaveChanges();
        }
    }
}