using Microsoft.EntityFrameworkCore.Migrations;

namespace SportStore.Data.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "25d9ab0a-14d1-408d-b2e6-e40e74d0e51e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "ca8f8814-7b77-4fb8-a67e-160a431874a2");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e302202d-b696-4c86-9d7f-247bda13262f", "AQAAAAEAACcQAAAAEFpeLe49OQEIBgTiQug10LiZPYTrVb6qFEflIgLJu3PlM1JuCc4K3eRQj3z0j3cAiQ==", "6d7147cb-0f7a-4dbe-a22a-df32cc95c695" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "7be1c1b0-31c4-4bd6-8426-d42d2c214545");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "895d2c0d-9dab-4d43-81e1-05f6e273f2fc");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b1707e55-d41f-41b7-b612-e149281afb51", "AQAAAAEAACcQAAAAEDkso618haJ+njkQZcafLKE6hQ4KmW8TKBdMCdg6CwrKPEpnt+2TeYHxljY5BUrTrA==", "9e19eb2b-271c-422d-aa2c-0a776cb8e802" });
        }
    }
}
