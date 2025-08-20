using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HostelHelpDesk.Migrations
{
    /// <inheritdoc />
    public partial class Initial2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Complaints_User_CaretakerId",
                table: "Complaints");

            migrationBuilder.DropForeignKey(
                name: "FK_Complaints_User_StudentId",
                table: "Complaints");

            migrationBuilder.DropForeignKey(
                name: "FK_Complaints_User_WorkerId",
                table: "Complaints");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Hostels_Caretaker_HostelId",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Hostels_HostelId",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Rooms_RoomId",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkerWorkerType_User_WorkersId",
                table: "WorkerWorkerType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Users");

            migrationBuilder.RenameIndex(
                name: "IX_User_RoomId",
                table: "Users",
                newName: "IX_Users_RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_User_RollNo",
                table: "Users",
                newName: "IX_Users_RollNo");

            migrationBuilder.RenameIndex(
                name: "IX_User_PhoneNumber",
                table: "Users",
                newName: "IX_Users_PhoneNumber");

            migrationBuilder.RenameIndex(
                name: "IX_User_HostelId",
                table: "Users",
                newName: "IX_Users_HostelId");

            migrationBuilder.RenameIndex(
                name: "IX_User_Caretaker_HostelId",
                table: "Users",
                newName: "IX_Users_Caretaker_HostelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_Users_CaretakerId",
                table: "Complaints",
                column: "CaretakerId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_Users_StudentId",
                table: "Complaints",
                column: "StudentId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_Users_WorkerId",
                table: "Complaints",
                column: "WorkerId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Hostels_Caretaker_HostelId",
                table: "Users",
                column: "Caretaker_HostelId",
                principalTable: "Hostels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Hostels_HostelId",
                table: "Users",
                column: "HostelId",
                principalTable: "Hostels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Rooms_RoomId",
                table: "Users",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkerWorkerType_Users_WorkersId",
                table: "WorkerWorkerType",
                column: "WorkersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Complaints_Users_CaretakerId",
                table: "Complaints");

            migrationBuilder.DropForeignKey(
                name: "FK_Complaints_Users_StudentId",
                table: "Complaints");

            migrationBuilder.DropForeignKey(
                name: "FK_Complaints_Users_WorkerId",
                table: "Complaints");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Hostels_Caretaker_HostelId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Hostels_HostelId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Rooms_RoomId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkerWorkerType_Users_WorkersId",
                table: "WorkerWorkerType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "User");

            migrationBuilder.RenameIndex(
                name: "IX_Users_RoomId",
                table: "User",
                newName: "IX_User_RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_RollNo",
                table: "User",
                newName: "IX_User_RollNo");

            migrationBuilder.RenameIndex(
                name: "IX_Users_PhoneNumber",
                table: "User",
                newName: "IX_User_PhoneNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Users_HostelId",
                table: "User",
                newName: "IX_User_HostelId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Caretaker_HostelId",
                table: "User",
                newName: "IX_User_Caretaker_HostelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_User_CaretakerId",
                table: "Complaints",
                column: "CaretakerId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_User_StudentId",
                table: "Complaints",
                column: "StudentId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_User_WorkerId",
                table: "Complaints",
                column: "WorkerId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Hostels_Caretaker_HostelId",
                table: "User",
                column: "Caretaker_HostelId",
                principalTable: "Hostels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Hostels_HostelId",
                table: "User",
                column: "HostelId",
                principalTable: "Hostels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Rooms_RoomId",
                table: "User",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkerWorkerType_User_WorkersId",
                table: "WorkerWorkerType",
                column: "WorkersId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
