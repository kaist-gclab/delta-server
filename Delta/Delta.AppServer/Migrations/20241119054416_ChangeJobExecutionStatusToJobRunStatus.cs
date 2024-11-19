using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Delta.AppServer.Migrations
{
    /// <inheritdoc />
    public partial class ChangeJobExecutionStatusToJobRunStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "job_execution_status");

            migrationBuilder.CreateTable(
                name: "job_run_status",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    job_execution_id = table.Column<long>(type: "bigint", nullable: false),
                    timestamp = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job_run_status", x => x.id);
                    table.ForeignKey(
                        name: "FK_job_run_status_job_run_job_execution_id",
                        column: x => x.job_execution_id,
                        principalTable: "job_run",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_job_run_status_job_execution_id",
                table: "job_run_status",
                column: "job_execution_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "job_run_status");

            migrationBuilder.CreateTable(
                name: "job_execution_status",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    job_execution_id = table.Column<long>(type: "bigint", nullable: false),
                    status = table.Column<string>(type: "text", nullable: true),
                    timestamp = table.Column<Instant>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job_execution_status", x => x.id);
                    table.ForeignKey(
                        name: "FK_job_execution_status_job_run_job_execution_id",
                        column: x => x.job_execution_id,
                        principalTable: "job_run",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_job_execution_status_job_execution_id",
                table: "job_execution_status",
                column: "job_execution_id");
        }
    }
}
