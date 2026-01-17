using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyBenhXa.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DonGia",
                table: "Thuocs",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "DaThanhToan",
                table: "HoSoKhamBenhs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "TongTien",
                table: "HoSoKhamBenhs",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DonGia",
                table: "Thuocs");

            migrationBuilder.DropColumn(
                name: "DaThanhToan",
                table: "HoSoKhamBenhs");

            migrationBuilder.DropColumn(
                name: "TongTien",
                table: "HoSoKhamBenhs");
        }
    }
}
