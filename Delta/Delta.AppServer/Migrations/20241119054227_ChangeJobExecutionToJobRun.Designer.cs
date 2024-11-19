﻿// <auto-generated />
using Delta.AppServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Delta.AppServer.Migrations
{
    [DbContext(typeof(DeltaContext))]
    [Migration("20241119054227_ChangeJobExecutionToJobRun")]
    partial class ChangeJobExecutionToJobRun
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

                    b.Property<Instant>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<long?>("EncryptionKeyId")
                        .HasColumnType("bigint")
                        .HasColumnName("encryption_key_id");

                    b.Property<long?>("JobRunId")
                        .HasColumnType("bigint")
                        .HasColumnName("job_run_id");

                    b.Property<string>("StoreKey")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("store_key");

                    b.HasKey("Id");

                    b.HasIndex("EncryptionKeyId");

                    b.HasIndex("JobRunId");

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

            modelBuilder.Entity("Delta.AppServer.Buckets.BucketAsset", b =>
                {
                    b.Property<long>("BucketId")
                        .HasColumnType("bigint")
                        .HasColumnName("bucket_id");

                    b.Property<string>("Path")
                        .HasColumnType("text")
                        .HasColumnName("path");

                    b.Property<long>("AssetId")
                        .HasColumnType("bigint")
                        .HasColumnName("asset_id");

                    b.HasKey("BucketId", "Path");

                    b.HasIndex("AssetId");

                    b.ToTable("bucket_asset");
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

            modelBuilder.Entity("Delta.AppServer.Buckets.BucketSource", b =>
                {
                    b.Property<long>("InputBucketId")
                        .HasColumnType("bigint")
                        .HasColumnName("input_bucket_id");

                    b.Property<long>("OutputBucketId")
                        .HasColumnType("bigint")
                        .HasColumnName("output_bucket_id");

                    b.HasKey("InputBucketId", "OutputBucketId");

                    b.HasIndex("OutputBucketId");

                    b.ToTable("bucket_source");
                });

            modelBuilder.Entity("Delta.AppServer.Buckets.BucketTag", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("BucketId")
                        .HasColumnType("bigint")
                        .HasColumnName("bucket_id");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("key");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("value");

                    b.HasKey("Id");

                    b.HasIndex("BucketId");

                    b.ToTable("bucket_tag");
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

                    b.Property<bool>("Optimized")
                        .HasColumnType("boolean")
                        .HasColumnName("optimized");

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

            modelBuilder.Entity("Delta.AppServer.Jobs.JobRun", b =>
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

                    b.ToTable("job_run");
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
                    b.HasOne("Delta.AppServer.Encryption.EncryptionKey", null)
                        .WithMany("Assets")
                        .HasForeignKey("EncryptionKeyId");

                    b.HasOne("Delta.AppServer.Jobs.JobRun", null)
                        .WithMany("ResultAssets")
                        .HasForeignKey("JobRunId");
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

            modelBuilder.Entity("Delta.AppServer.Buckets.BucketAsset", b =>
                {
                    b.HasOne("Delta.AppServer.Assets.Asset", "Asset")
                        .WithMany()
                        .HasForeignKey("AssetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Delta.AppServer.Buckets.Bucket", "Bucket")
                        .WithMany("Assets")
                        .HasForeignKey("BucketId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Asset");

                    b.Navigation("Bucket");
                });

            modelBuilder.Entity("Delta.AppServer.Buckets.BucketSource", b =>
                {
                    b.HasOne("Delta.AppServer.Buckets.Bucket", "InputBucket")
                        .WithMany()
                        .HasForeignKey("InputBucketId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Delta.AppServer.Buckets.Bucket", "OutputBucket")
                        .WithMany()
                        .HasForeignKey("OutputBucketId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("InputBucket");

                    b.Navigation("OutputBucket");
                });

            modelBuilder.Entity("Delta.AppServer.Buckets.BucketTag", b =>
                {
                    b.HasOne("Delta.AppServer.Buckets.Bucket", "Bucket")
                        .WithMany("Tags")
                        .HasForeignKey("BucketId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bucket");
                });

            modelBuilder.Entity("Delta.AppServer.Jobs.Job", b =>
                {
                    b.HasOne("Delta.AppServer.Processors.ProcessorNode", "AssignedProcessorNode")
                        .WithMany("AssignedJobs")
                        .HasForeignKey("AssignedProcessorNodeId");

                    b.HasOne("Delta.AppServer.Assets.Asset", "InputAsset")
                        .WithMany()
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

            modelBuilder.Entity("Delta.AppServer.Jobs.JobExecutionStatus", b =>
                {
                    b.HasOne("Delta.AppServer.Jobs.JobRun", "JobExecution")
                        .WithMany("JobExecutionStatuses")
                        .HasForeignKey("JobExecutionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("JobExecution");
                });

            modelBuilder.Entity("Delta.AppServer.Jobs.JobRun", b =>
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
                });

            modelBuilder.Entity("Delta.AppServer.Buckets.Bucket", b =>
                {
                    b.Navigation("Assets");

                    b.Navigation("Tags");
                });

            modelBuilder.Entity("Delta.AppServer.Encryption.EncryptionKey", b =>
                {
                    b.Navigation("Assets");
                });

            modelBuilder.Entity("Delta.AppServer.Jobs.Job", b =>
                {
                    b.Navigation("JobExecutions");
                });

            modelBuilder.Entity("Delta.AppServer.Jobs.JobRun", b =>
                {
                    b.Navigation("JobExecutionStatuses");

                    b.Navigation("ResultAssets");
                });

            modelBuilder.Entity("Delta.AppServer.Jobs.JobType", b =>
                {
                    b.Navigation("Jobs");
                });

            modelBuilder.Entity("Delta.AppServer.Processors.ProcessorNode", b =>
                {
                    b.Navigation("AssignedJobs");

                    b.Navigation("JobExecutions");

                    b.Navigation("ProcessorNodeStatuses");
                });
#pragma warning restore 612, 618
        }
    }
}
