using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Delta.AppServer.Migrations
{
    /// <inheritdoc />
    public partial class AddBucketGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "bucket_group_id",
                table: "bucket",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "bucket_group",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bucket_group", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_bucket_bucket_group_id",
                table: "bucket",
                column: "bucket_group_id");

            migrationBuilder.AddForeignKey(
                name: "FK_bucket_bucket_group_bucket_group_id",
                table: "bucket",
                column: "bucket_group_id",
                principalTable: "bucket_group",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bucket_bucket_group_bucket_group_id",
                table: "bucket");

            migrationBuilder.DropTable(
                name: "bucket_group");

            migrationBuilder.DropIndex(
                name: "IX_bucket_bucket_group_id",
                table: "bucket");

            migrationBuilder.DropColumn(
                name: "bucket_group_id",
                table: "bucket");
        }
    }
}
