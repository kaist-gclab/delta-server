﻿// <auto-generated />
using Delta.AppServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Delta.AppServer.Migrations
{
    [DbContext(typeof(DeltaContext))]
    partial class DeltaContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Delta.AppServer.Assets.Asset", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("AssetTypeId")
                        .HasColumnType("bigint")
                        .HasColumnName("asset_type_id");

                    b.Property<Instant>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<long?>("EncryptionKeyId")
                        .HasColumnType("bigint")
                        .HasColumnName("encryption_key_id");

                    b.Property<string>("MediaType")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("media_type");

                    b.Property<long?>("ParentJobExecutionId")
                        .HasColumnType("bigint")
                        .HasColumnName("parent_job_execution_id");

                    b.Property<string>("StoreKey")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("store_key");

                    b.HasKey("Id");

                    b.HasIndex("AssetTypeId");

                    b.HasIndex("EncryptionKeyId");

                    b.HasIndex("ParentJobExecutionId");

                    b.ToTable("asset");
                });

            modelBuilder.Entity("Delta.AppServer.Assets.AssetTag", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("AssetId")
                        .HasColumnType("bigint")
                        .HasColumnName("asset_id");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("key");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("value");

                    b.HasKey("Id");

                    b.HasIndex("AssetId");

                    b.ToTable("asset_tag");
                });

            modelBuilder.Entity("Delta.AppServer.Assets.AssetType", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("key");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("asset_type");
                });

            modelBuilder.Entity("Delta.AppServer.Buckets.Bucket", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long?>("BucketGroupId")
                        .HasColumnType("bigint")
                        .HasColumnName("bucket_group_id");

                    b.Property<Instant>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<long?>("EncryptionKeyId")
                        .HasColumnType("bigint")
                        .HasColumnName("encryption_key_id");

                    b.HasKey("Id");

                    b.HasIndex("BucketGroupId");

                    b.HasIndex("EncryptionKeyId");

                    b.ToTable("bucket");
                });

            modelBuilder.Entity("Delta.AppServer.Buckets.BucketGroup", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<int>("Order")
                        .HasColumnType("integer")
                        .HasColumnName("order");

                    b.HasKey("Id");

                    b.ToTable("bucket_group");
                });

            modelBuilder.Entity("Delta.AppServer.Encryption.EncryptionKey", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<bool>("Enabled")
                        .HasColumnType("boolean")
                        .HasColumnName("enabled");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("Value")
                        .HasColumnType("text")
                        .HasColumnName("value");

                    b.HasKey("Id");

                    b.ToTable("encryption_key");
                });

            modelBuilder.Entity("Delta.AppServer.Jobs.Job", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long?>("AssignedProcessorNodeId")
                        .HasColumnType("bigint")
                        .HasColumnName("assigned_processor_node_id");

                    b.Property<Instant>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<long?>("InputAssetId")
                        .HasColumnType("bigint")
                        .HasColumnName("input_asset_id");

                    b.Property<string>("JobArguments")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("job_arguments");

                    b.Property<long>("JobTypeId")
                        .HasColumnType("bigint")
                        .HasColumnName("job_type_id");

                    b.HasKey("Id");

                    b.HasIndex("AssignedProcessorNodeId");

                    b.HasIndex("InputAssetId");

                    b.HasIndex("JobTypeId");

                    b.ToTable("job");
                });

            modelBuilder.Entity("Delta.AppServer.Jobs.JobExecution", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("JobId")
                        .HasColumnType("bigint")
                        .HasColumnName("job_id");

                    b.Property<long>("ProcessorNodeId")
                        .HasColumnType("bigint")
                        .HasColumnName("processor_node_id");

                    b.HasKey("Id");

                    b.HasIndex("JobId");

                    b.HasIndex("ProcessorNodeId");

                    b.ToTable("job_execution");
                });

            modelBuilder.Entity("Delta.AppServer.Jobs.JobExecutionStatus", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("JobExecutionId")
                        .HasColumnType("bigint")
                        .HasColumnName("job_execution_id");

                    b.Property<string>("Status")
                        .HasColumnType("text")
                        .HasColumnName("status");

                    b.Property<Instant>("Timestamp")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("timestamp");

                    b.HasKey("Id");

                    b.HasIndex("JobExecutionId");

                    b.ToTable("job_execution_status");
                });

            modelBuilder.Entity("Delta.AppServer.Jobs.JobType", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("key");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("job_type");
                });

            modelBuilder.Entity("Delta.AppServer.Processors.ProcessorNode", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("key");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<long>("ProcessorVersionId")
                        .HasColumnType("bigint")
                        .HasColumnName("processor_version_id");

                    b.HasKey("Id");

                    b.ToTable("processor_node");
                });

            modelBuilder.Entity("Delta.AppServer.Processors.ProcessorNodeCapability", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long?>("AssetTypeId")
                        .HasColumnType("bigint")
                        .HasColumnName("asset_type_id");

                    b.Property<long>("JobTypeId")
                        .HasColumnType("bigint")
                        .HasColumnName("job_type_id");

                    b.Property<string>("MediaType")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("media_type");

                    b.Property<long>("ProcessorNodeId")
                        .HasColumnType("bigint")
                        .HasColumnName("processor_node_id");

                    b.HasKey("Id");

                    b.HasIndex("AssetTypeId");

                    b.HasIndex("JobTypeId");

                    b.HasIndex("ProcessorNodeId");

                    b.ToTable("processor_node_capability");
                });

            modelBuilder.Entity("Delta.AppServer.Processors.ProcessorNodeStatus", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("ProcessorNodeId")
                        .HasColumnType("bigint")
                        .HasColumnName("processor_node_id");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("status");

                    b.Property<Instant>("Timestamp")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("timestamp");

                    b.HasKey("Id");

                    b.HasIndex("ProcessorNodeId");

                    b.ToTable("processor_node_status");
                });

            modelBuilder.Entity("Delta.AppServer.Settings.Setting", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("text")
                        .HasColumnName("key");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("value");

                    b.HasKey("Key");

                    b.ToTable("setting");
                });

            modelBuilder.Entity("Delta.AppServer.Users.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("Password")
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.Property<string>("Salt")
                        .HasColumnType("text")
                        .HasColumnName("salt");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("username");

                    b.HasKey("Id");

                    b.ToTable("user");
                });

            modelBuilder.Entity("Delta.AppServer.Assets.Asset", b =>
                {
                    b.HasOne("Delta.AppServer.Assets.AssetType", "AssetType")
                        .WithMany("Assets")
                        .HasForeignKey("AssetTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Delta.AppServer.Encryption.EncryptionKey", "EncryptionKey")
                        .WithMany("Assets")
                        .HasForeignKey("EncryptionKeyId");

                    b.HasOne("Delta.AppServer.Jobs.JobExecution", "ParentJobExecution")
                        .WithMany("ResultAssets")
                        .HasForeignKey("ParentJobExecutionId");

                    b.Navigation("AssetType");

                    b.Navigation("EncryptionKey");

                    b.Navigation("ParentJobExecution");
                });

            modelBuilder.Entity("Delta.AppServer.Assets.AssetTag", b =>
                {
                    b.HasOne("Delta.AppServer.Assets.Asset", "Asset")
                        .WithMany("AssetTags")
                        .HasForeignKey("AssetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Asset");
                });

            modelBuilder.Entity("Delta.AppServer.Buckets.Bucket", b =>
                {
                    b.HasOne("Delta.AppServer.Buckets.BucketGroup", "BucketGroup")
                        .WithMany()
                        .HasForeignKey("BucketGroupId");

                    b.HasOne("Delta.AppServer.Encryption.EncryptionKey", "EncryptionKey")
                        .WithMany()
                        .HasForeignKey("EncryptionKeyId");

                    b.Navigation("BucketGroup");

                    b.Navigation("EncryptionKey");
                });

            modelBuilder.Entity("Delta.AppServer.Jobs.Job", b =>
                {
                    b.HasOne("Delta.AppServer.Processors.ProcessorNode", "AssignedProcessorNode")
                        .WithMany("AssignedJobs")
                        .HasForeignKey("AssignedProcessorNodeId");

                    b.HasOne("Delta.AppServer.Assets.Asset", "InputAsset")
                        .WithMany("InputJobs")
                        .HasForeignKey("InputAssetId");

                    b.HasOne("Delta.AppServer.Jobs.JobType", "JobType")
                        .WithMany("Jobs")
                        .HasForeignKey("JobTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AssignedProcessorNode");

                    b.Navigation("InputAsset");

                    b.Navigation("JobType");
                });

            modelBuilder.Entity("Delta.AppServer.Jobs.JobExecution", b =>
                {
                    b.HasOne("Delta.AppServer.Jobs.Job", "Job")
                        .WithMany("JobExecutions")
                        .HasForeignKey("JobId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Delta.AppServer.Processors.ProcessorNode", "ProcessorNode")
                        .WithMany("JobExecutions")
                        .HasForeignKey("ProcessorNodeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Job");

                    b.Navigation("ProcessorNode");
                });

            modelBuilder.Entity("Delta.AppServer.Jobs.JobExecutionStatus", b =>
                {
                    b.HasOne("Delta.AppServer.Jobs.JobExecution", "JobExecution")
                        .WithMany("JobExecutionStatuses")
                        .HasForeignKey("JobExecutionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("JobExecution");
                });

            modelBuilder.Entity("Delta.AppServer.Processors.ProcessorNodeCapability", b =>
                {
                    b.HasOne("Delta.AppServer.Assets.AssetType", "AssetType")
                        .WithMany("ProcessorNodeCapabilities")
                        .HasForeignKey("AssetTypeId");

                    b.HasOne("Delta.AppServer.Jobs.JobType", "JobType")
                        .WithMany("ProcessorNodeCapabilities")
                        .HasForeignKey("JobTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Delta.AppServer.Processors.ProcessorNode", "ProcessorNode")
                        .WithMany("ProcessorNodeCapabilities")
                        .HasForeignKey("ProcessorNodeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AssetType");

                    b.Navigation("JobType");

                    b.Navigation("ProcessorNode");
                });

            modelBuilder.Entity("Delta.AppServer.Processors.ProcessorNodeStatus", b =>
                {
                    b.HasOne("Delta.AppServer.Processors.ProcessorNode", "ProcessorNode")
                        .WithMany("ProcessorNodeStatuses")
                        .HasForeignKey("ProcessorNodeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProcessorNode");
                });

            modelBuilder.Entity("Delta.AppServer.Assets.Asset", b =>
                {
                    b.Navigation("AssetTags");

                    b.Navigation("InputJobs");
                });

            modelBuilder.Entity("Delta.AppServer.Assets.AssetType", b =>
                {
                    b.Navigation("Assets");

                    b.Navigation("ProcessorNodeCapabilities");
                });

            modelBuilder.Entity("Delta.AppServer.Encryption.EncryptionKey", b =>
                {
                    b.Navigation("Assets");
                });

            modelBuilder.Entity("Delta.AppServer.Jobs.Job", b =>
                {
                    b.Navigation("JobExecutions");
                });

            modelBuilder.Entity("Delta.AppServer.Jobs.JobExecution", b =>
                {
                    b.Navigation("JobExecutionStatuses");

                    b.Navigation("ResultAssets");
                });

            modelBuilder.Entity("Delta.AppServer.Jobs.JobType", b =>
                {
                    b.Navigation("Jobs");

                    b.Navigation("ProcessorNodeCapabilities");
                });

            modelBuilder.Entity("Delta.AppServer.Processors.ProcessorNode", b =>
                {
                    b.Navigation("AssignedJobs");

                    b.Navigation("JobExecutions");

                    b.Navigation("ProcessorNodeCapabilities");

                    b.Navigation("ProcessorNodeStatuses");
                });
#pragma warning restore 612, 618
        }
    }
}
