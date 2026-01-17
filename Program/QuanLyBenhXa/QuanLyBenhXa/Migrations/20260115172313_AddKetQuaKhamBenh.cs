using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyBenhXa.Migrations
{
    /// <inheritdoc />
    public partial class AddKetQuaKhamBenh : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KetQuaKhamBenhs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoSoKhamBenhId = table.Column<int>(type: "int", nullable: false),
                    TenPhongKham = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KetQua = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayKham = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BacSiThucHien = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KetQuaKhamBenhs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KetQuaKhamBenhs_HoSoKhamBenhs_HoSoKhamBenhId",
                        column: x => x.HoSoKhamBenhId,
                        principalTable: "HoSoKhamBenhs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KetQuaKhamBenhs_HoSoKhamBenhId",
                table: "KetQuaKhamBenhs",
                column: "HoSoKhamBenhId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KetQuaKhamBenhs");
        }
    }
}
