using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Delta.AppServer.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAssetType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_asset_asset_type_asset_type_id",
                table: "asset");

            migrationBuilder.DropForeignKey(
                name: "FK_processor_node_capability_asset_type_asset_type_id",
                table: "processor_node_capability");

            migrationBuilder.DropTable(
                name: "asset_type");

            migrationBuilder.DropIndex(
                name: "IX_processor_node_capability_asset_type_id",
                table: "processor_node_capability");

            migrationBuilder.DropIndex(
                name: "IX_asset_asset_type_id",
                table: "asset");

            migrationBuilder.DropColumn(
                name: "asset_type_id",
                table: "processor_node_capability");

            migrationBuilder.DropColumn(
                name: "asset_type_id",
                table: "asset");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "asset_type_id",
                table: "processor_node_capability",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "asset_type_id",
                table: "asset",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "asset_type",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    key = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_asset_type", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_processor_node_capability_asset_type_id",
                table: "processor_node_capability",
                column: "asset_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_asset_asset_type_id",
                table: "asset",
                column: "asset_type_id");

            migrationBuilder.AddForeignKey(
                name: "FK_asset_asset_type_asset_type_id",
                table: "asset",
                column: "asset_type_id",
                principalTable: "asset_type",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_processor_node_capability_asset_type_asset_type_id",
                table: "processor_node_capability",
                column: "asset_type_id",
                principalTable: "asset_type",
                principalColumn: "id");
        }
    }
}
