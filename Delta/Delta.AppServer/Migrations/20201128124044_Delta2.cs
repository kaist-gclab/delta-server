using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Delta.AppServer.Migrations
{
    public partial class Delta2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_asset_asset_format_asset_format_id",
                table: "asset");

            migrationBuilder.DropForeignKey(
                name: "FK_asset_asset_type_asset_type_id",
                table: "asset");

            migrationBuilder.DropForeignKey(
                name: "FK_job_processor_version_processor_version_id",
                table: "job");

            migrationBuilder.DropForeignKey(
                name: "FK_processor_node_processor_version_processor_version_id",
                table: "processor_node");

            migrationBuilder.DropTable(
                name: "processor_version_input_capability");

            migrationBuilder.DropTable(
                name: "asset_format");

            migrationBuilder.DropTable(
                name: "processor_version");

            migrationBuilder.DropTable(
                name: "processor_type");

            migrationBuilder.DropIndex(
                name: "IX_processor_node_processor_version_id",
                table: "processor_node");

            migrationBuilder.DropIndex(
                name: "IX_asset_asset_format_id",
                table: "asset");

            migrationBuilder.DropColumn(
                name: "asset_format_id",
                table: "asset");

            migrationBuilder.RenameColumn(
                name: "processor_version_id",
                table: "job",
                newName: "job_type_id");

            migrationBuilder.RenameIndex(
                name: "IX_job_processor_version_id",
                table: "job",
                newName: "IX_job_job_type_id");

            migrationBuilder.AddColumn<long>(
                name: "assigned_processor_node_id",
                table: "job",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "store_key",
                table: "asset",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "asset_type_id",
                table: "asset",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "media_type",
                table: "asset",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "job_type",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    key = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "setting",
                columns: table => new
                {
                    key = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_setting", x => x.key);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    username = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: true),
                    salt = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "processor_node_capability",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    processor_node_id = table.Column<long>(type: "bigint", nullable: false),
                    job_type_id = table.Column<long>(type: "bigint", nullable: false),
                    asset_type_id = table.Column<long>(type: "bigint", nullable: true),
                    media_type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_processor_node_capability", x => x.id);
                    table.ForeignKey(
                        name: "FK_processor_node_capability_asset_type_asset_type_id",
                        column: x => x.asset_type_id,
                        principalTable: "asset_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "IX_job_assigned_processor_node_id",
                table: "job",
                column: "assigned_processor_node_id");

            migrationBuilder.CreateIndex(
                name: "IX_processor_node_capability_asset_type_id",
                table: "processor_node_capability",
                column: "asset_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_processor_node_capability_job_type_id",
                table: "processor_node_capability",
                column: "job_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_processor_node_capability_processor_node_id",
                table: "processor_node_capability",
                column: "processor_node_id");

            migrationBuilder.AddForeignKey(
                name: "FK_asset_asset_type_asset_type_id",
                table: "asset",
                column: "asset_type_id",
                principalTable: "asset_type",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

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
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_asset_asset_type_asset_type_id",
                table: "asset");

            migrationBuilder.DropForeignKey(
                name: "FK_job_job_type_job_type_id",
                table: "job");

            migrationBuilder.DropForeignKey(
                name: "FK_job_processor_node_assigned_processor_node_id",
                table: "job");

            migrationBuilder.DropTable(
                name: "processor_node_capability");

            migrationBuilder.DropTable(
                name: "setting");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "job_type");

            migrationBuilder.DropIndex(
                name: "IX_job_assigned_processor_node_id",
                table: "job");

            migrationBuilder.DropColumn(
                name: "assigned_processor_node_id",
                table: "job");

            migrationBuilder.DropColumn(
                name: "media_type",
                table: "asset");

            migrationBuilder.RenameColumn(
                name: "job_type_id",
                table: "job",
                newName: "processor_version_id");

            migrationBuilder.RenameIndex(
                name: "IX_job_job_type_id",
                table: "job",
                newName: "IX_job_processor_version_id");

            migrationBuilder.AlterColumn<string>(
                name: "store_key",
                table: "asset",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<long>(
                name: "asset_type_id",
                table: "asset",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "asset_format_id",
                table: "asset",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "asset_format",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "text", nullable: false),
                    file_extension = table.Column<string>(type: "text", nullable: true),
                    key = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_asset_format", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "processor_type",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    key = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_processor_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "processor_version",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<Instant>(type: "timestamp", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    key = table.Column<string>(type: "text", nullable: false),
                    processor_type_id = table.Column<long>(type: "bigint", nullable: false)
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
                name: "processor_version_input_capability",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    asset_format_id = table.Column<long>(type: "bigint", nullable: true),
                    asset_type_id = table.Column<long>(type: "bigint", nullable: true),
                    processor_version_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_processor_version_input_capability", x => x.id);
                    table.ForeignKey(
                        name: "FK_processor_version_input_capability_asset_format_asset_forma~",
                        column: x => x.asset_format_id,
                        principalTable: "asset_format",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
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

            migrationBuilder.CreateIndex(
                name: "IX_processor_node_processor_version_id",
                table: "processor_node",
                column: "processor_version_id");

            migrationBuilder.CreateIndex(
                name: "IX_asset_asset_format_id",
                table: "asset",
                column: "asset_format_id");

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
                name: "FK_asset_asset_format_asset_format_id",
                table: "asset",
                column: "asset_format_id",
                principalTable: "asset_format",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_asset_asset_type_asset_type_id",
                table: "asset",
                column: "asset_type_id",
                principalTable: "asset_type",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_job_processor_version_processor_version_id",
                table: "job",
                column: "processor_version_id",
                principalTable: "processor_version",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_processor_node_processor_version_processor_version_id",
                table: "processor_node",
                column: "processor_version_id",
                principalTable: "processor_version",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
