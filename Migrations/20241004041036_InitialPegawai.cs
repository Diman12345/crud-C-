using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace internal_api.Migrations
{
    /// <inheritdoc />
    public partial class InitialPegawai : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Barang",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nama = table.Column<string>(type: "text", nullable: true),
                    Serial_number = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Barang", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fasilitases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nama = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fasilitases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Shiftings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Waktu = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shiftings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pegawais",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Nik = table.Column<string>(type: "text", nullable: true),
                    Alamat = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    ShiftingId = table.Column<int>(type: "integer", nullable: false),
                    BarangId = table.Column<int>(type: "integer", nullable: false),
                    FasilitasId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pegawais", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pegawais_Barang_BarangId",
                        column: x => x.BarangId,
                        principalTable: "Barang",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pegawais_Fasilitases_FasilitasId",
                        column: x => x.FasilitasId,
                        principalTable: "Fasilitases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pegawais_Shiftings_ShiftingId",
                        column: x => x.ShiftingId,
                        principalTable: "Shiftings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pegawais_BarangId",
                table: "Pegawais",
                column: "BarangId");

            migrationBuilder.CreateIndex(
                name: "IX_Pegawais_FasilitasId",
                table: "Pegawais",
                column: "FasilitasId");

            migrationBuilder.CreateIndex(
                name: "IX_Pegawais_ShiftingId",
                table: "Pegawais",
                column: "ShiftingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pegawais");

            migrationBuilder.DropTable(
                name: "Barang");

            migrationBuilder.DropTable(
                name: "Fasilitases");

            migrationBuilder.DropTable(
                name: "Shiftings");
        }
    }
}
