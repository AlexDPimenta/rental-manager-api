using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentalManager.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Drivers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Cnpj = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    BirthdayDate = table.Column<DateOnly>(type: "date", nullable: false),
                    LicenseNumber = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    LicenseCategory = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    LicenseImage = table.Column<string>(type: "varchar(1500)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drivers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MotorCycles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    Model = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Plate = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    LeaseId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MotorCycles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Leases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MotorCycleId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    DriverId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpectedEndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DriverId1 = table.Column<string>(type: "varchar(100)", nullable: true),
                    MotorCycleId1 = table.Column<string>(type: "varchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Leases_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Leases_Drivers_DriverId1",
                        column: x => x.DriverId1,
                        principalTable: "Drivers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Leases_MotorCycles_MotorCycleId",
                        column: x => x.MotorCycleId,
                        principalTable: "MotorCycles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Leases_MotorCycles_MotorCycleId1",
                        column: x => x.MotorCycleId1,
                        principalTable: "MotorCycles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Leases_DriverId",
                table: "Leases",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Leases_DriverId1",
                table: "Leases",
                column: "DriverId1");

            migrationBuilder.CreateIndex(
                name: "IX_Leases_MotorCycleId",
                table: "Leases",
                column: "MotorCycleId");

            migrationBuilder.CreateIndex(
                name: "IX_Leases_MotorCycleId1",
                table: "Leases",
                column: "MotorCycleId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Leases");

            migrationBuilder.DropTable(
                name: "Drivers");

            migrationBuilder.DropTable(
                name: "MotorCycles");
        }
    }
}
