using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockScribe.Api.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUserPersonaKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "persona_key",
                table: "users",
                type: "character varying(40)",
                maxLength: 40,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_users_persona_key",
                table: "users",
                column: "persona_key",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_users_persona_key",
                table: "users");

            migrationBuilder.DropColumn(
                name: "persona_key",
                table: "users");
        }
    }
}
