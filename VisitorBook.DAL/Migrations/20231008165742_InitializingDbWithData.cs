using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VisitorBook.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitializingDbWithData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Visitors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visitors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "States",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => x.Id);
                    table.ForeignKey(
                        name: "FK_States_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VisitorAddress",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StateId = table.Column<int>(type: "int", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    VisitorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitorAddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VisitorAddress_Visitors_VisitorId",
                        column: x => x.VisitorId,
                        principalTable: "Visitors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VisitedStates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VisitorId = table.Column<int>(type: "int", nullable: false),
                    StateId = table.Column<int>(type: "int", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitedStates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VisitedStates_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VisitedStates_Visitors_VisitorId",
                        column: x => x.VisitorId,
                        principalTable: "Visitors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "Code", "Name" },
                values: new object[,]
                {
                    { 1, "06", "Ankara" },
                    { 2, "35", "İzmir" },
                    { 3, "34", "İstanbul" }
                });

            migrationBuilder.InsertData(
                table: "Visitors",
                columns: new[] { "Id", "BirthDate", "Gender", "Name", "Surname" },
                values: new object[,]
                {
                    { 1, new DateTime(1992, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "Eren", "Gaygusuz" },
                    { 2, new DateTime(1995, 11, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "Eren", "Özcan" },
                    { 3, new DateTime(1996, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Ceyda", "Meyda" },
                    { 4, new DateTime(1990, 5, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Tuğçe", "Güzel" }
                });

            migrationBuilder.InsertData(
                table: "States",
                columns: new[] { "Id", "CityId", "Latitude", "Longitude", "Name" },
                values: new object[,]
                {
                    { 1, 1, 39.796688099999997, 32.223354700000002, "Çankaya" },
                    { 2, 1, 39.905137199999999, 32.692093999999997, "Mamak" },
                    { 3, 1, 40.086525000000002, 32.820312000000001, "Keçiören" },
                    { 4, 2, 38.422052700000002, 26.964354, "Konak" },
                    { 5, 2, 38.478544100000001, 27.075009600000001, "Bayraklı" },
                    { 6, 2, 38.5013997, 26.96218, "Karşıyaka" },
                    { 7, 3, 40.9812333, 28.980652599999999, "Kadıköy" },
                    { 8, 3, 40.984420299999996, 28.974454399999999, "Ataşehir" },
                    { 9, 3, 41.024865200000001, 28.637796699999999, "Avcılar" }
                });

            migrationBuilder.InsertData(
                table: "VisitorAddress",
                columns: new[] { "Id", "CityId", "StateId", "VisitorId" },
                values: new object[,]
                {
                    { 1, 1, 1, 1 },
                    { 2, 3, 7, 3 }
                });

            migrationBuilder.InsertData(
                table: "VisitedStates",
                columns: new[] { "Id", "CityId", "Date", "StateId", "VisitorId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2015, 11, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 1 },
                    { 2, 2, new DateTime(2015, 10, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 1 },
                    { 3, 3, new DateTime(2017, 1, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, 1 },
                    { 4, 3, new DateTime(2022, 8, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, 1 },
                    { 5, 2, new DateTime(2012, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 2 },
                    { 6, 1, new DateTime(2023, 8, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2 },
                    { 7, 1, new DateTime(2010, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 3 },
                    { 8, 3, new DateTime(2002, 10, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, 3 },
                    { 9, 3, new DateTime(2011, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, 3 },
                    { 10, 3, new DateTime(2020, 5, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, 3 },
                    { 11, 3, new DateTime(2008, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, 4 },
                    { 12, 3, new DateTime(2000, 8, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_States_CityId",
                table: "States",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitedStates_StateId",
                table: "VisitedStates",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitedStates_VisitorId",
                table: "VisitedStates",
                column: "VisitorId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitorAddress_VisitorId",
                table: "VisitorAddress",
                column: "VisitorId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VisitedStates");

            migrationBuilder.DropTable(
                name: "VisitorAddress");

            migrationBuilder.DropTable(
                name: "States");

            migrationBuilder.DropTable(
                name: "Visitors");

            migrationBuilder.DropTable(
                name: "Cities");
        }
    }
}
