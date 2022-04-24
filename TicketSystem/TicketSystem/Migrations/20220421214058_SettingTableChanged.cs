using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketSystem.Migrations
{
    public partial class SettingTableChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Settings",
                newName: "Address2");

            migrationBuilder.AddColumn<string>(
                name: "Address1",
                table: "Settings",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address1",
                table: "Settings");

            migrationBuilder.RenameColumn(
                name: "Address2",
                table: "Settings",
                newName: "Address");
        }
    }
}
