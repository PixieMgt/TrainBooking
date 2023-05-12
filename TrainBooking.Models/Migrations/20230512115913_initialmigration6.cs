using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainBooking.Models.Migrations
{
    public partial class initialmigration6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sections_Stations_DepartureStationId",
                table: "Sections");

            migrationBuilder.DropForeignKey(
                name: "FK_Sections_Stations_DestinationStationId",
                table: "Sections");

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_Stations_DepartureStationId",
                table: "Sections",
                column: "DepartureStationId",
                principalTable: "Stations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_Stations_DestinationStationId",
                table: "Sections",
                column: "DestinationStationId",
                principalTable: "Stations",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sections_Stations_DepartureStationId",
                table: "Sections");

            migrationBuilder.DropForeignKey(
                name: "FK_Sections_Stations_DestinationStationId",
                table: "Sections");

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_Stations_DepartureStationId",
                table: "Sections",
                column: "DepartureStationId",
                principalTable: "Stations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_Stations_DestinationStationId",
                table: "Sections",
                column: "DestinationStationId",
                principalTable: "Stations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
