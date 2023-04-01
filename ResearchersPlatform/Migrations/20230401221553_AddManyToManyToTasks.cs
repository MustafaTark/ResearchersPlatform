using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResearchersPlatform.Migrations
{
    /// <inheritdoc />
    public partial class AddManyToManyToTasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Researchers_Tasks_TaskId",
                table: "Researchers");

            migrationBuilder.DropIndex(
                name: "IX_Researchers_TaskId",
                table: "Researchers");

            migrationBuilder.DropColumn(
                name: "TaskId",
                table: "Researchers");

            migrationBuilder.CreateTable(
                name: "ResearcherTask",
                columns: table => new
                {
                    ParticipantsId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TasksId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResearcherTask", x => new { x.ParticipantsId, x.TasksId });
                    table.ForeignKey(
                        name: "FK_ResearcherTask_Researchers_ParticipantsId",
                        column: x => x.ParticipantsId,
                        principalTable: "Researchers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResearcherTask_Tasks_TasksId",
                        column: x => x.TasksId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResearcherTask_TasksId",
                table: "ResearcherTask",
                column: "TasksId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResearcherTask");

            migrationBuilder.AddColumn<Guid>(
                name: "TaskId",
                table: "Researchers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Researchers_TaskId",
                table: "Researchers",
                column: "TaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_Researchers_Tasks_TaskId",
                table: "Researchers",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id");
        }
    }
}
