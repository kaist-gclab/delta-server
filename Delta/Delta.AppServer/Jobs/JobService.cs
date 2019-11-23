using System;
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
    }
}