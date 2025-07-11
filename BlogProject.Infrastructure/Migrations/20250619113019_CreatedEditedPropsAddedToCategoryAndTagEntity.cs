using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreatedEditedPropsAddedToCategoryAndTagEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "ef",
                table: "Tags",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                schema: "ef",
                table: "Tags",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "EditedBy",
                schema: "ef",
                table: "Tags",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EditedDate",
                schema: "ef",
                table: "Tags",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "ef",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                schema: "ef",
                table: "Categories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "EditedBy",
                schema: "ef",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EditedDate",
                schema: "ef",
                table: "Categories",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "ef",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                schema: "ef",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "EditedBy",
                schema: "ef",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "EditedDate",
                schema: "ef",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "ef",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                schema: "ef",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "EditedBy",
                schema: "ef",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "EditedDate",
                schema: "ef",
                table: "Categories");
        }
    }
}
