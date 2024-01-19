using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreCoinMarket.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Coins",
                columns: table => new
                {
                    CoinId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Symbol = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coins", x => x.CoinId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CoinHistorys",
                columns: table => new
                {
                    LastUpdate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CoinId = table.Column<int>(type: "int", nullable: false),
                    CirculatingSupply = table.Column<int>(type: "int", nullable: false),
                    TotalSupply = table.Column<int>(type: "int", nullable: false),
                    MaxSupply = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "double", nullable: false),
                    Volume24h = table.Column<double>(type: "double", nullable: false),
                    VolumeChange24h = table.Column<double>(type: "double", nullable: false),
                    PercentChange1h = table.Column<double>(type: "double", nullable: false),
                    PercentChange24h = table.Column<double>(type: "double", nullable: false),
                    PercentChange7d = table.Column<double>(type: "double", nullable: false),
                    PercentChange30d = table.Column<double>(type: "double", nullable: false),
                    MarketCap = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoinHistorys", x => x.LastUpdate);
                    table.ForeignKey(
                        name: "FK_CoinHistorys_Coins_CoinId",
                        column: x => x.CoinId,
                        principalTable: "Coins",
                        principalColumn: "CoinId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CoinHistorys_CoinId",
                table: "CoinHistorys",
                column: "CoinId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoinHistorys");

            migrationBuilder.DropTable(
                name: "Coins");
        }
    }
}
