using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HostelHelpDesk.Migrations
{
    /// <inheritdoc />
    public partial class Initials1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkerTypes_User_WorkerId",
                table: "WorkerTypes");

            migrationBuilder.DropIndex(
                name: "IX_WorkerTypes_WorkerId",
                table: "WorkerTypes");

            migrationBuilder.DropColumn(
                name: "WorkerId",
                table: "WorkerTypes");

            migrationBuilder.CreateTable(
                name: "WorkerWorkerType",
                columns: table => new
                {
                    WorkerSpecializationId = table.Column<int>(type: "int", nullable: false),
                    WorkersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerWorkerType", x => new { x.WorkerSpecializationId, x.WorkersId });
                    table.ForeignKey(
                        name: "FK_WorkerWorkerType_User_WorkersId",
                        column: x => x.WorkersId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkerWorkerType_WorkerTypes_WorkerSpecializationId",
                        column: x => x.WorkerSpecializationId,
                        principalTable: "WorkerTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerWorkerType_WorkersId",
                table: "WorkerWorkerType",
                column: "WorkersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkerWorkerType");

            migrationBuilder.AddColumn<int>(
                name: "WorkerId",
                table: "WorkerTypes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkerTypes_WorkerId",
                table: "WorkerTypes",
                column: "WorkerId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkerTypes_User_WorkerId",
                table: "WorkerTypes",
                column: "WorkerId",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}
