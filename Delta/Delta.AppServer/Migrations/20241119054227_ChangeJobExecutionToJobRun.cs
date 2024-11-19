using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Delta.AppServer.Migrations
{
    /// <inheritdoc />
    public partial class ChangeJobExecutionToJobRun : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_asset_job_execution_job_execution_id",
                table: "asset");

            migrationBuilder.DropForeignKey(
                name: "FK_job_execution_status_job_execution_job_execution_id",
                table: "job_execution_status");

            migrationBuilder.DropTable(
                name: "job_execution");

            migrationBuilder.RenameColumn(
                name: "job_execution_id",
                table: "asset",
                newName: "job_run_id");

            migrationBuilder.RenameIndex(
                name: "IX_asset_job_execution_id",
                table: "asset",
                newName: "IX_asset_job_run_id");

            migrationBuilder.CreateTable(
                name: "job_run",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    job_id = table.Column<long>(type: "bigint", nullable: false),
                    processor_node_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job_run", x => x.id);
                    table.ForeignKey(
                        name: "FK_job_run_job_job_id",
                        column: x => x.job_id,
                        principalTable: "job",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_job_run_processor_node_processor_node_id",
                        column: x => x.processor_node_id,
                        principalTable: "processor_node",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_job_run_job_id",
                table: "job_run",
                column: "job_id");

            migrationBuilder.CreateIndex(
                name: "IX_job_run_processor_node_id",
                table: "job_run",
                column: "processor_node_id");

            migrationBuilder.AddForeignKey(
                name: "FK_asset_job_run_job_run_id",
                table: "asset",
                column: "job_run_id",
                principalTable: "job_run",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_job_execution_status_job_run_job_execution_id",
                table: "job_execution_status",
                column: "job_execution_id",
                principalTable: "job_run",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_asset_job_run_job_run_id",
                table: "asset");

            migrationBuilder.DropForeignKey(
                name: "FK_job_execution_status_job_run_job_execution_id",
                table: "job_execution_status");

            migrationBuilder.DropTable(
                name: "job_run");

            migrationBuilder.RenameColumn(
                name: "job_run_id",
                table: "asset",
                newName: "job_execution_id");

            migrationBuilder.RenameIndex(
                name: "IX_asset_job_run_id",
                table: "asset",
                newName: "IX_asset_job_execution_id");

            migrationBuilder.CreateTable(
                name: "job_execution",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    job_id = table.Column<long>(type: "bigint", nullable: false),
                    processor_node_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job_execution", x => x.id);
                    table.ForeignKey(
                        name: "FK_job_execution_job_job_id",
                        column: x => x.job_id,
                        principalTable: "job",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_job_execution_processor_node_processor_node_id",
                        column: x => x.processor_node_id,
                        principalTable: "processor_node",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_job_execution_job_id",
                table: "job_execution",
                column: "job_id");

            migrationBuilder.CreateIndex(
                name: "IX_job_execution_processor_node_id",
                table: "job_execution",
                column: "processor_node_id");

            migrationBuilder.AddForeignKey(
                name: "FK_asset_job_execution_job_execution_id",
                table: "asset",
                column: "job_execution_id",
                principalTable: "job_execution",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_job_execution_status_job_execution_job_execution_id",
                table: "job_execution_status",
                column: "job_execution_id",
                principalTable: "job_execution",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
