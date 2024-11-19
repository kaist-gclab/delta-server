using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Delta.AppServer.Migrations
{
    /// <inheritdoc />
    public partial class AddBucketCreatedByJobRun : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "created_by_job_run_id",
                table: "bucket",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_bucket_created_by_job_run_id",
                table: "bucket",
                column: "created_by_job_run_id");

            migrationBuilder.AddForeignKey(
                name: "FK_bucket_job_run_created_by_job_run_id",
                table: "bucket",
                column: "created_by_job_run_id",
                principalTable: "job_run",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bucket_job_run_created_by_job_run_id",
                table: "bucket");

            migrationBuilder.DropIndex(
                name: "IX_bucket_created_by_job_run_id",
                table: "bucket");

            migrationBuilder.DropColumn(
                name: "created_by_job_run_id",
                table: "bucket");
        }
    }
}
