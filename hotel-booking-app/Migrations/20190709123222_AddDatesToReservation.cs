using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelBookingApp.Migrations
{
    public partial class AddDatesToReservation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FromDate",
                table: "Reservations",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ToDate",
                table: "Reservations",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromDate",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "ToDate",
                table: "Reservations");
        }
    }
}
