using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TimeTracking.Dal.Impl.Migrations
{
    public partial class ChangedLogTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(   
                name: "TimeSpent",
                table: "WorkLogs");
            migrationBuilder.AddColumn<long>(  
                name: "TimeSpent",
                table: "WorkLogs",
                nullable: false
                );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "TimeSpent",
                table: "WorkLogs",
                type: "time",
                nullable: false,
                oldClrType: typeof(long));
        }
    }
}
