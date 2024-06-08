using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineShop_4M_DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTypeOfPriceOrderDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "PricePer",
                table: "OrderDetail",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PricePer",
                table: "OrderDetail",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "REAL");
        }
    }
}
