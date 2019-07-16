using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelBookingApp.Migrations
{
    public partial class Hotel_Capacity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Capacity",
                table: "Hotels",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "Hotels");
        }
    }
}
