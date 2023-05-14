using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ResearchersPlatform.Migrations
{
    /// <inheritdoc />
    public partial class AddProblemsMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DeleteData(
            //    table: "AspNetRoles",
            //    keyColumn: "Id",
            //    keyValue: "1163defe-9ade-435e-a130-d146f386e338");

            //migrationBuilder.DeleteData(
            //    table: "AspNetRoles",
            //    keyColumn: "Id",
            //    keyValue: "87b234e1-3157-4996-9598-aee7bef5eaee");

            //migrationBuilder.DeleteData(
            //    table: "AspNetRoles",
            //    keyColumn: "Id",
            //    keyValue: "c42697ce-f1a7-4a2d-9bb5-095b960fa508");

            //migrationBuilder.DeleteData(
            //    table: "AspNetRoles",
            //    keyColumn: "Id",
            //    keyValue: "e52fb3e5-137e-4dae-825f-ad31d982244a");

            migrationBuilder.CreateTable(
                name: "ProblemCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProblemCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Problems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProblemCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Problems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Problems_ProblemCategories_ProblemCategoryId",
                        column: x => x.ProblemCategoryId,
                        principalTable: "ProblemCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Problems_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Problems_ProblemCategoryId",
                table: "Problems",
                column: "ProblemCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Problems_StudentId",
                table: "Problems",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Problems");

            migrationBuilder.DropTable(
                name: "ProblemCategories");

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

            //migrationBuilder.InsertData(
            //    table: "AspNetRoles",
            //    columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
            //    values: new object[,]
            //    {
            //        { "1163defe-9ade-435e-a130-d146f386e338", null, "Student", "STUDENT" },
            //        { "87b234e1-3157-4996-9598-aee7bef5eaee", null, "Admin", "ADMIN" },
            //        { "c42697ce-f1a7-4a2d-9bb5-095b960fa508", null, "Student", "STUDENT" },
            //        { "e52fb3e5-137e-4dae-825f-ad31d982244a", null, "Admin", "ADMIN" }
            //    });
        }
    }
}
