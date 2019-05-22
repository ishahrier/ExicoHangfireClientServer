﻿using System;
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
                    Id = table.Column<string>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    HfJobId = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HfUserJob", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HfUserJob");
        }
    }
}
