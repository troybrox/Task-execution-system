using Microsoft.EntityFrameworkCore.Migrations;

namespace TaskExecutionSystem.DAL.Migrations
{
    public partial class RepoFile_Update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RepoFiles_Paragraphs_ParagraphId",
                table: "RepoFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_RepoFiles_RepositoryModels_RepositoryItemId",
                table: "RepoFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_RepoFiles_Themes_ThemeId",
                table: "RepoFiles");

            migrationBuilder.DropIndex(
                name: "IX_RepoFiles_ParagraphId",
                table: "RepoFiles");

            migrationBuilder.DropIndex(
                name: "IX_RepoFiles_RepositoryItemId",
                table: "RepoFiles");

            migrationBuilder.DropIndex(
                name: "IX_RepoFiles_ThemeId",
                table: "RepoFiles");

            migrationBuilder.DropColumn(
                name: "ParagraphId",
                table: "RepoFiles");

            migrationBuilder.DropColumn(
                name: "RepositoryItemId",
                table: "RepoFiles");

            migrationBuilder.DropColumn(
                name: "ThemeId",
                table: "RepoFiles");

            migrationBuilder.AddColumn<int>(
                name: "FileId",
                table: "Themes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepositoryModelId",
                table: "RepoFiles",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FileId",
                table: "Paragraphs",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Themes_FileId",
                table: "Themes",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_RepoFiles_RepositoryModelId",
                table: "RepoFiles",
                column: "RepositoryModelId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Paragraphs_FileId",
                table: "Paragraphs",
                column: "FileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Paragraphs_RepoFiles_FileId",
                table: "Paragraphs",
                column: "FileId",
                principalTable: "RepoFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RepoFiles_RepositoryModels_RepositoryModelId",
                table: "RepoFiles",
                column: "RepositoryModelId",
                principalTable: "RepositoryModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Themes_RepoFiles_FileId",
                table: "Themes",
                column: "FileId",
                principalTable: "RepoFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Paragraphs_RepoFiles_FileId",
                table: "Paragraphs");

            migrationBuilder.DropForeignKey(
                name: "FK_RepoFiles_RepositoryModels_RepositoryModelId",
                table: "RepoFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Themes_RepoFiles_FileId",
                table: "Themes");

            migrationBuilder.DropIndex(
                name: "IX_Themes_FileId",
                table: "Themes");

            migrationBuilder.DropIndex(
                name: "IX_RepoFiles_RepositoryModelId",
                table: "RepoFiles");

            migrationBuilder.DropIndex(
                name: "IX_Paragraphs_FileId",
                table: "Paragraphs");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "Themes");

            migrationBuilder.DropColumn(
                name: "RepositoryModelId",
                table: "RepoFiles");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "Paragraphs");

            migrationBuilder.AddColumn<int>(
                name: "ParagraphId",
                table: "RepoFiles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepositoryItemId",
                table: "RepoFiles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ThemeId",
                table: "RepoFiles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RepoFiles_ParagraphId",
                table: "RepoFiles",
                column: "ParagraphId",
                unique: true,
                filter: "[ParagraphId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RepoFiles_RepositoryItemId",
                table: "RepoFiles",
                column: "RepositoryItemId",
                unique: true,
                filter: "[RepositoryItemId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RepoFiles_ThemeId",
                table: "RepoFiles",
                column: "ThemeId",
                unique: true,
                filter: "[ThemeId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_RepoFiles_Paragraphs_ParagraphId",
                table: "RepoFiles",
                column: "ParagraphId",
                principalTable: "Paragraphs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RepoFiles_RepositoryModels_RepositoryItemId",
                table: "RepoFiles",
                column: "RepositoryItemId",
                principalTable: "RepositoryModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RepoFiles_Themes_ThemeId",
                table: "RepoFiles",
                column: "ThemeId",
                principalTable: "Themes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
