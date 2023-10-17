using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PruebaHiberusHost.Migrations
{
    /// <inheritdoc />
    public partial class SinSum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Sums_SKU",
                table: "Transactions");

            migrationBuilder.DropTable(
                name: "Sums");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_SKU",
                table: "Transactions");

            migrationBuilder.AlterColumn<string>(
                name: "SKU",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SKU",
                table: "Transactions",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Sums",
                columns: table => new
                {
                    SKU = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TotalSum = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sums", x => x.SKU);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_SKU",
                table: "Transactions",
                column: "SKU");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Sums_SKU",
                table: "Transactions",
                column: "SKU",
                principalTable: "Sums",
                principalColumn: "SKU",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
