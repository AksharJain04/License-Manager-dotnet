using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LicenseGenerator.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CustomerEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerID);
                });

            migrationBuilder.CreateTable(
                name: "DeviceModel",
                columns: table => new
                {
                    ModelId = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    ModelName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceModel", x => x.ModelId);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    InvoiceID = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    CustomerID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SaleDate = table.Column<DateOnly>(type: "date", nullable: false),
                    DealerID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.InvoiceID);
                    table.ForeignKey(
                        name: "FK_Invoices_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    SerialNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DeviceID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModelID = table.Column<string>(type: "nvarchar(8)", nullable: false),
                    SaleDate = table.Column<DateOnly>(type: "date", nullable: false),
                    DeviceStatus = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.SerialNumber);
                    table.ForeignKey(
                        name: "FK_Devices_DeviceModel_ModelID",
                        column: x => x.ModelID,
                        principalTable: "DeviceModel",
                        principalColumn: "ModelId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceDeviceMappings",
                columns: table => new
                {
                    InvoiceID = table.Column<string>(type: "nvarchar(8)", nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DeviceSerialNumber = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceDeviceMappings", x => new { x.InvoiceID, x.SerialNumber });
                    table.ForeignKey(
                        name: "FK_InvoiceDeviceMappings_Devices_DeviceSerialNumber",
                        column: x => x.DeviceSerialNumber,
                        principalTable: "Devices",
                        principalColumn: "SerialNumber");
                    table.ForeignKey(
                        name: "FK_InvoiceDeviceMappings_Invoices_InvoiceID",
                        column: x => x.InvoiceID,
                        principalTable: "Invoices",
                        principalColumn: "InvoiceID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Licenses",
                columns: table => new
                {
                    LicenseID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LicenseKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InvoiceID = table.Column<string>(type: "nvarchar(8)", nullable: false),
                    DeviceSerialNumber = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActivationDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ExpirationDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ActivationStatus = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Licenses", x => x.LicenseID);
                    table.ForeignKey(
                        name: "FK_Licenses_Devices_DeviceSerialNumber",
                        column: x => x.DeviceSerialNumber,
                        principalTable: "Devices",
                        principalColumn: "SerialNumber");
                    table.ForeignKey(
                        name: "FK_Licenses_Invoices_InvoiceID",
                        column: x => x.InvoiceID,
                        principalTable: "Invoices",
                        principalColumn: "InvoiceID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Devices_ModelID",
                table: "Devices",
                column: "ModelID");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceDeviceMappings_DeviceSerialNumber",
                table: "InvoiceDeviceMappings",
                column: "DeviceSerialNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_CustomerID",
                table: "Invoices",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Licenses_DeviceSerialNumber",
                table: "Licenses",
                column: "DeviceSerialNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Licenses_InvoiceID",
                table: "Licenses",
                column: "InvoiceID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoiceDeviceMappings");

            migrationBuilder.DropTable(
                name: "Licenses");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "DeviceModel");

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
