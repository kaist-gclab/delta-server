using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Delta.AppServer.Assets;
using Delta.AppServer.Processors;
using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace Delta.AppServer.Jobs;

public class JobService(DeltaContext context, IClock clock)
{
    public async Task<IEnumerable<JobTypeView>> GetJobTypeViews()
    {
        var q = from t in context.JobType
            select new JobTypeView(t.Id, t.Key, t.Name);

        return await q.ToListAsync();
    }

    public async Task CreateJob(CreateJobRequest createJobRequest)
    {
        var inputAsset = await context.FindAsync<Asset>(createJobRequest.InputAssetId);
        if (inputAsset == null)
        {
            return;
        }

        var jobType = await context.FindAsync<JobType>(createJobRequest.JobTypeId);
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
            CreatedAt = clock.GetCurrentInstant()
        };

        await context.AddAsync(job);
        await context.SaveChangesAsync();
    }

    public async Task<JobScheduleResponse?> ScheduleNextJob(JobScheduleRequest jobScheduleRequest)
    {
        await using var trx = await context.Database.BeginTransactionAsync();

        var processorNodeQuery = from n in context.ProcessorNode
            where n.Id == jobScheduleRequest.ProcessorNodeId
            select n;
        var processorNode = await processorNodeQuery.FirstOrDefaultAsync();
        if (processorNode == null)
        {
            return null;
        }

        var availableJobs = from j in context.Job
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

        var jobExecution = new JobRun
        {
            ProcessorNode = processorNode,
            Job = job
        };

        var now = clock.GetCurrentInstant();
        jobExecution.AddStatus(now, PredefinedJobExecutionStatuses.Assigned);
        processorNode.AddNodeStatus(now, PredefinedProcessorNodeStatuses.Busy);

        await context.SaveChangesAsync();
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

                if (ValidateCompatibility(j))
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
        var jobExecution = await context.JobRun.FindAsync(jobExecutionId);
        if (jobExecution == null)
        {
            return;
        }

        jobExecution.AddStatus(clock.GetCurrentInstant(),
            addJobExecutionStatusRequest.Status);
        await context.SaveChangesAsync();
    }

    private static bool ValidateCompatibility(Job job)
    {
        return true;
    }

    public async Task<IEnumerable<Job>> GetJobs()
    {
        return await context.Job.ToListAsync();
    }
}