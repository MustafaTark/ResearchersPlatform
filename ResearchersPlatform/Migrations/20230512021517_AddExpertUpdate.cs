using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ResearchersPlatform.Migrations
{
    /// <inheritdoc />
    public partial class AddExpertUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "ExpertRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdeaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParticipantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpertRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpertRequests_Ideas_IdeaId",
                        column: x => x.IdeaId,
                        principalTable: "Ideas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExpertRequests_Researchers_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "Researchers",
                        principalColumn: "Id");
                });

            //migrationBuilder.InsertData(
            //    table: "AspNetRoles",
            //    columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
            //    values: new object[,]
            //    {
            //        { "2cce98d1-54a4-49d1-8fd9-ad02b3c37234", null, "Admin", "ADMIN" },
            //        { "35ebfbc9-89b7-443b-b516-0de053523d3f", null, "Student", "STUDENT" },
            //        { "914530bb-ce2f-4c4c-8dbf-ee3d806d2d40", null, "Student", "STUDENT" },
            //        { "b2297157-ee6f-4527-be03-227f811b0cae", null, "Admin", "ADMIN" }
            //    });

            migrationBuilder.CreateIndex(
                name: "IX_ExpertRequests_IdeaId",
                table: "ExpertRequests",
                column: "IdeaId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpertRequests_ParticipantId",
                table: "ExpertRequests",
                column: "ParticipantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExpertRequests");

            //migrationBuilder.DeleteData(
            //    table: "AspNetRoles",
            //    keyColumn: "Id",
            //    keyValue: "2cce98d1-54a4-49d1-8fd9-ad02b3c37234");

            //migrationBuilder.DeleteData(
            //    table: "AspNetRoles",
            //    keyColumn: "Id",
            //    keyValue: "35ebfbc9-89b7-443b-b516-0de053523d3f");

            //migrationBuilder.DeleteData(
            //    table: "AspNetRoles",
            //    keyColumn: "Id",
            //    keyValue: "914530bb-ce2f-4c4c-8dbf-ee3d806d2d40");

            //migrationBuilder.DeleteData(
            //    table: "AspNetRoles",
            //    keyColumn: "Id",
            //    keyValue: "b2297157-ee6f-4527-be03-227f811b0cae");

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
        }
    }
}
