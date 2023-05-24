using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoadsideAssistance.Api.data.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    VehicleVINNumber = table.Column<string>(type: "nvarchar(17)", maxLength: 17, nullable: false),
                    VehicleMakeModel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer_Id", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GeoLocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeoLocation_Id", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Assistants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CurrentGeoLocationId = table.Column<int>(type: "int", nullable: false),
                    IsReserved = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assistant_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assistant_GeoLocation_CurrentGeoLocationId",
                        column: x => x.CurrentGeoLocationId,
                        principalTable: "GeoLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomerAssistants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    AssistantId = table.Column<int>(type: "int", nullable: false),
                    GeoLocationId = table.Column<int>(type: "int", nullable: false),
                    ServiceStartDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ServiceCompleteDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerAssistant_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerAssistant_Assistant_AssistantId",
                        column: x => x.AssistantId,
                        principalTable: "Assistants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerAssistant_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerAssistant_GeoLocation_GeoLocationId",
                        column: x => x.GeoLocationId,
                        principalTable: "GeoLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assistants_CurrentGeoLocationId",
                table: "Assistants",
                column: "CurrentGeoLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAssistants_AssistantId",
                table: "CustomerAssistants",
                column: "AssistantId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAssistants_CustomerId",
                table: "CustomerAssistants",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAssistants_GeoLocationId",
                table: "CustomerAssistants",
                column: "GeoLocationId");

            migrationBuilder.CreateIndex(
                name: "UC_GeoLocation_Longitude_Latitude",
                table: "GeoLocations",
                columns: new[] { "Longitude", "Latitude" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerAssistants");

            migrationBuilder.DropTable(
                name: "Assistants");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "GeoLocations");
        }
    }
}
