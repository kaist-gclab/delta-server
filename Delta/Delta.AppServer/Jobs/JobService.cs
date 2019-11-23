using System;
using System.Collections.Generic;
using System.Linq;
using Delta.AppServer.Assets;
using Delta.AppServer.Processors;
using NodaTime;

namespace Delta.AppServer.Jobs
{
    public class JobService
    {
        private readonly DeltaContext _context;
        private readonly IClock _clock;

        public JobService(DeltaContext context, IClock clock)
        {
            _context = context;
            _clock = clock;
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

        public Job GetJob(long id)
        {
            return _context.Jobs.Find(id);
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
                Timestamp = _clock.GetCurrentInstant(),
            };
            jobExecution.JobExecutionStatuses.Add(jobExecutionStatus);
            _context.SaveChanges();
            return jobExecutionStatus;
        }

        public bool ValidateCompatibility(Asset asset, ProcessorVersion processorVersion)
        {
            if (asset == null)
            {
                return true;
            }

            return (from c in processorVersion.ProcessorVersionInputCapabilities
                    where (c.AssertFormat == null || c.AssertFormat == asset.AssetFormat) &&
                          (c.AssertType == null || c.AssertType == asset.AssetType)
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
    }
}