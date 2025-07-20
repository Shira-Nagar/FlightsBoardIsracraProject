using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightsDl.Migrations
{
    /// <inheritdoc />
    public partial class InitFullDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FlightNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Departure = table.Column<string>(type: "TEXT", nullable: false),
                    Destination = table.Column<string>(type: "TEXT", nullable: false),
                    DepartureTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ArrivalTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Gate = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserLogIns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogIns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserFlight",
                columns: table => new
                {
                    UserLogInId = table.Column<int>(type: "INTEGER", nullable: false),
                    FlightEntityId = table.Column<int>(type: "INTEGER", nullable: false),
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    BookingDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SeatNumber = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFlight", x => new { x.UserLogInId, x.FlightEntityId });
                    table.ForeignKey(
                        name: "FK_UserFlight_Flights_FlightEntityId",
                        column: x => x.FlightEntityId,
                        principalTable: "Flights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserFlight_UserLogIns_UserLogInId",
                        column: x => x.UserLogInId,
                        principalTable: "UserLogIns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserFlight_FlightEntityId",
                table: "UserFlight",
                column: "FlightEntityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserFlight");

            migrationBuilder.DropTable(
                name: "Flights");

            migrationBuilder.DropTable(
                name: "UserLogIns");
        }
    }
}
