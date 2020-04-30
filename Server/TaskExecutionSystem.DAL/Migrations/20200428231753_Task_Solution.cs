using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TaskExecutionSystem.DAL.Migrations
{
    public partial class Task_Solution : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NotificationCounter",
                table: "Teachers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NotificationCounter",
                table: "Students",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "RepositoryModels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContentText = table.Column<string>(nullable: true),
                    SubjectId = table.Column<int>(nullable: false),
                    TeacherId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepositoryModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepositoryModels_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RepositoryModels_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Themes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    ContentText = table.Column<string>(nullable: true),
                    RepositoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Themes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Themes_RepositoryModels_RepositoryId",
                        column: x => x.RepositoryId,
                        principalTable: "RepositoryModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskModels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    ContentText = table.Column<string>(maxLength: 2000, nullable: true),
                    BeginDate = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    FinishDate = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    IsOpen = table.Column<bool>(nullable: false, defaultValue: true),
                    TimePercentage = table.Column<int>(maxLength: 3, nullable: false, defaultValue: 100),
                    TeacherId = table.Column<int>(nullable: false),
                    TypeId = table.Column<int>(nullable: false),
                    SubjectId = table.Column<int>(nullable: false),
                    GroupId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskModels_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskModels_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskModels_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaskModels_TaskTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "TaskTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Paragraphs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContentText = table.Column<string>(nullable: true),
                    ThemeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paragraphs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Paragraphs_Themes_ThemeId",
                        column: x => x.ThemeId,
                        principalTable: "Themes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Solutions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContentText = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    InTime = table.Column<bool>(nullable: false),
                    TaskId = table.Column<int>(nullable: false),
                    StudentId = table.Column<int>(nullable: false),
                    SubjectId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Solutions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Solutions_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Solutions_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Solutions_TaskModels_TaskId",
                        column: x => x.TaskId,
                        principalTable: "TaskModels",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TaskFiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(nullable: true),
                    Path = table.Column<string>(nullable: true),
                    FileURI = table.Column<string>(nullable: true),
                    TaskModelId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskFiles_TaskModels_TaskModelId",
                        column: x => x.TaskModelId,
                        principalTable: "TaskModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskStudentItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskId = table.Column<int>(nullable: false),
                    StudentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskStudentItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskStudentItems_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaskStudentItems_TaskModels_TaskId",
                        column: x => x.TaskId,
                        principalTable: "TaskModels",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RepoFiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(nullable: true),
                    Path = table.Column<string>(nullable: true),
                    FileURI = table.Column<string>(nullable: true),
                    RepositoryItemId = table.Column<int>(nullable: true),
                    ThemeId = table.Column<int>(nullable: true),
                    ParagraphId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepoFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepoFiles_Paragraphs_ParagraphId",
                        column: x => x.ParagraphId,
                        principalTable: "Paragraphs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RepoFiles_RepositoryModels_RepositoryItemId",
                        column: x => x.RepositoryItemId,
                        principalTable: "RepositoryModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RepoFiles_Themes_ThemeId",
                        column: x => x.ThemeId,
                        principalTable: "Themes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SolutionFiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(nullable: true),
                    Path = table.Column<string>(nullable: true),
                    FileURI = table.Column<string>(nullable: true),
                    SolutionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolutionFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SolutionFiles_Solutions_SolutionId",
                        column: x => x.SolutionId,
                        principalTable: "Solutions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Paragraphs_ThemeId",
                table: "Paragraphs",
                column: "ThemeId");

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

            migrationBuilder.CreateIndex(
                name: "IX_RepositoryModels_SubjectId",
                table: "RepositoryModels",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_RepositoryModels_TeacherId",
                table: "RepositoryModels",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_SolutionFiles_SolutionId",
                table: "SolutionFiles",
                column: "SolutionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Solutions_StudentId",
                table: "Solutions",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Solutions_SubjectId",
                table: "Solutions",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Solutions_TaskId",
                table: "Solutions",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskFiles_TaskModelId",
                table: "TaskFiles",
                column: "TaskModelId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaskModels_GroupId",
                table: "TaskModels",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskModels_SubjectId",
                table: "TaskModels",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskModels_TeacherId",
                table: "TaskModels",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskModels_TypeId",
                table: "TaskModels",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskStudentItems_StudentId",
                table: "TaskStudentItems",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskStudentItems_TaskId",
                table: "TaskStudentItems",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Themes_RepositoryId",
                table: "Themes",
                column: "RepositoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RepoFiles");

            migrationBuilder.DropTable(
                name: "SolutionFiles");

            migrationBuilder.DropTable(
                name: "TaskFiles");

            migrationBuilder.DropTable(
                name: "TaskStudentItems");

            migrationBuilder.DropTable(
                name: "Paragraphs");

            migrationBuilder.DropTable(
                name: "Solutions");

            migrationBuilder.DropTable(
                name: "Themes");

            migrationBuilder.DropTable(
                name: "TaskModels");

            migrationBuilder.DropTable(
                name: "RepositoryModels");

            migrationBuilder.DropTable(
                name: "TaskTypes");

            migrationBuilder.DropColumn(
                name: "NotificationCounter",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "NotificationCounter",
                table: "Students");
        }
    }
}
