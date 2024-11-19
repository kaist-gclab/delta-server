using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Delta.AppServer.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDeleteBehaviorToRestrict : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_asset_job_run_job_run_id",
                table: "asset");

            migrationBuilder.DropForeignKey(
                name: "FK_asset_tag_asset_asset_id",
                table: "asset_tag");

            migrationBuilder.DropForeignKey(
                name: "FK_bucket_bucket_group_bucket_group_id",
                table: "bucket");

            migrationBuilder.DropForeignKey(
                name: "FK_bucket_encryption_key_encryption_key_id",
                table: "bucket");

            migrationBuilder.DropForeignKey(
                name: "FK_bucket_asset_asset_asset_id",
                table: "bucket_asset");

            migrationBuilder.DropForeignKey(
                name: "FK_bucket_asset_bucket_bucket_id",
                table: "bucket_asset");

            migrationBuilder.DropForeignKey(
                name: "FK_bucket_source_bucket_input_bucket_id",
                table: "bucket_source");

            migrationBuilder.DropForeignKey(
                name: "FK_bucket_source_bucket_output_bucket_id",
                table: "bucket_source");

            migrationBuilder.DropForeignKey(
                name: "FK_bucket_tag_bucket_bucket_id",
                table: "bucket_tag");

            migrationBuilder.DropForeignKey(
                name: "FK_job_asset_input_asset_id",
                table: "job");

            migrationBuilder.DropForeignKey(
                name: "FK_job_job_type_job_type_id",
                table: "job");

            migrationBuilder.DropForeignKey(
                name: "FK_job_processor_node_assigned_processor_node_id",
                table: "job");

            migrationBuilder.DropForeignKey(
                name: "FK_job_run_job_job_id",
                table: "job_run");

            migrationBuilder.DropForeignKey(
                name: "FK_job_run_processor_node_processor_node_id",
                table: "job_run");

            migrationBuilder.DropForeignKey(
                name: "FK_job_run_status_job_run_job_execution_id",
                table: "job_run_status");

            migrationBuilder.DropForeignKey(
                name: "FK_processor_node_status_processor_node_processor_node_id",
                table: "processor_node_status");

            migrationBuilder.AddForeignKey(
                name: "FK_asset_job_run_job_run_id",
                table: "asset",
                column: "job_run_id",
                principalTable: "job_run",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_asset_tag_asset_asset_id",
                table: "asset_tag",
                column: "asset_id",
                principalTable: "asset",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_bucket_bucket_group_bucket_group_id",
                table: "bucket",
                column: "bucket_group_id",
                principalTable: "bucket_group",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_bucket_encryption_key_encryption_key_id",
                table: "bucket",
                column: "encryption_key_id",
                principalTable: "encryption_key",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_bucket_asset_asset_asset_id",
                table: "bucket_asset",
                column: "asset_id",
                principalTable: "asset",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_bucket_asset_bucket_bucket_id",
                table: "bucket_asset",
                column: "bucket_id",
                principalTable: "bucket",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_bucket_source_bucket_input_bucket_id",
                table: "bucket_source",
                column: "input_bucket_id",
                principalTable: "bucket",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_bucket_source_bucket_output_bucket_id",
                table: "bucket_source",
                column: "output_bucket_id",
                principalTable: "bucket",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_bucket_tag_bucket_bucket_id",
                table: "bucket_tag",
                column: "bucket_id",
                principalTable: "bucket",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_job_asset_input_asset_id",
                table: "job",
                column: "input_asset_id",
                principalTable: "asset",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_job_job_type_job_type_id",
                table: "job",
                column: "job_type_id",
                principalTable: "job_type",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_job_processor_node_assigned_processor_node_id",
                table: "job",
                column: "assigned_processor_node_id",
                principalTable: "processor_node",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_job_run_job_job_id",
                table: "job_run",
                column: "job_id",
                principalTable: "job",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_job_run_processor_node_processor_node_id",
                table: "job_run",
                column: "processor_node_id",
                principalTable: "processor_node",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_job_run_status_job_run_job_execution_id",
                table: "job_run_status",
                column: "job_execution_id",
                principalTable: "job_run",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_processor_node_status_processor_node_processor_node_id",
                table: "processor_node_status",
                column: "processor_node_id",
                principalTable: "processor_node",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_asset_job_run_job_run_id",
                table: "asset");

            migrationBuilder.DropForeignKey(
                name: "FK_asset_tag_asset_asset_id",
                table: "asset_tag");

            migrationBuilder.DropForeignKey(
                name: "FK_bucket_bucket_group_bucket_group_id",
                table: "bucket");

            migrationBuilder.DropForeignKey(
                name: "FK_bucket_encryption_key_encryption_key_id",
                table: "bucket");

            migrationBuilder.DropForeignKey(
                name: "FK_bucket_asset_asset_asset_id",
                table: "bucket_asset");

            migrationBuilder.DropForeignKey(
                name: "FK_bucket_asset_bucket_bucket_id",
                table: "bucket_asset");

            migrationBuilder.DropForeignKey(
                name: "FK_bucket_source_bucket_input_bucket_id",
                table: "bucket_source");

            migrationBuilder.DropForeignKey(
                name: "FK_bucket_source_bucket_output_bucket_id",
                table: "bucket_source");

            migrationBuilder.DropForeignKey(
                name: "FK_bucket_tag_bucket_bucket_id",
                table: "bucket_tag");

            migrationBuilder.DropForeignKey(
                name: "FK_job_asset_input_asset_id",
                table: "job");

            migrationBuilder.DropForeignKey(
                name: "FK_job_job_type_job_type_id",
                table: "job");

            migrationBuilder.DropForeignKey(
                name: "FK_job_processor_node_assigned_processor_node_id",
                table: "job");

            migrationBuilder.DropForeignKey(
                name: "FK_job_run_job_job_id",
                table: "job_run");

            migrationBuilder.DropForeignKey(
                name: "FK_job_run_processor_node_processor_node_id",
                table: "job_run");

            migrationBuilder.DropForeignKey(
                name: "FK_job_run_status_job_run_job_execution_id",
                table: "job_run_status");

            migrationBuilder.DropForeignKey(
                name: "FK_processor_node_status_processor_node_processor_node_id",
                table: "processor_node_status");

            migrationBuilder.AddForeignKey(
                name: "FK_asset_job_run_job_run_id",
                table: "asset",
                column: "job_run_id",
                principalTable: "job_run",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_asset_tag_asset_asset_id",
                table: "asset_tag",
                column: "asset_id",
                principalTable: "asset",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_bucket_bucket_group_bucket_group_id",
                table: "bucket",
                column: "bucket_group_id",
                principalTable: "bucket_group",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_bucket_encryption_key_encryption_key_id",
                table: "bucket",
                column: "encryption_key_id",
                principalTable: "encryption_key",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_bucket_asset_asset_asset_id",
                table: "bucket_asset",
                column: "asset_id",
                principalTable: "asset",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_bucket_asset_bucket_bucket_id",
                table: "bucket_asset",
                column: "bucket_id",
                principalTable: "bucket",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_bucket_source_bucket_input_bucket_id",
                table: "bucket_source",
                column: "input_bucket_id",
                principalTable: "bucket",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_bucket_source_bucket_output_bucket_id",
                table: "bucket_source",
                column: "output_bucket_id",
                principalTable: "bucket",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_bucket_tag_bucket_bucket_id",
                table: "bucket_tag",
                column: "bucket_id",
                principalTable: "bucket",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_job_asset_input_asset_id",
                table: "job",
                column: "input_asset_id",
                principalTable: "asset",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_job_job_type_job_type_id",
                table: "job",
                column: "job_type_id",
                principalTable: "job_type",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_job_processor_node_assigned_processor_node_id",
                table: "job",
                column: "assigned_processor_node_id",
                principalTable: "processor_node",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_job_run_job_job_id",
                table: "job_run",
                column: "job_id",
                principalTable: "job",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_job_run_processor_node_processor_node_id",
                table: "job_run",
                column: "processor_node_id",
                principalTable: "processor_node",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_job_run_status_job_run_job_execution_id",
                table: "job_run_status",
                column: "job_execution_id",
                principalTable: "job_run",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_processor_node_status_processor_node_processor_node_id",
                table: "processor_node_status",
                column: "processor_node_id",
                principalTable: "processor_node",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
