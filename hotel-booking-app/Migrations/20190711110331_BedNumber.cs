using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelBookingApp.Migrations
{
    public partial class BedNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BedNumber",
                table: "RoomBeds",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ToDate",
                table: "Reservations",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "GuestNames",
                table: "Reservations",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FromDate",
                table: "Reservations",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Hotels",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BedNumber",
                table: "RoomBeds");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ToDate",
                table: "Reservations",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<string>(
                name: "GuestNames",
                table: "Reservations",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<DateTime>(
                name: "FromDate",
                table: "Reservations",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Hotels",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
