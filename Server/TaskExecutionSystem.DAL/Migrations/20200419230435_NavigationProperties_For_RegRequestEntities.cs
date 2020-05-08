using Microsoft.EntityFrameworkCore.Migrations;

namespace TaskExecutionSystem.DAL.Migrations
{
    public partial class NavigationProperties_For_RegRequestEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TeacherRegisterRequests_DepartmentId",
                table: "TeacherRegisterRequests",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentRegisterRequests_GroupId",
                table: "StudentRegisterRequests",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentRegisterRequests_Groups_GroupId",
                table: "StudentRegisterRequests",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherRegisterRequests_Departments_DepartmentId",
                table: "TeacherRegisterRequests",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentRegisterRequests_Groups_GroupId",
                table: "StudentRegisterRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherRegisterRequests_Departments_DepartmentId",
                table: "TeacherRegisterRequests");

            migrationBuilder.DropIndex(
                name: "IX_TeacherRegisterRequests_DepartmentId",
                table: "TeacherRegisterRequests");

            migrationBuilder.DropIndex(
                name: "IX_StudentRegisterRequests_GroupId",
                table: "StudentRegisterRequests");
        }
    }
}
