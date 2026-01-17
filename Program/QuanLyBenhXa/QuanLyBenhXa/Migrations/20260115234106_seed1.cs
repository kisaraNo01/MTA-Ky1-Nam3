using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyBenhXa.Migrations
{
    /// <inheritdoc />
    public partial class seed1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SoLuongTonKho",
                table: "Thuocs",
                newName: "SoLuongTon");

            migrationBuilder.AddColumn<string>(
                name: "CachDung",
                table: "Thuocs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HamLuong",
                table: "Thuocs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CachDung",
                table: "Thuocs");

            migrationBuilder.DropColumn(
                name: "HamLuong",
                table: "Thuocs");

            migrationBuilder.RenameColumn(
                name: "SoLuongTon",
                table: "Thuocs",
                newName: "SoLuongTonKho");
        }
    }
}
