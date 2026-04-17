using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Infraestructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddClientPropertyOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Client",
                table: "Orders",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Client",
                table: "Orders");
        }
    }
}
