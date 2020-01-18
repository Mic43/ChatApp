using Microsoft.EntityFrameworkCore.Migrations;

namespace ServerTest.Migrations
{
    public partial class seeddatausers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Login", "Password" },
                values: new object[] { "admin", "admin" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Login", "Password" },
                values: new object[] { "admin2", "admin" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Login", "Password" },
                values: new object[] { "admin3", "admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Login",
                keyValue: "admin");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Login",
                keyValue: "admin2");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Login",
                keyValue: "admin3");
        }
    }
}
