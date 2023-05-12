using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainBooking.Models.Migrations
{
    public partial class initialmigration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sections_Stations_DepartureStationId",
                table: "Sections");

            migrationBuilder.DropForeignKey(
                name: "FK_Sections_Stations_DestinationStationId",
                table: "Sections");

            migrationBuilder.DropForeignKey(
                name: "FK_Sections_Tickets_TicketId",
                table: "Sections");

            migrationBuilder.DropIndex(
                name: "IX_Sections_TicketId",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "TicketId",
                table: "Sections");

            migrationBuilder.CreateTable(
                name: "SectionTickets",
                columns: table => new
                {
                    SectionId = table.Column<int>(type: "int", nullable: false),
                    TicketId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SectionTickets", x => new { x.SectionId, x.TicketId });
                    table.ForeignKey(
                        name: "FK_SectionTickets_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SectionTickets_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SectionTickets_TicketId",
                table: "SectionTickets",
                column: "TicketId");

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

            migrationBuilder.DropTable(
                name: "SectionTickets");

            migrationBuilder.AddColumn<int>(
                name: "TicketId",
                table: "Sections",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sections_TicketId",
                table: "Sections",
                column: "TicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_Stations_DepartureStationId",
                table: "Sections",
                column: "DepartureStationId",
                principalTable: "Stations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_Stations_DestinationStationId",
                table: "Sections",
                column: "DestinationStationId",
                principalTable: "Stations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_Tickets_TicketId",
                table: "Sections",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id");
        }
    }
}
