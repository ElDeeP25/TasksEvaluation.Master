using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TasksEvaluation.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DataBasee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                schema: "security",
                table: "Users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "AdminAccepted",
                schema: "security",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                schema: "security",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AdminAccepted",
                schema: "security",
                table: "Users");
        }
    }
}
