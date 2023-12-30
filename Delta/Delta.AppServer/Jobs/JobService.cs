using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Delta.AppServer.Processors;
using NodaTime;

namespace Delta.AppServer.Jobs;

public class JobService
{
    private readonly DeltaContext _context;
    private readonly IClock _clock;

    public JobService(DeltaContext context, IClock clock)
    {
        _context = context;
        _clock = clock;
    }

    public IQueryable<JobType> GetJobTypes() => _context.JobTypes;

    public async Task CreateJob(CreateJobRequest createJobRequest)
    {
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

    public JobScheduleResponse ScheduleNextJob(JobScheduleRequest jobScheduleRequest)
    {
        using var trx = _context.Database.BeginTransaction();

        var processorNode = _context.ProcessorNodes.Find(jobScheduleRequest.ProcessorNodeId);
        if (processorNode == null)
        {
            throw new Exception();
        }

        var availableJobs = from j in _context.Jobs
            let last =
                from e in j.JobExecutions
                let last =
                    (from s in e.JobExecutionStatuses
                        orderby s.Timestamp descending
                        select s).First()
                where
                    last.Status == PredefinedJobExecutionStatuses.Assigned ||
                    last.Status == PredefinedJobExecutionStatuses.Complete
                select last
            where !last.Any()
            select j;


        Job GetFirstJob()
        {
            foreach (var j in availableJobs)
            {
                if (j.AssignedProcessorNode != null)
                {
                    if (j.AssignedProcessorNode == processorNode)
                    {
                        return j;
                    }

                    continue;
                }

                if (ValidateCompatibility(j, processorNode.ProcessorNodeCapabilities))
                {
                    return j;
                }
            }

            return null;
        }

        var job = GetFirstJob();
        if (job == null)
        {
            return null;
        }

        var jobExecution = new JobExecution
        {
            ProcessorNode = processorNode,
            Job = job
        };

        var now = _clock.GetCurrentInstant();
        jobExecution.AddStatus(now, PredefinedJobExecutionStatuses.Assigned);
        processorNode.AddNodeStatus(now, PredefinedProcessorNodeStatuses.Busy);

        _context.SaveChanges();
        trx.Commit();
        return new JobScheduleResponse(jobExecution);
    }

    public void AddJobExecutionStatus(long jobExecutionId,
        AddJobExecutionStatusRequest addJobExecutionStatusRequest)
    {
        var jobExecution = _context.JobExecutions.Find(jobExecutionId);
        if (jobExecution == null)
        {
            throw new Exception();
        }

        jobExecution.AddStatus(_clock.GetCurrentInstant(),
            addJobExecutionStatusRequest.Status);
        _context.SaveChanges();
    }

    private static bool ValidateCompatibility(Job job, IEnumerable<ProcessorNodeCapability> capabilities)
    {
        foreach (var cap in capabilities)
        {
            if (job.InputAsset.MediaType != cap.MediaType ||
                job.JobType != cap.JobType)
            {
                continue;
            }

            if (cap.AssetType == null ||
                cap.AssetType == job.InputAsset.AssetType)
            {
                return true;
            }
        }

        return false;
    }

    public IEnumerable<Job> GetJobs()
    {
        return _context.Jobs.ToList();
    }
}