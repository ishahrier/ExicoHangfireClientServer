using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Exico.HF.DbAccess.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HfUserJob",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    JobType = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    HfJobId = table.Column<string>(nullable: true),
                    WorkDataId = table.Column<int>(nullable: true),
                    WorkerClass = table.Column<string>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    TimeZoneId = table.Column<string>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HfUserJob", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HfUserRecurringJob",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HfUserJobId = table.Column<int>(nullable: false),
                    LastHfJobId = table.Column<int>(nullable: true),
                    LastRun = table.Column<DateTimeOffset>(nullable: true),
                    NextRun = table.Column<DateTimeOffset>(nullable: true),
                    CronExpression = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HfUserRecurringJob", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HfUserRecurringJob_HfUserJob_HfUserJobId",
                        column: x => x.HfUserJobId,
                        principalTable: "HfUserJob",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HfUserScheduledJob",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HfUserJobId = table.Column<int>(nullable: false),
                    ScheduledAt = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HfUserScheduledJob", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HfUserScheduledJob_HfUserJob_HfUserJobId",
                        column: x => x.HfUserJobId,
                        principalTable: "HfUserJob",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HfUserRecurringJob_HfUserJobId",
                table: "HfUserRecurringJob",
                column: "HfUserJobId");

            migrationBuilder.CreateIndex(
                name: "IX_HfUserScheduledJob_HfUserJobId",
                table: "HfUserScheduledJob",
                column: "HfUserJobId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HfUserRecurringJob");

            migrationBuilder.DropTable(
                name: "HfUserScheduledJob");

            migrationBuilder.DropTable(
                name: "HfUserJob");
        }
    }
}
