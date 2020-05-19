using Microsoft.EntityFrameworkCore.Migrations;

namespace TaskExecutionSystem.DAL.Migrations
{
    public partial class TaskStudentFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskStudentItems_Students_StudentId",
                table: "TaskStudentItems");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskStudentItems_Students_StudentId",
                table: "TaskStudentItems",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskStudentItems_Students_StudentId",
                table: "TaskStudentItems");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskStudentItems_Students_StudentId",
                table: "TaskStudentItems",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
