using Microsoft.EntityFrameworkCore.Migrations;

namespace Delta.AppServer.Migrations
{
    public partial class JobProcessorVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_job_processor_type_processor_type_id",
                table: "job");

            migrationBuilder.DropIndex(
                name: "IX_job_processor_type_id",
                table: "job");

            migrationBuilder.DropColumn(
                name: "processor_type_id",
                table: "job");

            migrationBuilder.AddColumn<long>(
                name: "processor_version_id",
                table: "job",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_job_processor_version_id",
                table: "job",
                column: "processor_version_id");

            migrationBuilder.AddForeignKey(
                name: "FK_job_processor_version_processor_version_id",
                table: "job",
                column: "processor_version_id",
                principalTable: "processor_version",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_job_processor_version_processor_version_id",
                table: "job");

            migrationBuilder.DropIndex(
                name: "IX_job_processor_version_id",
                table: "job");

            migrationBuilder.DropColumn(
                name: "processor_version_id",
                table: "job");

            migrationBuilder.AddColumn<long>(
                name: "processor_type_id",
                table: "job",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_job_processor_type_id",
                table: "job",
                column: "processor_type_id");

            migrationBuilder.AddForeignKey(
                name: "FK_job_processor_type_processor_type_id",
                table: "job",
                column: "processor_type_id",
                principalTable: "processor_type",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
