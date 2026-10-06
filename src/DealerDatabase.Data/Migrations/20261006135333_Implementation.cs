using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DealerDatabase.Data.Migrations
{
    /// <inheritdoc />
    public partial class Implementation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Dealers");

            migrationBuilder.AddColumn<string>(
                name: "CompanyHouseNumber",
                table: "Dealers",
                type: "TEXT",
                maxLength: 8,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Dealers",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Dealers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FcaReferenceNumber",
                table: "Dealers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FcaStatus",
                table: "Dealers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "FcaStatusEffectiveDate",
                table: "Dealers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "IcoExpirationDate",
                table: "Dealers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IcoRegistrationNumber",
                table: "Dealers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "IncorporationDate",
                table: "Dealers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LegalName",
                table: "Dealers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedName",
                table: "Dealers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Dealers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RegisteredAddressId",
                table: "Dealers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TradingAddressId",
                table: "Dealers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Dealers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VatNumber",
                table: "Dealers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VatStatus",
                table: "Dealers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "Dealers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Country = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    County = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    PostalCode = table.Column<string>(type: "TEXT", maxLength: 10, nullable: true),
                    Town = table.Column<string>(type: "TEXT", maxLength: 60, nullable: true),
                    Line1 = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Line2 = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Line3 = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DealerOfficers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DealerId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    OfficerRole = table.Column<int>(type: "INTEGER", nullable: false),
                    AppointedOn = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    ResignedOn = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    Nationality = table.Column<string>(type: "TEXT", nullable: true),
                    Occupation = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealerOfficers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DealerOfficers_Dealers_DealerId",
                        column: x => x.DealerId,
                        principalTable: "Dealers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DealerTradingNames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DealerId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealerTradingNames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DealerTradingNames_Dealers_DealerId",
                        column: x => x.DealerId,
                        principalTable: "Dealers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImportDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RanAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    RecordsCreated = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportDetails", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dealers_RegisteredAddressId",
                table: "Dealers",
                column: "RegisteredAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Dealers_TradingAddressId",
                table: "Dealers",
                column: "TradingAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerOfficers_DealerId",
                table: "DealerOfficers",
                column: "DealerId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerTradingNames_DealerId",
                table: "DealerTradingNames",
                column: "DealerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dealers_Addresses_RegisteredAddressId",
                table: "Dealers",
                column: "RegisteredAddressId",
                principalTable: "Addresses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Dealers_Addresses_TradingAddressId",
                table: "Dealers",
                column: "TradingAddressId",
                principalTable: "Addresses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dealers_Addresses_RegisteredAddressId",
                table: "Dealers");

            migrationBuilder.DropForeignKey(
                name: "FK_Dealers_Addresses_TradingAddressId",
                table: "Dealers");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "DealerOfficers");

            migrationBuilder.DropTable(
                name: "DealerTradingNames");

            migrationBuilder.DropTable(
                name: "ImportDetails");

            migrationBuilder.DropIndex(
                name: "IX_Dealers_RegisteredAddressId",
                table: "Dealers");

            migrationBuilder.DropIndex(
                name: "IX_Dealers_TradingAddressId",
                table: "Dealers");

            migrationBuilder.DropColumn(
                name: "CompanyHouseNumber",
                table: "Dealers");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Dealers");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Dealers");

            migrationBuilder.DropColumn(
                name: "FcaReferenceNumber",
                table: "Dealers");

            migrationBuilder.DropColumn(
                name: "FcaStatus",
                table: "Dealers");

            migrationBuilder.DropColumn(
                name: "FcaStatusEffectiveDate",
                table: "Dealers");

            migrationBuilder.DropColumn(
                name: "IcoExpirationDate",
                table: "Dealers");

            migrationBuilder.DropColumn(
                name: "IcoRegistrationNumber",
                table: "Dealers");

            migrationBuilder.DropColumn(
                name: "IncorporationDate",
                table: "Dealers");

            migrationBuilder.DropColumn(
                name: "LegalName",
                table: "Dealers");

            migrationBuilder.DropColumn(
                name: "NormalizedName",
                table: "Dealers");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Dealers");

            migrationBuilder.DropColumn(
                name: "RegisteredAddressId",
                table: "Dealers");

            migrationBuilder.DropColumn(
                name: "TradingAddressId",
                table: "Dealers");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Dealers");

            migrationBuilder.DropColumn(
                name: "VatNumber",
                table: "Dealers");

            migrationBuilder.DropColumn(
                name: "VatStatus",
                table: "Dealers");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "Dealers");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Dealers",
                type: "TEXT",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }
    }
}
