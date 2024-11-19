using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Delta.AppServer.Migrations
{
    /// <inheritdoc />
    public partial class AddEncryptionKeyIndexName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_asset_encryption_key_encryption_key_id",
                table: "asset");

            migrationBuilder.DropIndex(
                name: "IX_asset_encryption_key_id",
                table: "asset");

            migrationBuilder.DropColumn(
                name: "encryption_key_id",
                table: "asset");

            migrationBuilder.CreateIndex(
                name: "IX_encryption_key_name",
                table: "encryption_key",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_encryption_key_name",
                table: "encryption_key");

            migrationBuilder.AddColumn<long>(
                name: "encryption_key_id",
                table: "asset",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_asset_encryption_key_id",
                table: "asset",
                column: "encryption_key_id");

            migrationBuilder.AddForeignKey(
                name: "FK_asset_encryption_key_encryption_key_id",
                table: "asset",
                column: "encryption_key_id",
                principalTable: "encryption_key",
                principalColumn: "id");
        }
    }
}
