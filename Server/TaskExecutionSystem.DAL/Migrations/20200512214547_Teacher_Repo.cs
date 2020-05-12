using Microsoft.EntityFrameworkCore.Migrations;

namespace TaskExecutionSystem.DAL.Migrations
{
    public partial class Teacher_Repo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskStudentItems_Students_StudentId",
                table: "TaskStudentItems");

            migrationBuilder.DropTable(
                name: "Paragraphs");

            migrationBuilder.DropTable(
                name: "Themes");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Teachers_UserId",
                table: "Teachers");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Students_UserId",
                table: "Students");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_UserId",
                table: "Teachers",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_UserId",
                table: "Students",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskStudentItems_Students_StudentId",
                table: "TaskStudentItems",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskStudentItems_Students_StudentId",
                table: "TaskStudentItems");

            migrationBuilder.DropIndex(
                name: "IX_Teachers_UserId",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Students_UserId",
                table: "Students");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Teachers_UserId",
                table: "Teachers",
                column: "UserId");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Students_UserId",
                table: "Students",
                column: "UserId");

            migrationBuilder.CreateTable(
                name: "Themes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContentText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RepositoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Themes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Themes_RepoFiles_FileId",
                        column: x => x.FileId,
                        principalTable: "RepoFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Themes_RepositoryModels_RepositoryId",
                        column: x => x.RepositoryId,
                        principalTable: "RepositoryModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Paragraphs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContentText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThemeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paragraphs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Paragraphs_RepoFiles_FileId",
                        column: x => x.FileId,
                        principalTable: "RepoFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Paragraphs_Themes_ThemeId",
                        column: x => x.ThemeId,
                        principalTable: "Themes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Paragraphs_FileId",
                table: "Paragraphs",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Paragraphs_ThemeId",
                table: "Paragraphs",
                column: "ThemeId");

            migrationBuilder.CreateIndex(
                name: "IX_Themes_FileId",
                table: "Themes",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Themes_RepositoryId",
                table: "Themes",
                column: "RepositoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskStudentItems_Students_StudentId",
                table: "TaskStudentItems",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");
        }
    }
}
