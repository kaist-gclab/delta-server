using System.Linq;
using Delta.AppServer.Assets;
using Delta.AppServer.Jobs;
using Delta.AppServer.Processors;
using Delta.AppServer.Encryption;
using Microsoft.EntityFrameworkCore;

namespace Delta.AppServer
{
    public class DeltaContext : DbContext
    {
        public DeltaContext(DbContextOptions<DeltaContext> options)
            : base(options)
        {
        }

        public DbSet<Asset> Assets { get; set; }
        public DbSet<AssetFormat> AssetFormats { get; set; }
        public DbSet<AssetTag> AssetTags { get; set; }
        public DbSet<AssetType> AssetTypes { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobExecution> JobExecutions { get; set; }
        public DbSet<JobExecutionStatus> JobExecutionStatuses { get; set; }
        public DbSet<ProcessorNode> ProcessorNodes { get; set; }
        public DbSet<ProcessorNodeStatus> ProcessorNodeStatuses { get; set; }
        public DbSet<ProcessorType> ProcessorTypes { get; set; }
        public DbSet<ProcessorVersion> ProcessorVersions { get; set; }
        public DbSet<ProcessorVersionInputCapability> ProcessorVersionInputCapabilities { get; set; }
        public DbSet<EncryptionKey> EncryptionKeys { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                entityType.SetTableName(entityType.DisplayName().ToUnderscoreCase());
                foreach (var property in entityType.GetProperties())
                {
                    property.SetColumnName(property.Name.ToUnderscoreCase());
                }
            }
        }
    }

    internal static class IdentifierNameHelper
    {
        public static string ToUnderscoreCase(this string str)
        {
            return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x : x.ToString())).ToLower();
        }
    }
}