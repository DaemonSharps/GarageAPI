using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GarageAPI.Migrations
{
    public partial class DBv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Records_Customers_CustomerId",
                table: "Records");

            migrationBuilder.DropForeignKey(
                name: "FK_Records_RecordStates_RecordStateId",
                table: "Records");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "CustomerStates");

            migrationBuilder.RenameColumn(
                name: "RecordStateId",
                table: "Records",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Records",
                newName: "StateId");

            migrationBuilder.RenameIndex(
                name: "IX_Records_RecordStateId",
                table: "Records",
                newName: "IX_Records_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Records_CustomerId",
                table: "Records",
                newName: "IX_Records_StateId");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreationDate",
                table: "Records",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "FinishDate",
                table: "Records",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastUpdate",
                table: "Records",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.CreateTable(
                name: "UserStates",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    LastName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Patronymic = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
                    VisitCount = table.Column<long>(type: "bigint", nullable: false),
                    StateId = table.Column<long>(type: "bigint", nullable: false),
                    CreationDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    FinishDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_UserStates_StateId",
                        column: x => x.StateId,
                        principalTable: "UserStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "UserStates",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1L, "Clear" },
                    { 2L, "Banned" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreationDate", "Email", "FinishDate", "FirstName", "LastName", "LastUpdate", "Patronymic", "StateId", "VisitCount" },
                values: new object[] { 1L, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "ar-seny@mail.ru", null, "Арсений", "Васильев", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Тестовый", 1L, 0L });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_StateId",
                table: "Users",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStates_Name",
                table: "UserStates",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Records_RecordStates_StateId",
                table: "Records",
                column: "StateId",
                principalTable: "RecordStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Records_Users_UserId",
                table: "Records",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Records_RecordStates_StateId",
                table: "Records");

            migrationBuilder.DropForeignKey(
                name: "FK_Records_Users_UserId",
                table: "Records");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "UserStates");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "FinishDate",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "LastUpdate",
                table: "Records");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Records",
                newName: "RecordStateId");

            migrationBuilder.RenameColumn(
                name: "StateId",
                table: "Records",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Records_UserId",
                table: "Records",
                newName: "IX_Records_RecordStateId");

            migrationBuilder.RenameIndex(
                name: "IX_Records_StateId",
                table: "Records",
                newName: "IX_Records_CustomerId");

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
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CustomerStateId = table.Column<long>(type: "bigint", nullable: false),
                    Email = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
                    FirstName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    LastName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    SecondName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    VisitCount = table.Column<long>(type: "bigint", nullable: false)
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

            migrationBuilder.InsertData(
                table: "CustomerStates",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1L, "Clear" },
                    { 2L, "Banned" }
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

            migrationBuilder.AddForeignKey(
                name: "FK_Records_Customers_CustomerId",
                table: "Records",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Records_RecordStates_RecordStateId",
                table: "Records",
                column: "RecordStateId",
                principalTable: "RecordStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
