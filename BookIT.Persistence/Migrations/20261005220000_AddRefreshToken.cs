using BookIT.Persistence.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BookIT.Persistence.Migrations;

[DbContext(typeof(BookItDbContext))]
[Migration("20261005220000_AddRefreshToken")]
public partial class AddRefreshToken : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "refresh_token",
            columns: table => new
            {
                id = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                staff_id = table.Column<long>(type: "bigint", nullable: false),
                token_hash = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                expires_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                revoked_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_refresh_token", x => x.id);
                table.ForeignKey(
                    name: "fk_refresh_token_staff_staff_id",
                    column: x => x.staff_id,
                    principalTable: "staff",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "ix_refresh_token_staff_id",
            table: "refresh_token",
            column: "staff_id");

        migrationBuilder.CreateIndex(
            name: "ix_refresh_token_token_hash",
            table: "refresh_token",
            column: "token_hash",
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "refresh_token");
    }
}
