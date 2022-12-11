using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GarageAPI.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerStates",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerStates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecordStates",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecordStates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    SecondName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    LastName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
                    VisitCount = table.Column<long>(type: "bigint", nullable: false),
                    CustomerStateId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_CustomerStates_CustomerStateId",
                        column: x => x.CustomerStateId,
                        principalTable: "CustomerStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Records",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    Time = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PlaceNumber = table.Column<int>(type: "integer", nullable: false),
                    RecordStateId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Records", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Records_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Records_RecordStates_RecordStateId",
                        column: x => x.RecordStateId,
                        principalTable: "RecordStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CustomerStates",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1L, "Clear" },
                    { 2L, "Banned" }
                });

            migrationBuilder.InsertData(
                table: "RecordStates",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1L, "Approved" },
                    { 2L, "Waiting" },
                    { 3L, "Rejected" }
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "CustomerStateId", "Email", "FirstName", "LastName", "SecondName", "VisitCount" },
                values: new object[] { 1L, 1L, "ar-seny@mail.ru", "Арсений", "Тестовый", "Васильев", 0L });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CustomerStateId",
                table: "Customers",
                column: "CustomerStateId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Email",
                table: "Customers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerStates_Name",
                table: "CustomerStates",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Records_CustomerId",
                table: "Records",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Records_RecordStateId",
                table: "Records",
                column: "RecordStateId");

            migrationBuilder.CreateIndex(
                name: "IX_RecordStates_Name",
                table: "RecordStates",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Records");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "RecordStates");

            migrationBuilder.DropTable(
                name: "CustomerStates");
        }
    }
}
