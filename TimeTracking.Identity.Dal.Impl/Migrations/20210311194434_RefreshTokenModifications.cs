using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TimeTracking.Identity.Dal.Impl.Migrations
{
    public partial class RefreshTokenModifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "About",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "Revoked",
                table: "RefreshToken",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "Expires",
                table: "RefreshToken",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "Created",
                table: "RefreshToken",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Revoked",
                table: "RefreshToken",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Expires",
                table: "RefreshToken",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTimeOffset));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "RefreshToken",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTimeOffset));

            migrationBuilder.AddColumn<string>(
                name: "About",
                table: "AspNetUsers",
                type: "text",
                nullable: true);
        }
    }
}
