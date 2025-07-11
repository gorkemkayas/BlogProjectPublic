using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AppUserPropertiesUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AppUserId",
                schema: "ef",
                table: "Posts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                schema: "ef",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                schema: "ef",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentPosition",
                schema: "ef",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GithubAddress",
                schema: "ef",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HighSchoolGraduationYear",
                schema: "ef",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HighSchoolName",
                schema: "ef",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HighSchoolStartYear",
                schema: "ef",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkedinAddress",
                schema: "ef",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MediumAddress",
                schema: "ef",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PersonalWebAddress",
                schema: "ef",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UniversityGraduationYear",
                schema: "ef",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UniversityName",
                schema: "ef",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UniversityStartYear",
                schema: "ef",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "XAddress",
                schema: "ef",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "YoutubeAddress",
                schema: "ef",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_AppUserId",
                schema: "ef",
                table: "Posts",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_AppUserId",
                schema: "ef",
                table: "Posts",
                column: "AppUserId",
                principalSchema: "ef",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_AppUserId",
                schema: "ef",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_AppUserId",
                schema: "ef",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                schema: "ef",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Address",
                schema: "ef",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "City",
                schema: "ef",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CurrentPosition",
                schema: "ef",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "GithubAddress",
                schema: "ef",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "HighSchoolGraduationYear",
                schema: "ef",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "HighSchoolName",
                schema: "ef",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "HighSchoolStartYear",
                schema: "ef",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LinkedinAddress",
                schema: "ef",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MediumAddress",
                schema: "ef",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PersonalWebAddress",
                schema: "ef",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UniversityGraduationYear",
                schema: "ef",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UniversityName",
                schema: "ef",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UniversityStartYear",
                schema: "ef",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "XAddress",
                schema: "ef",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "YoutubeAddress",
                schema: "ef",
                table: "AspNetUsers");
        }
    }
}
