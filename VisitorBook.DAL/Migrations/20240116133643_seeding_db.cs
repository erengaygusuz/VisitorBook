using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VisitorBook.DAL.Migrations
{
    /// <inheritdoc />
    public partial class seeding_db : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { 1, null, "SuperAdmin", "SUPERADMIN" },
                    { 2, null, "Admin", "ADMIN" },
                    { 3, null, "VisitorRecorder", "VISITORRECORDER" },
                    { 4, null, "Visitor", "VISITOR" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "BirthDate", "ConcurrencyStamp", "Email", "EmailConfirmed", "Gender", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Picture", "SecurityStamp", "Surname", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { 1, 0, new DateTime(1990, 11, 2, 11, 34, 55, 0, DateTimeKind.Unspecified), "5d9f6482-4ba7-4345-ad60-9f97e03222b8", "ozcaneren@gmail.com", true, 0, true, null, "Eren", "OZCANEREN@GMAIL.COM", "ERENOZCAN", "AQAAAAIAAYagAAAAED5jrSTAsrG4lBYx0bhM7fBiSUWqn07370xX5pAni6ieB2FxI8hfaCEuHM4W5v67PQ==", "(555) 555-5555", false, null, "5bd6de21-150e-48e9-b08a-4a8d612e7e4a", "Özcan", false, "erenozcan" },
                    { 2, 0, new DateTime(1996, 8, 22, 1, 32, 50, 0, DateTimeKind.Unspecified), "73b78b06-e407-4eaa-a30b-da8c7c7b81bb", "ceydakamis@gmail.com", true, 1, true, null, "Ceyda", "CEYDAKAMIS@GMAIL.COM", "CEYDAKAMIS", "AQAAAAIAAYagAAAAEKDg/e1fWu9B1U9H8d0KCP45Dj/wS7o0DhDDXK9gqm7DcHvj2V54vL6JDq3W0k/lAA==", "(555) 555-5555", false, null, "41d7c224-6487-4768-9ccf-eb18fbec9a02", "Kamış", false, "ceydakamis" },
                    { 3, 0, new DateTime(1985, 4, 20, 1, 33, 10, 0, DateTimeKind.Unspecified), "06a8de45-1669-4a4e-8e32-da9bf7f21394", "aliveli@gmail.com", true, 0, true, null, "Ali", "ALIVELI@GMAIL.COM", "ALIVELI", "AQAAAAIAAYagAAAAECIAttjDoddQ+qSy4NOaiKI/ADQlUmUtT6xIhlNJchbn799kpHW+HkP/Yp9Jsc6Yiw==", "(555) 555-5555", false, null, "dd97713d-9e08-4a3e-a501-21653f503ea8", "Veli", false, "aliveli" },
                    { 4, 0, new DateTime(1988, 11, 20, 11, 37, 20, 0, DateTimeKind.Unspecified), "2132784d-e560-4c5e-9914-c1141d76de3e", "sekserenay@gmail.com", true, 1, true, null, "Serenay", "SEKSERENAY@GMAIL.COM", "SEKSERENAY", "AQAAAAIAAYagAAAAEBdobkSjOOe20Pq6efjjiZqnPlNinyHRV5v7zBbfK3kgKq7ONySbJbAQuXa2DpJERg==", "(555) 555-5555", false, null, "d811d66c-91ab-4324-801a-63a003ea21b6", "Sek", false, "sekserenay" }
                });

            migrationBuilder.InsertData(
                table: "Regions",
                columns: new[] { "Id", "CreatedDate", "Name", "UpdatedDate" },
                values: new object[] { 1, new DateTime(2024, 1, 16, 16, 36, 43, 623, DateTimeKind.Local).AddTicks(2290), "Asia", null });

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 1, "Permission", "Permissions.UserManagement.View", 1 },
                    { 2, "Permission", "Permissions.UserManagement.Create", 1 },
                    { 3, "Permission", "Permissions.UserManagement.Edit", 1 },
                    { 4, "Permission", "Permissions.UserManagement.Delete", 1 },
                    { 5, "Permission", "Permissions.PlaceManagement.View", 1 },
                    { 6, "Permission", "Permissions.PlaceManagement.Create", 1 },
                    { 7, "Permission", "Permissions.PlaceManagement.Edit", 1 },
                    { 8, "Permission", "Permissions.PlaceManagement.Delete", 1 },
                    { 9, "Permission", "Permissions.VisitedCountyManagement.View", 1 },
                    { 10, "Permission", "Permissions.VisitedCountyManagement.Create", 1 },
                    { 11, "Permission", "Permissions.VisitedCountyManagement.Edit", 1 },
                    { 12, "Permission", "Permissions.VisitedCountyManagement.Delete", 1 },
                    { 13, "Permission", "Permissions.FakeDataManagement.View", 1 },
                    { 14, "Permission", "Permissions.FakeDataManagement.Create", 1 },
                    { 15, "Permission", "Permissions.FakeDataManagement.Edit", 1 },
                    { 16, "Permission", "Permissions.FakeDataManagement.Delete", 1 },
                    { 17, "Permission", "Permissions.ContactMessageManagement.View", 1 },
                    { 18, "Permission", "Permissions.ContactMessageManagement.Create", 1 },
                    { 19, "Permission", "Permissions.ContactMessageManagement.Edit", 1 },
                    { 20, "Permission", "Permissions.ContactMessageManagement.Delete", 1 },
                    { 21, "Permission", "Permissions.AuditTrailManagement.View", 1 },
                    { 22, "Permission", "Permissions.AuditTrailManagement.Create", 1 },
                    { 23, "Permission", "Permissions.AuditTrailManagement.Edit", 1 },
                    { 24, "Permission", "Permissions.AuditTrailManagement.Delete", 1 },
                    { 25, "Permission", "Permissions.RegisterApplicationManagement.View", 1 },
                    { 26, "Permission", "Permissions.RegisterApplicationManagement.Create", 1 },
                    { 27, "Permission", "Permissions.RegisterApplicationManagement.Edit", 1 },
                    { 28, "Permission", "Permissions.RegisterApplicationManagement.Delete", 1 },
                    { 29, "Permission", "Permissions.ExceptionLogManagement.View", 1 },
                    { 30, "Permission", "Permissions.ExceptionLogManagement.Create", 1 },
                    { 31, "Permission", "Permissions.ExceptionLogManagement.Edit", 1 },
                    { 32, "Permission", "Permissions.ExceptionLogManagement.Delete", 1 },
                    { 33, "Permission", "Permissions.PlaceManagement.View", 2 },
                    { 34, "Permission", "Permissions.PlaceManagement.Create", 2 },
                    { 35, "Permission", "Permissions.PlaceManagement.Edit", 2 },
                    { 36, "Permission", "Permissions.PlaceManagement.Delete", 2 },
                    { 37, "Permission", "Permissions.VisitedCountyManagement.View", 2 },
                    { 38, "Permission", "Permissions.VisitedCountyManagement.Create", 2 },
                    { 39, "Permission", "Permissions.VisitedCountyManagement.Edit", 2 },
                    { 40, "Permission", "Permissions.VisitedCountyManagement.Delete", 2 },
                    { 41, "Permission", "Permissions.ContactMessageManagement.View", 2 },
                    { 42, "Permission", "Permissions.ContactMessageManagement.Create", 2 },
                    { 43, "Permission", "Permissions.ContactMessageManagement.Edit", 2 },
                    { 44, "Permission", "Permissions.ContactMessageManagement.Delete", 2 },
                    { 45, "Permission", "Permissions.AuditTrailManagement.View", 2 },
                    { 46, "Permission", "Permissions.AuditTrailManagement.Create", 2 },
                    { 47, "Permission", "Permissions.AuditTrailManagement.Edit", 2 },
                    { 48, "Permission", "Permissions.AuditTrailManagement.Delete", 2 },
                    { 49, "Permission", "Permissions.RegisterApplicationManagement.View", 2 },
                    { 50, "Permission", "Permissions.RegisterApplicationManagement.Create", 2 },
                    { 51, "Permission", "Permissions.RegisterApplicationManagement.Edit", 2 },
                    { 52, "Permission", "Permissions.RegisterApplicationManagement.Delete", 2 },
                    { 53, "Permission", "Permissions.ExceptionLogManagement.View", 2 },
                    { 54, "Permission", "Permissions.ExceptionLogManagement.Create", 2 },
                    { 55, "Permission", "Permissions.ExceptionLogManagement.Edit", 2 },
                    { 56, "Permission", "Permissions.ExceptionLogManagement.Delete", 2 },
                    { 57, "Permission", "Permissions.VisitedCountyManagement.View", 3 },
                    { 58, "Permission", "Permissions.VisitedCountyManagement.Create", 3 },
                    { 59, "Permission", "Permissions.VisitedCountyManagement.Edit", 3 },
                    { 60, "Permission", "Permissions.VisitedCountyManagement.Delete", 3 },
                    { 61, "Permission", "Permissions.VisitedCountyManagement.View", 4 },
                    { 62, "Permission", "Permissions.VisitedCountyManagement.Create", 4 },
                    { 63, "Permission", "Permissions.VisitedCountyManagement.Edit", 4 },
                    { 64, "Permission", "Permissions.VisitedCountyManagement.Delete", 4 }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 3, 3 },
                    { 4, 4 }
                });

            migrationBuilder.InsertData(
                table: "SubRegions",
                columns: new[] { "Id", "CreatedDate", "Name", "RegionId", "UpdatedDate" },
                values: new object[] { 1, new DateTime(2024, 1, 16, 16, 36, 43, 623, DateTimeKind.Local).AddTicks(2957), "Western Asia", 1, null });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "Id", "Code", "CreatedDate", "Name", "SubRegionId", "UpdatedDate" },
                values: new object[] { 1, "TUR", new DateTime(2024, 1, 16, 16, 36, 43, 623, DateTimeKind.Local).AddTicks(3002), "Turkey", 1, null });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "Code", "CountryId", "CreatedDate", "Name", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, "06", 1, new DateTime(2024, 1, 16, 16, 36, 43, 623, DateTimeKind.Local).AddTicks(3039), "Ankara", null },
                    { 2, "35", 1, new DateTime(2024, 1, 16, 16, 36, 43, 623, DateTimeKind.Local).AddTicks(3042), "İzmir", null },
                    { 3, "34", 1, new DateTime(2024, 1, 16, 16, 36, 43, 623, DateTimeKind.Local).AddTicks(3043), "İstanbul", null }
                });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CityId", "CreatedDate", "Latitude", "Longitude", "Name", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 1, 16, 16, 36, 43, 623, DateTimeKind.Local).AddTicks(3168), 39.796688099999997, 32.223354700000002, "Çankaya", null },
                    { 2, 1, new DateTime(2024, 1, 16, 16, 36, 43, 623, DateTimeKind.Local).AddTicks(3172), 39.905137199999999, 32.692093999999997, "Mamak", null },
                    { 3, 1, new DateTime(2024, 1, 16, 16, 36, 43, 623, DateTimeKind.Local).AddTicks(3174), 40.086525000000002, 32.820312000000001, "Keçiören", null },
                    { 4, 2, new DateTime(2024, 1, 16, 16, 36, 43, 623, DateTimeKind.Local).AddTicks(3175), 38.422052700000002, 26.964354, "Konak", null },
                    { 5, 2, new DateTime(2024, 1, 16, 16, 36, 43, 623, DateTimeKind.Local).AddTicks(3177), 38.478544100000001, 27.075009600000001, "Bayraklı", null },
                    { 6, 2, new DateTime(2024, 1, 16, 16, 36, 43, 623, DateTimeKind.Local).AddTicks(3178), 38.5013997, 26.96218, "Karşıyaka", null },
                    { 7, 3, new DateTime(2024, 1, 16, 16, 36, 43, 623, DateTimeKind.Local).AddTicks(3180), 40.9812333, 28.980652599999999, "Kadıköy", null },
                    { 8, 3, new DateTime(2024, 1, 16, 16, 36, 43, 623, DateTimeKind.Local).AddTicks(3181), 40.984420299999996, 28.974454399999999, "Ataşehir", null },
                    { 9, 3, new DateTime(2024, 1, 16, 16, 36, 43, 623, DateTimeKind.Local).AddTicks(3183), 41.024865200000001, 28.637796699999999, "Avcılar", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, 3 });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 4, 4 });

            migrationBuilder.DeleteData(
                table: "Counties",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Counties",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Counties",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Counties",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Counties",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Counties",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Counties",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Counties",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Counties",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "SubRegions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
