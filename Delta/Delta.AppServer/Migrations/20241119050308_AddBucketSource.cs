using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Delta.AppServer.Migrations
{
    /// <inheritdoc />
    public partial class AddBucketSource : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "bucket_source",
                columns: table => new
                {
                    input_bucket_id = table.Column<long>(type: "bigint", nullable: false),
                    output_bucket_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bucket_source", x => new { x.input_bucket_id, x.output_bucket_id });
                    table.ForeignKey(
                        name: "FK_bucket_source_bucket_input_bucket_id",
                        column: x => x.input_bucket_id,
                        principalTable: "bucket",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bucket_source_bucket_output_bucket_id",
                        column: x => x.output_bucket_id,
                        principalTable: "bucket",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_bucket_source_output_bucket_id",
                table: "bucket_source",
                column: "output_bucket_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bucket_source");
        }
    }
}
