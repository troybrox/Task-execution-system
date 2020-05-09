using Microsoft.EntityFrameworkCore.Migrations;

namespace TaskExecutionSystem.DAL.Migrations
{
    public partial class Repo_Update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "RepositoryModels",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Paragraphs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "RepositoryModels");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Paragraphs");
        }
    }
}
