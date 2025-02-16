using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUsersView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE VIEW UsersView AS
                SELECT Id, Username, Password, 'Admin' AS Role FROM Admins
                UNION ALL
                SELECT Id, Username, Password, 'Employee' AS Role FROM Employees
                UNION ALL
                SELECT Id, Username, Password, 'Manager' AS Role FROM Managers;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS UsersView;");
        }
    }
}
