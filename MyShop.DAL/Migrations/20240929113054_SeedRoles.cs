using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyShop.Web.Migrations
{
    /// <inheritdoc />
    public partial class SeedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var adminRole = "Admin";
            migrationBuilder.Sql($@"
            INSERT INTO Roles (Id, Name, NormalizedName, ConcurrencyStamp)
            VALUES ('{Guid.NewGuid()}', '{adminRole}', '{adminRole.ToUpper()}', '{Guid.NewGuid()}')");


            var EditorRole = "Editor";
            migrationBuilder.Sql($@"
            INSERT INTO Roles (Id, Name, NormalizedName, ConcurrencyStamp)
            VALUES ('{Guid.NewGuid()}', '{EditorRole}', '{EditorRole.ToUpper()}', '{Guid.NewGuid()}')");


            var CustomerRole = "User";
            migrationBuilder.Sql($@"
            INSERT INTO Roles (Id, Name, NormalizedName, ConcurrencyStamp)
            VALUES ('{Guid.NewGuid()}', '{CustomerRole}', '{CustomerRole.ToUpper()}', '{Guid.NewGuid()}')");



        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("delete from Roles");
        }
    }
}
