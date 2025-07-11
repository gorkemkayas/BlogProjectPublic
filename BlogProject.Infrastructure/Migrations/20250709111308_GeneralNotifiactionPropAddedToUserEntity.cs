using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class GeneralNotifiactionPropAddedToUserEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "GeneralNotificationActive",
                schema: "ef",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GeneralNotificationActive",
                schema: "ef",
                table: "AspNetUsers");
        }
    }
}
