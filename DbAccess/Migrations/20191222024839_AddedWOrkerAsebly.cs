using Microsoft.EntityFrameworkCore.Migrations;

namespace Exico.HF.DbAccess.Migrations
{
    public partial class AddedWOrkerAsebly : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkerClass",
                table: "HfUserJob");

            migrationBuilder.AddColumn<string>(
                name: "WorkerAssemblyName",
                table: "HfUserJob",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkerClassName",
                table: "HfUserJob",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkerAssemblyName",
                table: "HfUserJob");

            migrationBuilder.DropColumn(
                name: "WorkerClassName",
                table: "HfUserJob");

            migrationBuilder.AddColumn<string>(
                name: "WorkerClass",
                table: "HfUserJob",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
