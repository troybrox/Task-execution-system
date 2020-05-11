using Microsoft.EntityFrameworkCore.Migrations;

namespace TaskExecutionSystem.DAL.Migrations
{
    public partial class Repo_Multifile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RepoFiles_RepositoryModelId",
                table: "RepoFiles");

            migrationBuilder.CreateIndex(
                name: "IX_RepoFiles_RepositoryModelId",
                table: "RepoFiles",
                column: "RepositoryModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RepoFiles_RepositoryModelId",
                table: "RepoFiles");

            migrationBuilder.CreateIndex(
                name: "IX_RepoFiles_RepositoryModelId",
                table: "RepoFiles",
                column: "RepositoryModelId",
                unique: true);
        }
    }
}
