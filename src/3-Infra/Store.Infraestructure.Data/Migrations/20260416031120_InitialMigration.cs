using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Store.Infraestructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    ord_int_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ord_str_number_order = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ord_date_created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ord_int_status = table.Column<int>(type: "integer", nullable: false),
                    ord_dec_total_price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.ord_int_id);
                });

            migrationBuilder.CreateTable(
                name: "Operations",
                columns: table => new
                {
                    ope_int_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ord_int_id = table.Column<long>(type: "bigint", nullable: false),
                    ope_int_status = table.Column<int>(type: "integer", nullable: false),
                    ope_date_created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ope_str_message = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operations", x => x.ope_int_id);
                    table.ForeignKey(
                        name: "FK_Operations_Orders_ord_int_id",
                        column: x => x.ord_int_id,
                        principalTable: "Orders",
                        principalColumn: "ord_int_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Operations_ord_int_id",
                table: "Operations",
                column: "ord_int_id");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ord_str_number_order",
                table: "Orders",
                column: "ord_str_number_order",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Operations");

            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
