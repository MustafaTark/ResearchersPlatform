using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ResearchersPlatform.Migrations
{
    /// <inheritdoc />
    public partial class AddMessagesMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "07c97ce5-b306-4efd-86af-323e7f27cd31");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "59de512a-c6d0-4210-8b55-3c5f27ae51ff");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bdd75fb0-e3cd-4cc8-9087-b7fdd08a4d58");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c2225411-3b30-4c1e-8370-2421b2eefc5c");

            migrationBuilder.CreateTable(
                name: "IdeaMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResearcherId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdeaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdeaMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IdeaMessages_Ideas_IdeaId",
                        column: x => x.IdeaId,
                        principalTable: "Ideas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IdeaMessages_Researchers_ResearcherId",
                        column: x => x.ResearcherId,
                        principalTable: "Researchers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResearcherId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaskIdeaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskMessages_Researchers_ResearcherId",
                        column: x => x.ResearcherId,
                        principalTable: "Researchers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskMessages_Tasks_TaskIdeaId",
                        column: x => x.TaskIdeaId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "8accc4ff-6dc7-4e6f-9062-19ae01014ce8", null, "Student", "STUDENT" },
                    { "b3df0a72-132a-4bae-be05-402a3f4c051d", null, "Admin", "ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_IdeaMessages_IdeaId",
                table: "IdeaMessages",
                column: "IdeaId");

            migrationBuilder.CreateIndex(
                name: "IX_IdeaMessages_ResearcherId",
                table: "IdeaMessages",
                column: "ResearcherId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskMessages_ResearcherId",
                table: "TaskMessages",
                column: "ResearcherId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskMessages_TaskIdeaId",
                table: "TaskMessages",
                column: "TaskIdeaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdeaMessages");

            migrationBuilder.DropTable(
                name: "TaskMessages");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8accc4ff-6dc7-4e6f-9062-19ae01014ce8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b3df0a72-132a-4bae-be05-402a3f4c051d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e9717870-1483-4c68-9590-8b7867e3b616");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ee10f166-7955-48ff-bab4-37a187a0e07d");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "07c97ce5-b306-4efd-86af-323e7f27cd31", null, "Student", "STUDENT" },
                    { "59de512a-c6d0-4210-8b55-3c5f27ae51ff", null, "Admin", "ADMIN" },
                    { "bdd75fb0-e3cd-4cc8-9087-b7fdd08a4d58", null, "Student", "STUDENT" },
                    { "c2225411-3b30-4c1e-8370-2421b2eefc5c", null, "Admin", "ADMIN" }
                });
        }
    }
}
