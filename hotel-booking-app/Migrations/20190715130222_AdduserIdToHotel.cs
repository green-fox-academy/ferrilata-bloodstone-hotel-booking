using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelBookingApp.Migrations
{
    public partial class AdduserIdToHotel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Hotels",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Hotels_ApplicationUserId",
                table: "Hotels",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hotels_AspNetUsers_ApplicationUserId",
                table: "Hotels",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hotels_AspNetUsers_ApplicationUserId",
                table: "Hotels");

            migrationBuilder.DropIndex(
                name: "IX_Hotels_ApplicationUserId",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Hotels");
        }
    }
}
