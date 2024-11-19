using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Delta.AppServer.Migrations
{
    /// <inheritdoc />
    public partial class RemoveProcessorNodeCapability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "processor_node_capability");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "processor_node_capability",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    job_type_id = table.Column<long>(type: "bigint", nullable: false),
                    processor_node_id = table.Column<long>(type: "bigint", nullable: false),
                    media_type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_processor_node_capability", x => x.id);
                    table.ForeignKey(
                        name: "FK_processor_node_capability_job_type_job_type_id",
                        column: x => x.job_type_id,
                        principalTable: "job_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_processor_node_capability_processor_node_processor_node_id",
                        column: x => x.processor_node_id,
                        principalTable: "processor_node",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_processor_node_capability_job_type_id",
                table: "processor_node_capability",
                column: "job_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_processor_node_capability_processor_node_id",
                table: "processor_node_capability",
                column: "processor_node_id");
        }
    }
}
