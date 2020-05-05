using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TaskExecutionSystem.DAL.Migrations
{
    public partial class Task_fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solutions_Subjects_SubjectId",
                table: "Solutions");

            migrationBuilder.DropIndex(
                name: "IX_Solutions_SubjectId",
                table: "Solutions");

            migrationBuilder.DropColumn(
                name: "TimePercentage",
                table: "TaskModels");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "Solutions");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "TaskModels",
                type: "DATETIME",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "TaskModels",
                type: "DATETIME",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TimePercentage",
                table: "TaskModels",
                type: "int",
                maxLength: 3,
                nullable: false,
                defaultValue: 100);

            migrationBuilder.AddColumn<int>(
                name: "SubjectId",
                table: "Solutions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Solutions_SubjectId",
                table: "Solutions",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Solutions_Subjects_SubjectId",
                table: "Solutions",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
