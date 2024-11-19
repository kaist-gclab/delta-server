using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Delta.AppServer.Migrations
{
    /// <inheritdoc />
    public partial class AddBucketAsset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_asset_job_execution_parent_job_execution_id",
                table: "asset");

            migrationBuilder.DropColumn(
                name: "media_type",
                table: "asset");

            migrationBuilder.RenameColumn(
                name: "parent_job_execution_id",
                table: "asset",
                newName: "job_execution_id");

            migrationBuilder.RenameIndex(
                name: "IX_asset_parent_job_execution_id",
                table: "asset",
                newName: "IX_asset_job_execution_id");

            migrationBuilder.CreateTable(
                name: "bucket_asset",
                columns: table => new
                {
                    path = table.Column<string>(type: "text", nullable: false),
                    bucket_id = table.Column<long>(type: "bigint", nullable: false),
                    asset_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bucket_asset", x => new { x.bucket_id, x.path });
                    table.ForeignKey(
                        name: "FK_bucket_asset_asset_asset_id",
                        column: x => x.asset_id,
                        principalTable: "asset",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bucket_asset_bucket_bucket_id",
                        column: x => x.bucket_id,
                        principalTable: "bucket",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_bucket_asset_asset_id",
                table: "bucket_asset",
                column: "asset_id");

            migrationBuilder.AddForeignKey(
                name: "FK_asset_job_execution_job_execution_id",
                table: "asset",
                column: "job_execution_id",
                principalTable: "job_execution",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_asset_job_execution_job_execution_id",
                table: "asset");

            migrationBuilder.DropTable(
                name: "bucket_asset");

            migrationBuilder.RenameColumn(
                name: "job_execution_id",
                table: "asset",
                newName: "parent_job_execution_id");

            migrationBuilder.RenameIndex(
                name: "IX_asset_job_execution_id",
                table: "asset",
                newName: "IX_asset_parent_job_execution_id");

            migrationBuilder.AddColumn<string>(
                name: "media_type",
                table: "asset",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_asset_job_execution_parent_job_execution_id",
                table: "asset",
                column: "parent_job_execution_id",
                principalTable: "job_execution",
                principalColumn: "id");
        }
    }
}
