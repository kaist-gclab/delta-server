using System.Linq;
using Delta.AppServer.Assets;
using Delta.AppServer.Jobs;
using Delta.AppServer.Processors;
using Delta.AppServer.Encryption;
using Delta.AppServer.Settings;
using Delta.AppServer.Users;
using Microsoft.EntityFrameworkCore;

namespace Delta.AppServer;

public class DeltaContext : DbContext
{
    public DeltaContext(DbContextOptions<DeltaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Asset> Assets => Set<Asset>();
    public virtual DbSet<Bucket> Bucket => Set<Bucket>();
    public virtual DbSet<AssetTag> AssetTags => Set<AssetTag>();
    public virtual DbSet<AssetType> AssetTypes => Set<AssetType>();
    public virtual DbSet<Job> Jobs => Set<Job>();
    public virtual DbSet<JobType> JobTypes => Set<JobType>();
    public virtual DbSet<JobExecution> JobExecutions => Set<JobExecution>();
    public virtual DbSet<JobExecutionStatus> JobExecutionStatuses => Set<JobExecutionStatus>();
    public virtual DbSet<ProcessorNode> ProcessorNodes => Set<ProcessorNode>();
    public virtual DbSet<ProcessorNodeStatus> ProcessorNodeStatuses => Set<ProcessorNodeStatus>();
    public virtual DbSet<ProcessorNodeCapability> ProcessorNodeCapabilities => Set<ProcessorNodeCapability>();
    public virtual DbSet<EncryptionKey> EncryptionKeys => Set<EncryptionKey>();
    public virtual DbSet<User> Users => Set<User>();
    public virtual DbSet<Setting> Settings => Set<Setting>();

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