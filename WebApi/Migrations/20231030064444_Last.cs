using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class Last : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "localdata",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndingDate",
                table: "localdata",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "localdata",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Price",
                table: "localdata",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoomNo",
                table: "localdata",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartingDate",
                table: "localdata",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "localdata",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "localdata");

            migrationBuilder.DropColumn(
                name: "EndingDate",
                table: "localdata");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "localdata");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "localdata");

            migrationBuilder.DropColumn(
                name: "RoomNo",
                table: "localdata");

            migrationBuilder.DropColumn(
                name: "StartingDate",
                table: "localdata");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "localdata");
        }
    }
}
