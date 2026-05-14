using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ArabRiver.Repository.Migrations
{
    /// <inheritdoc />
    public partial class partner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Catalogs");

            migrationBuilder.AddColumn<Guid>(
                name: "PartnerId",
                table: "Catalogs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Partners",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partners", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Partners",
                columns: new[] { "Id", "CreatedAt", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("0c1c7c20-71de-4a0d-bb8e-2c1f7903f6a2"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Aygun", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("4ed77bdb-2c1d-4d5a-97f2-6a8f1d35c6b4"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "DrFrigz", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("8e1c1f52-6b54-4bf0-9f84-3b6c71b624af"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Vicoris Health", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("9f5bfb8b-8f5a-48a3-97ab-19c2b4f4a8c5"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Frimed", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("a12d67de-1bc1-48ce-b11d-8c0c5a9f6b77"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Steristar", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("b65d0dcb-0d0d-4f63-9a7e-7d4b1ad6d8a1"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Insto", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("ba3d6f3b-7a21-4f20-9f79-0a0dd1c6e5b8"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Nouvag", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("e13a1c9a-62a5-4e1e-8d2f-4f5b3b2b2e11"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Swantia", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("f6a4e9f2-3bfa-4b08-8a5c-46f88f1b6a10"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ABI surgical", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Catalogs_PartnerId",
                table: "Catalogs",
                column: "PartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Partners_Name",
                table: "Partners",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Catalogs_Partners_PartnerId",
                table: "Catalogs",
                column: "PartnerId",
                principalTable: "Partners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Catalogs_Partners_PartnerId",
                table: "Catalogs");

            migrationBuilder.DropTable(
                name: "Partners");

            migrationBuilder.DropIndex(
                name: "IX_Catalogs_PartnerId",
                table: "Catalogs");

            migrationBuilder.DropColumn(
                name: "PartnerId",
                table: "Catalogs");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Catalogs",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
