using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogProject.Migrations
{
    /// <inheritdoc />
    public partial class AppUserAddDeletedBy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                schema: "ef",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "ef",
                table: "AspNetUsers");
        }
    }
}
