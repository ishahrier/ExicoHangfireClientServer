using Microsoft.EntityFrameworkCore.Migrations;

namespace Exico.HF.DbAccess.Migrations
{
    public partial class ChangeRecTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LastHfJobId",
                table: "HfUserRecurringJob",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "LastHfJobId",
                table: "HfUserRecurringJob",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
