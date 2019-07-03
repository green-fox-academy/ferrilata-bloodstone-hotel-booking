using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelBookingApp.Migrations
{
    public partial class seedData1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "Address", "City", "Country", "Region" },
                values: new object[,]
                {
                    { 1, "a street 1", "Moon", "Moon", "Moon District?" },
                    { 2, "a street mars", "Mars", "Mars", "Mars District?" },
                    { 3, "a street p", "p", "Pluto", "Pluto District?" }
                });

            migrationBuilder.InsertData(
                table: "PropertyTypes",
                columns: new[] { "PropertyTypeId", "Type" },
                values: new object[,]
                {
                    { 1, "Apartment" },
                    { 2, "Hostel" },
                    { 3, "Hotel" },
                    { 4, "Guesthouse" }
                });

            migrationBuilder.InsertData(
                table: "Hotels",
                columns: new[] { "HotelId", "Description", "LocationId", "Name", "Price", "PropertyTypeId", "StarRating" },
                values: new object[] { 1, "description1", 1, "hotel1", 1, 1, 1 });

            migrationBuilder.InsertData(
                table: "Hotels",
                columns: new[] { "HotelId", "Description", "LocationId", "Name", "Price", "PropertyTypeId", "StarRating" },
                values: new object[] { 3, "description3", 3, "hotel3", 3, 1, 1 });

            migrationBuilder.InsertData(
                table: "Hotels",
                columns: new[] { "HotelId", "Description", "LocationId", "Name", "Price", "PropertyTypeId", "StarRating" },
                values: new object[] { 2, "description2", 2, "hotel2", 2, 2, 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "HotelId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "HotelId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "HotelId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PropertyTypes",
                keyColumn: "PropertyTypeId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PropertyTypes",
                keyColumn: "PropertyTypeId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "LocationId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "LocationId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "LocationId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PropertyTypes",
                keyColumn: "PropertyTypeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PropertyTypes",
                keyColumn: "PropertyTypeId",
                keyValue: 2);
        }
    }
}
