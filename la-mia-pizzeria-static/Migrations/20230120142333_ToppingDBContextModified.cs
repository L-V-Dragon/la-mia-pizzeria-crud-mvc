using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace lamiapizzeriastatic.Migrations
{
    /// <inheritdoc />
    public partial class ToppingDBContextModified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PizzaTopping_Topping_ToppingsId",
                table: "PizzaTopping");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Topping",
                table: "Topping");

            migrationBuilder.RenameTable(
                name: "Topping",
                newName: "Toppings");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Toppings",
                table: "Toppings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PizzaTopping_Toppings_ToppingsId",
                table: "PizzaTopping",
                column: "ToppingsId",
                principalTable: "Toppings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PizzaTopping_Toppings_ToppingsId",
                table: "PizzaTopping");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Toppings",
                table: "Toppings");

            migrationBuilder.RenameTable(
                name: "Toppings",
                newName: "Topping");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Topping",
                table: "Topping",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PizzaTopping_Topping_ToppingsId",
                table: "PizzaTopping",
                column: "ToppingsId",
                principalTable: "Topping",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
