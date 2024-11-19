using System.Linq;
using Delta.AppServer.Assets;
using Delta.AppServer.Buckets;
using Delta.AppServer.Jobs;
using Delta.AppServer.Processors;
using Delta.AppServer.Encryption;
using Delta.AppServer.Settings;
using Delta.AppServer.Users;
using Microsoft.EntityFrameworkCore;

namespace Delta.AppServer;

public class DeltaContext(DbContextOptions<DeltaContext> options) : DbContext(options)
{
    public virtual DbSet<Asset> Asset => Set<Asset>();
    public virtual DbSet<Bucket> Bucket => Set<Bucket>();
    public virtual DbSet<BucketGroup> BucketGroup => Set<BucketGroup>();
    public virtual DbSet<BucketSource> BucketSource => Set<BucketSource>();
    public virtual DbSet<BucketTag> BucketTag => Set<BucketTag>();
    public virtual DbSet<AssetTag> AssetTag => Set<AssetTag>();
    public virtual DbSet<Job> Job => Set<Job>();
    public virtual DbSet<JobType> JobType => Set<JobType>();
    public virtual DbSet<JobExecution> JobExecution => Set<JobExecution>();
    public virtual DbSet<JobExecutionStatus> JobExecutionStatus => Set<JobExecutionStatus>();
    public virtual DbSet<ProcessorNode> ProcessorNode => Set<ProcessorNode>();
    public virtual DbSet<ProcessorNodeStatus> ProcessorNodeStatus => Set<ProcessorNodeStatus>();
    public virtual DbSet<ProcessorNodeCapability> ProcessorNodeCapability => Set<ProcessorNodeCapability>();
    public virtual DbSet<EncryptionKey> EncryptionKey => Set<EncryptionKey>();
    public virtual DbSet<User> User => Set<User>();
    public virtual DbSet<Setting> Setting => Set<Setting>();

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