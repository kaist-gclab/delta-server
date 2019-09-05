using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Delta.AppServer.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "asset_format",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    key = table.Column<string>(nullable: false),
                    name = table.Column<string>(nullable: false),
                    description = table.Column<string>(nullable: false),
                    file_extension = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_asset_format", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "asset_type",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    key = table.Column<string>(nullable: false),
                    name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_asset_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "encryption_key",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    name = table.Column<string>(nullable: false),
                    value = table.Column<string>(nullable: true),
                    enabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_encryption_key", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "processor_type",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    key = table.Column<string>(nullable: false),
                    name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_processor_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "processor_version",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    processor_type_id = table.Column<long>(nullable: false),
                    key = table.Column<string>(nullable: false),
                    description = table.Column<string>(nullable: false),
                    created_at = table.Column<Instant>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_processor_version", x => x.id);
                    table.ForeignKey(
                        name: "FK_processor_version_processor_type_processor_type_id",
                        column: x => x.processor_type_id,
                        principalTable: "processor_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "processor_node",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    processor_version_id = table.Column<long>(nullable: false),
                    key = table.Column<string>(nullable: false),
                    name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_processor_node", x => x.id);
                    table.ForeignKey(
                        name: "FK_processor_node_processor_version_processor_version_id",
                        column: x => x.processor_version_id,
                        principalTable: "processor_version",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "processor_version_input_capability",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    processor_version_id = table.Column<long>(nullable: false),
                    asset_format_id = table.Column<long>(nullable: false),
                    asset_type_id = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_processor_version_input_capability", x => x.id);
                    table.ForeignKey(
                        name: "FK_processor_version_input_capability_asset_format_asset_forma~",
                        column: x => x.asset_format_id,
                        principalTable: "asset_format",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_processor_version_input_capability_asset_type_asset_type_id",
                        column: x => x.asset_type_id,
                        principalTable: "asset_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_processor_version_input_capability_processor_version_proces~",
                        column: x => x.processor_version_id,
                        principalTable: "processor_version",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "processor_node_status",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    processor_node_id = table.Column<long>(nullable: false),
                    timestamp = table.Column<Instant>(nullable: false),
                    status = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_processor_node_status", x => x.id);
                    table.ForeignKey(
                        name: "FK_processor_node_status_processor_node_processor_node_id",
                        column: x => x.processor_node_id,
                        principalTable: "processor_node",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "asset_tag",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    asset_id = table.Column<long>(nullable: false),
                    key = table.Column<string>(nullable: false),
                    value = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_asset_tag", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "job",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    processor_type_id = table.Column<long>(nullable: false),
                    input_asset_id = table.Column<long>(nullable: true),
                    job_arguments = table.Column<string>(nullable: false),
                    created_at = table.Column<Instant>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job", x => x.id);
                    table.ForeignKey(
                        name: "FK_job_processor_type_processor_type_id",
                        column: x => x.processor_type_id,
                        principalTable: "processor_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "job_execution",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    job_id = table.Column<long>(nullable: false),
                    processor_node_id = table.Column<long>(nullable: false)
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

            migrationBuilder.CreateTable(
                name: "asset",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    asset_format_id = table.Column<long>(nullable: true),
                    encryption_key_id = table.Column<long>(nullable: true),
                    asset_type_id = table.Column<long>(nullable: true),
                    store_key = table.Column<string>(nullable: true),
                    parent_job_execution_id = table.Column<long>(nullable: true),
                    created_at = table.Column<Instant>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_asset", x => x.id);
                    table.ForeignKey(
                        name: "FK_asset_asset_format_asset_format_id",
                        column: x => x.asset_format_id,
                        principalTable: "asset_format",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_asset_asset_type_asset_type_id",
                        column: x => x.asset_type_id,
                        principalTable: "asset_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_asset_encryption_key_encryption_key_id",
                        column: x => x.encryption_key_id,
                        principalTable: "encryption_key",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_asset_job_execution_parent_job_execution_id",
                        column: x => x.parent_job_execution_id,
                        principalTable: "job_execution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "job_execution_status",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    job_execution_id = table.Column<long>(nullable: false),
                    timestamp = table.Column<Instant>(nullable: false),
                    status = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job_execution_status", x => x.id);
                    table.ForeignKey(
                        name: "FK_job_execution_status_job_execution_job_execution_id",
                        column: x => x.job_execution_id,
                        principalTable: "job_execution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_asset_asset_format_id",
                table: "asset",
                column: "asset_format_id");

            migrationBuilder.CreateIndex(
                name: "IX_asset_asset_type_id",
                table: "asset",
                column: "asset_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_asset_encryption_key_id",
                table: "asset",
                column: "encryption_key_id");

            migrationBuilder.CreateIndex(
                name: "IX_asset_parent_job_execution_id",
                table: "asset",
                column: "parent_job_execution_id");

            migrationBuilder.CreateIndex(
                name: "IX_asset_tag_asset_id",
                table: "asset_tag",
                column: "asset_id");

            migrationBuilder.CreateIndex(
                name: "IX_job_input_asset_id",
                table: "job",
                column: "input_asset_id");

            migrationBuilder.CreateIndex(
                name: "IX_job_processor_type_id",
                table: "job",
                column: "processor_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_job_execution_job_id",
                table: "job_execution",
                column: "job_id");

            migrationBuilder.CreateIndex(
                name: "IX_job_execution_processor_node_id",
                table: "job_execution",
                column: "processor_node_id");

            migrationBuilder.CreateIndex(
                name: "IX_job_execution_status_job_execution_id",
                table: "job_execution_status",
                column: "job_execution_id");

            migrationBuilder.CreateIndex(
                name: "IX_processor_node_processor_version_id",
                table: "processor_node",
                column: "processor_version_id");

            migrationBuilder.CreateIndex(
                name: "IX_processor_node_status_processor_node_id",
                table: "processor_node_status",
                column: "processor_node_id");

            migrationBuilder.CreateIndex(
                name: "IX_processor_version_processor_type_id",
                table: "processor_version",
                column: "processor_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_processor_version_input_capability_asset_format_id",
                table: "processor_version_input_capability",
                column: "asset_format_id");

            migrationBuilder.CreateIndex(
                name: "IX_processor_version_input_capability_asset_type_id",
                table: "processor_version_input_capability",
                column: "asset_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_processor_version_input_capability_processor_version_id",
                table: "processor_version_input_capability",
                column: "processor_version_id");

            migrationBuilder.AddForeignKey(
                name: "FK_asset_tag_asset_asset_id",
                table: "asset_tag",
                column: "asset_id",
                principalTable: "asset",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_job_asset_input_asset_id",
                table: "job",
                column: "input_asset_id",
                principalTable: "asset",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_asset_asset_format_asset_format_id",
                table: "asset");

            migrationBuilder.DropForeignKey(
                name: "FK_asset_asset_type_asset_type_id",
                table: "asset");

            migrationBuilder.DropForeignKey(
                name: "FK_asset_encryption_key_encryption_key_id",
                table: "asset");

            migrationBuilder.DropForeignKey(
                name: "FK_asset_job_execution_parent_job_execution_id",
                table: "asset");

            migrationBuilder.DropTable(
                name: "asset_tag");

            migrationBuilder.DropTable(
                name: "job_execution_status");

            migrationBuilder.DropTable(
                name: "processor_node_status");

            migrationBuilder.DropTable(
                name: "processor_version_input_capability");

            migrationBuilder.DropTable(
                name: "asset_format");

            migrationBuilder.DropTable(
                name: "asset_type");

            migrationBuilder.DropTable(
                name: "encryption_key");

            migrationBuilder.DropTable(
                name: "job_execution");

            migrationBuilder.DropTable(
                name: "job");

            migrationBuilder.DropTable(
                name: "processor_node");

            migrationBuilder.DropTable(
                name: "asset");

            migrationBuilder.DropTable(
                name: "processor_version");

            migrationBuilder.DropTable(
                name: "processor_type");
        }
    }
}
