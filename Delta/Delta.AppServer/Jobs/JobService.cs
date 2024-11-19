using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Delta.AppServer.Assets;
using Delta.AppServer.Processors;
using Microsoft.EntityFrameworkCore;
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

    public async Task<IEnumerable<JobTypeView>> GetJobTypeViews()
    {
        var q = from t in _context.JobType
            select new JobTypeView(t.Id, t.Key, t.Name);

        return await q.ToListAsync();
    }

    public async Task CreateJob(CreateJobRequest createJobRequest)
    {
        var inputAsset = await _context.FindAsync<Asset>(createJobRequest.InputAssetId);
        if (inputAsset == null)
        {
            return;
        }

        var jobType = await _context.FindAsync<JobType>(createJobRequest.JobTypeId);
        if (jobType == null)
        {
            return;
        }

        var job = new Job
        {
            InputAsset = inputAsset,
            JobType = jobType,
            JobArguments = createJobRequest.JobArguments,
            AssignedProcessorNodeId = createJobRequest.AssignedProcessorNodeId,
            CreatedAt = _clock.GetCurrentInstant()
        };

        await _context.AddAsync(job);
        await _context.SaveChangesAsync();
    }

    public async Task<JobScheduleResponse?> ScheduleNextJob(JobScheduleRequest jobScheduleRequest)
    {
        await using var trx = await _context.Database.BeginTransactionAsync();

        var processorNodeQuery = from n in _context.ProcessorNode
                .Include(n => n.ProcessorNodeCapabilities)
            where n.Id == jobScheduleRequest.ProcessorNodeId
            select n;
        var processorNode = await processorNodeQuery.FirstOrDefaultAsync();
        if (processorNode == null)
        {
            return null;
        }

        var availableJobs = from j in _context.Job
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

        await _context.SaveChangesAsync();
        await trx.CommitAsync();
        return new JobScheduleResponse(jobExecution);

        Job? GetFirstJob()
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
    }

    public async Task AddJobExecutionStatus(long jobExecutionId,
        AddJobExecutionStatusRequest addJobExecutionStatusRequest)
    {
        var jobExecution = await _context.JobExecution.FindAsync(jobExecutionId);
        if (jobExecution == null)
        {
            return;
        }

        jobExecution.AddStatus(_clock.GetCurrentInstant(),
            addJobExecutionStatusRequest.Status);
        await _context.SaveChangesAsync();
    }

    private static bool ValidateCompatibility(Job job, IEnumerable<ProcessorNodeCapability> capabilities)
    {
        foreach (var cap in capabilities)
        {
            if (job.InputAsset == null)
            {
                // TODO
                continue;
            }

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

    public async Task<IEnumerable<Job>> GetJobs()
    {
        return await _context.Job.ToListAsync();
    }
}