﻿// <auto-generated />
using System;
using Delta.AppServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Delta.AppServer.Migrations
{
    [DbContext(typeof(DeltaContext))]
    [Migration("20191119220428_ProcessorVersionInputCapabilityNullAssetFormat")]
    partial class ProcessorVersionInputCapabilityNullAssetFormat
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Delta.AppServer.Assets.Asset", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<long?>("AssetFormatId")
                        .HasColumnName("asset_format_id")
                        .HasColumnType("bigint");

                    b.Property<long?>("AssetTypeId")
                        .HasColumnName("asset_type_id")
                        .HasColumnType("bigint");

                    b.Property<Instant>("CreatedAt")
                        .HasColumnName("created_at")
                        .HasColumnType("timestamp");

                    b.Property<long?>("EncryptionKeyId")
                        .HasColumnName("encryption_key_id")
                        .HasColumnType("bigint");

                    b.Property<long?>("ParentJobExecutionId")
                        .HasColumnName("parent_job_execution_id")
                        .HasColumnType("bigint");

                    b.Property<string>("StoreKey")
                        .HasColumnName("store_key")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AssetFormatId");

                    b.HasIndex("AssetTypeId");

                    b.HasIndex("EncryptionKeyId");

                    b.HasIndex("ParentJobExecutionId");

                    b.ToTable("asset");
                });

            modelBuilder.Entity("Delta.AppServer.Assets.AssetFormat", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnName("description")
                        .HasColumnType("text");

                    b.Property<string>("FileExtension")
                        .HasColumnName("file_extension")
                        .HasColumnType("text");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnName("key")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("asset_format");
                });

            modelBuilder.Entity("Delta.AppServer.Assets.AssetTag", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<long>("AssetId")
                        .HasColumnName("asset_id")
                        .HasColumnType("bigint");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnName("key")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnName("value")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AssetId");

                    b.ToTable("asset_tag");
                });

            modelBuilder.Entity("Delta.AppServer.Assets.AssetType", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnName("key")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("asset_type");
                });

            modelBuilder.Entity("Delta.AppServer.Jobs.Job", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<Instant>("CreatedAt")
                        .HasColumnName("created_at")
                        .HasColumnType("timestamp");

                    b.Property<long?>("InputAssetId")
                        .HasColumnName("input_asset_id")
                        .HasColumnType("bigint");

                    b.Property<string>("JobArguments")
                        .IsRequired()
                        .HasColumnName("job_arguments")
                        .HasColumnType("text");

                    b.Property<long>("ProcessorTypeId")
                        .HasColumnName("processor_type_id")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("InputAssetId");

                    b.HasIndex("ProcessorTypeId");

                    b.ToTable("job");
                });

            modelBuilder.Entity("Delta.AppServer.Jobs.JobExecution", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<long>("JobId")
                        .HasColumnName("job_id")
                        .HasColumnType("bigint");

                    b.Property<long>("ProcessorNodeId")
                        .HasColumnName("processor_node_id")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("JobId");

                    b.HasIndex("ProcessorNodeId");

                    b.ToTable("job_execution");
                });

            modelBuilder.Entity("Delta.AppServer.Jobs.JobExecutionStatus", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<long>("JobExecutionId")
                        .HasColumnName("job_execution_id")
                        .HasColumnType("bigint");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnName("status")
                        .HasColumnType("text");

                    b.Property<Instant>("Timestamp")
                        .HasColumnName("timestamp")
                        .HasColumnType("timestamp");

                    b.HasKey("Id");

                    b.HasIndex("JobExecutionId");

                    b.ToTable("job_execution_status");
                });

            modelBuilder.Entity("Delta.AppServer.Processors.ProcessorNode", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnName("key")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnName("name")
                        .HasColumnType("text");

                    b.Property<long>("ProcessorVersionId")
                        .HasColumnName("processor_version_id")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ProcessorVersionId");

                    b.ToTable("processor_node");
                });

            modelBuilder.Entity("Delta.AppServer.Processors.ProcessorNodeStatus", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<long>("ProcessorNodeId")
                        .HasColumnName("processor_node_id")
                        .HasColumnType("bigint");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnName("status")
                        .HasColumnType("text");

                    b.Property<Instant>("Timestamp")
                        .HasColumnName("timestamp")
                        .HasColumnType("timestamp");

                    b.HasKey("Id");

                    b.HasIndex("ProcessorNodeId");

                    b.ToTable("processor_node_status");
                });

            modelBuilder.Entity("Delta.AppServer.Processors.ProcessorType", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnName("key")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnName("name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("processor_type");
                });

            modelBuilder.Entity("Delta.AppServer.Processors.ProcessorVersion", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<Instant>("CreatedAt")
                        .HasColumnName("created_at")
                        .HasColumnType("timestamp");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnName("description")
                        .HasColumnType("text");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnName("key")
                        .HasColumnType("text");

                    b.Property<long>("ProcessorTypeId")
                        .HasColumnName("processor_type_id")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ProcessorTypeId");

                    b.ToTable("processor_version");
                });

            modelBuilder.Entity("Delta.AppServer.Processors.ProcessorVersionInputCapability", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<long?>("AssetFormatId")
                        .HasColumnName("asset_format_id")
                        .HasColumnType("bigint");

                    b.Property<long?>("AssetTypeId")
                        .HasColumnName("asset_type_id")
                        .HasColumnType("bigint");

                    b.Property<long>("ProcessorVersionId")
                        .HasColumnName("processor_version_id")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("AssetFormatId");

                    b.HasIndex("AssetTypeId");

                    b.HasIndex("ProcessorVersionId");

                    b.ToTable("processor_version_input_capability");
                });

            modelBuilder.Entity("Delta.AppServer.Security.EncryptionKey", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<bool>("Enabled")
                        .HasColumnName("enabled")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnName("value")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("encryption_key");
                });

            modelBuilder.Entity("Delta.AppServer.Assets.Asset", b =>
                {
                    b.HasOne("Delta.AppServer.Assets.AssetFormat", "AssetFormat")
                        .WithMany("Assets")
                        .HasForeignKey("AssetFormatId");

                    b.HasOne("Delta.AppServer.Assets.AssetType", "AssetType")
                        .WithMany("Assets")
                        .HasForeignKey("AssetTypeId");

                    b.HasOne("Delta.AppServer.Security.EncryptionKey", "EncryptionKey")
                        .WithMany("Assets")
                        .HasForeignKey("EncryptionKeyId");

                    b.HasOne("Delta.AppServer.Jobs.JobExecution", "ParentJobExecution")
                        .WithMany("ChildAssets")
                        .HasForeignKey("ParentJobExecutionId");
                });

            modelBuilder.Entity("Delta.AppServer.Assets.AssetTag", b =>
                {
                    b.HasOne("Delta.AppServer.Assets.Asset", "Asset")
                        .WithMany("AssetTags")
                        .HasForeignKey("AssetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Delta.AppServer.Jobs.Job", b =>
                {
                    b.HasOne("Delta.AppServer.Assets.Asset", "InputAsset")
                        .WithMany("InputJobs")
                        .HasForeignKey("InputAssetId");

                    b.HasOne("Delta.AppServer.Processors.ProcessorType", "ProcessorType")
                        .WithMany("Jobs")
                        .HasForeignKey("ProcessorTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
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
                });

            modelBuilder.Entity("Delta.AppServer.Jobs.JobExecutionStatus", b =>
                {
                    b.HasOne("Delta.AppServer.Jobs.JobExecution", "JobExecution")
                        .WithMany("JobExecutionStatuses")
                        .HasForeignKey("JobExecutionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Delta.AppServer.Processors.ProcessorNode", b =>
                {
                    b.HasOne("Delta.AppServer.Processors.ProcessorVersion", "ProcessorVersion")
                        .WithMany("ProcessorNodes")
                        .HasForeignKey("ProcessorVersionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Delta.AppServer.Processors.ProcessorNodeStatus", b =>
                {
                    b.HasOne("Delta.AppServer.Processors.ProcessorNode", "ProcessorNode")
                        .WithMany("ProcessorNodeStatuses")
                        .HasForeignKey("ProcessorNodeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Delta.AppServer.Processors.ProcessorVersion", b =>
                {
                    b.HasOne("Delta.AppServer.Processors.ProcessorType", "ProcessorType")
                        .WithMany("ProcessorVersions")
                        .HasForeignKey("ProcessorTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Delta.AppServer.Processors.ProcessorVersionInputCapability", b =>
                {
                    b.HasOne("Delta.AppServer.Assets.AssetFormat", "AssertFormat")
                        .WithMany("ProcessorVersionInputCapabilities")
                        .HasForeignKey("AssetFormatId");

                    b.HasOne("Delta.AppServer.Assets.AssetType", "AssertType")
                        .WithMany("ProcessorVersionInputCapabilities")
                        .HasForeignKey("AssetTypeId");

                    b.HasOne("Delta.AppServer.Processors.ProcessorVersion", "ProcessorVersion")
                        .WithMany("ProcessorVersionInputCapabilities")
                        .HasForeignKey("ProcessorVersionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
