using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Delta.AppServer.Migrations
{
    /// <inheritdoc />
    public partial class AddBucket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "bucket",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    encryption_key_id = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bucket", x => x.id);
                    table.ForeignKey(
                        name: "FK_bucket_encryption_key_encryption_key_id",
                        column: x => x.encryption_key_id,
                        principalTable: "encryption_key",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_bucket_encryption_key_id",
                table: "bucket",
                column: "encryption_key_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bucket");
        }
    }
}
