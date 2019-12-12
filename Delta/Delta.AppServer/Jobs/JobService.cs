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
        private readonly AssetService _assetService;
        private readonly AssetMetadataService _assetMetadataService;
        private readonly ProcessorService _processorService;

        public JobService(DeltaContext context, IClock clock,
            AssetService assetService, AssetMetadataService assetMetadataService,
            ProcessorService processorService)
        {
            _context = context;
            _clock = clock;
            _assetService = assetService;
            _assetMetadataService = assetMetadataService;
            _processorService = processorService;
        }

        public Job AddJob(Asset inputAsset, ProcessorVersion processorVersion, string jobArguments)
        {
            if (processorVersion == null)
            {
                throw new Exception();
            }

            if (jobArguments == null)
            {
                throw new Exception();
            }

            if (ValidateCompatibility(inputAsset, processorVersion) == false)
            {
                throw new Exception();
            }

            var job = new Job
            {
                InputAsset = inputAsset,
                ProcessorVersion = processorVersion,
                JobArguments = jobArguments,
                CreatedAt = _clock.GetCurrentInstant()
            };
            job = _context.Add(job).Entity;
            _context.SaveChanges();
            return job;
        }

        public Job AddJob(AddJobRequest addJobRequest)
        {
            var inputAsset = _assetMetadataService.GetAsset(addJobRequest.InputAssetId);
            var processorVersion = _processorService.GetProcessorVersion(addJobRequest.ProcessorVersionKey);
            return AddJob(inputAsset, processorVersion, addJobRequest.JobArguments);
        }

        public Job GetJob(long id)
        {
            return _context.Jobs.Find(id);
        }

        public JobExecution GetJobExecution(long id)
        {
            return _context.JobExecutions.Find(id);
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

        private JobExecutionStatus AddJobExecutionStatus(JobExecution jobExecution, string status)
        {
            var jobExecutionStatus = new JobExecutionStatus
            {
                Status = status,
                Timestamp = _clock.GetCurrentInstant()
            };
            jobExecution.JobExecutionStatuses.Add(jobExecutionStatus);
            _context.SaveChanges();
            return jobExecutionStatus;
        }

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

        public async Task<JobExecutionStatus> AddJobResult(JobExecution jobExecution,
            IEnumerable<ResultAsset> resultAssets)
        {
            foreach (var r in resultAssets)
            {
                var encryptionKey = jobExecution.Job.InputAsset?.EncryptionKey;
                await _assetService.AddAsset(r.AssetFormat, r.AssetType, r.Content, encryptionKey, jobExecution);
            }

            var status = AddJobExecutionStatus(jobExecution, PredefinedJobExecutionStatuses.Complete);
            _context.SaveChanges();
            return status;
        }

        public async Task<JobExecutionStatus> AddJobResult(AddJobResultRequest addJobResultRequest)
        {
            var jobExecution = GetJobExecution(addJobResultRequest.JobExecutionId);
            var resultAssets = from r in addJobResultRequest.ResultAssets
                               let tags = from t in r.AssetTags
                                          select new AssetTag {Key = t.Key, Value = t.Value}
                               select new ResultAsset
                               {
                                   AssetFormat = _assetMetadataService.GetAssetFormat(r.AssetFormatKey),
                                   AssetType = _assetMetadataService.GetAssetType(r.AssetTypeKey),
                                   AssetTags = tags,
                                   Content = r.Content
                               };
            return await AddJobResult(jobExecution, resultAssets);
        }
    }
}