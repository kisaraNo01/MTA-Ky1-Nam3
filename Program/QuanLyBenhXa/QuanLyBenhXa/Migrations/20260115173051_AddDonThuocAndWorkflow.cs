using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyBenhXa.Migrations
{
    /// <inheritdoc />
    public partial class AddDonThuocAndWorkflow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhongYeuCau",
                table: "HoSoKhamBenhs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "DonThuocs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoSoKhamBenhId = table.Column<int>(type: "int", nullable: false),
                    ThuocId = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    CachDung = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonThuocs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DonThuocs_HoSoKhamBenhs_HoSoKhamBenhId",
                        column: x => x.HoSoKhamBenhId,
                        principalTable: "HoSoKhamBenhs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DonThuocs_Thuocs_ThuocId",
                        column: x => x.ThuocId,
                        principalTable: "Thuocs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DonThuocs_HoSoKhamBenhId",
                table: "DonThuocs",
                column: "HoSoKhamBenhId");

            migrationBuilder.CreateIndex(
                name: "IX_DonThuocs_ThuocId",
                table: "DonThuocs",
                column: "ThuocId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DonThuocs");

            migrationBuilder.DropColumn(
                name: "PhongYeuCau",
                table: "HoSoKhamBenhs");
        }
    }
}
