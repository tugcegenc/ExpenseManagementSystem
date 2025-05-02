using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Expense.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_UniqueConstraints_To_User : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                schema: "dbo",
                table: "Users",
                column: "UserName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_UserName",
                schema: "dbo",
                table: "Users");
        }
    }
}
