using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ResearchersPlatform.Migrations
{
    /// <inheritdoc />
    public partial class AddPrivMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DeleteData(
            //    table: "AspNetRoles",
            //    keyColumn: "Id",
            //    keyValue: "0f9006be-894e-4e73-be5f-2e6af4594159");

            //migrationBuilder.DeleteData(
            //    table: "AspNetRoles",
            //    keyColumn: "Id",
            //    keyValue: "5ccb1707-84f6-4d56-9881-9a696b14143e");

            //migrationBuilder.DeleteData(
            //    table: "AspNetRoles",
            //    keyColumn: "Id",
            //    keyValue: "688caab9-ca49-4a7e-8b77-366952ba5c43");

            //migrationBuilder.DeleteData(
            //    table: "AspNetRoles",
            //    keyColumn: "Id",
            //    keyValue: "af6cb40e-f0b1-4e4d-b714-f69a99c1280d");

            migrationBuilder.CreateTable(
                name: "PrivateMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SenderId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ReciverId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrivateMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrivateMessages_Users_ReciverId",
                        column: x => x.ReciverId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PrivateMessages_Users_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            //migrationBuilder.InsertData(
            //    table: "AspNetRoles",
            //    columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
            //    values: new object[,]
            //    {
            //        { "6c5affe2-4b0a-4e38-a245-c5cd5b9b3977", null, "Student", "STUDENT" },
            //        { "78123a89-92c0-4d41-9dac-10d285a5db4e", null, "Admin", "ADMIN" },
            //        { "8e166fe3-cba1-4c18-a8df-b111b4d2928f", null, "Admin", "ADMIN" },
            //        { "a7e37d5d-ac49-45c9-8891-1a87515d5712", null, "Student", "STUDENT" }
            //    });

            migrationBuilder.CreateIndex(
                name: "IX_PrivateMessages_ReciverId",
                table: "PrivateMessages",
                column: "ReciverId");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateMessages_SenderId",
                table: "PrivateMessages",
                column: "SenderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrivateMessages");

            //migrationBuilder.DeleteData(
            //    table: "AspNetRoles",
            //    keyColumn: "Id",
            //    keyValue: "6c5affe2-4b0a-4e38-a245-c5cd5b9b3977");

            //migrationBuilder.DeleteData(
            //    table: "AspNetRoles",
            //    keyColumn: "Id",
            //    keyValue: "78123a89-92c0-4d41-9dac-10d285a5db4e");

            //migrationBuilder.DeleteData(
            //    table: "AspNetRoles",
            //    keyColumn: "Id",
            //    keyValue: "8e166fe3-cba1-4c18-a8df-b111b4d2928f");

            //migrationBuilder.DeleteData(
            //    table: "AspNetRoles",
            //    keyColumn: "Id",
            //    keyValue: "a7e37d5d-ac49-45c9-8891-1a87515d5712");

            //migrationBuilder.InsertData(
            //    table: "AspNetRoles",
            //    columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
            //    values: new object[,]
            //    {
            //        { "0f9006be-894e-4e73-be5f-2e6af4594159", null, "Admin", "ADMIN" },
            //        { "5ccb1707-84f6-4d56-9881-9a696b14143e", null, "Student", "STUDENT" },
            //        { "688caab9-ca49-4a7e-8b77-366952ba5c43", null, "Student", "STUDENT" },
            //        { "af6cb40e-f0b1-4e4d-b714-f69a99c1280d", null, "Admin", "ADMIN" }
            //    });
        }
    }
}
