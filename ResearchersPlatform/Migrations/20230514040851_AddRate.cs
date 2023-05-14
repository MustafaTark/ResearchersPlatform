using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ResearchersPlatform.Migrations
{
    /// <inheritdoc />
    public partial class AddRate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<int>(
                name: "Rate",
                table: "Researchers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            //migrationBuilder.InsertData(
            //    table: "AspNetRoles",
            //    columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
            //    values: new object[,]
            //    {
            //        { "968bf1ff-7993-4d4e-80b7-f771ceeb76b5", null, "Admin", "ADMIN" },
            //        { "a43bfaa5-99de-4ea8-9093-dfea075d6255", null, "Admin", "ADMIN" },
            //        { "f87151e2-cff8-4cd0-8af2-907055d36878", null, "Student", "STUDENT" },
            //        { "f98e5596-dda0-4713-8482-8cc7bf9ecd0a", null, "Student", "STUDENT" }
            //    });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DeleteData(
            //    table: "AspNetRoles",
            //    keyColumn: "Id",
            //    keyValue: "968bf1ff-7993-4d4e-80b7-f771ceeb76b5");

            //migrationBuilder.DeleteData(
            //    table: "AspNetRoles",
            //    keyColumn: "Id",
            //    keyValue: "a43bfaa5-99de-4ea8-9093-dfea075d6255");

            //migrationBuilder.DeleteData(
            //    table: "AspNetRoles",
            //    keyColumn: "Id",
            //    keyValue: "f87151e2-cff8-4cd0-8af2-907055d36878");

            //migrationBuilder.DeleteData(
            //    table: "AspNetRoles",
            //    keyColumn: "Id",
            //    keyValue: "f98e5596-dda0-4713-8482-8cc7bf9ecd0a");

            migrationBuilder.DropColumn(
                name: "Rate",
                table: "Researchers");

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
        }
    }
}
