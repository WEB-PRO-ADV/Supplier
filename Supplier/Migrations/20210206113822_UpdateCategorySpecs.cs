using Microsoft.EntityFrameworkCore.Migrations;

namespace Supplier.Migrations
{
    public partial class UpdateCategorySpecs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategorySpecs_Categories_CategoryId",
                table: "CategorySpecs");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "CategorySpecs",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CategorySpecs_Categories_CategoryId",
                table: "CategorySpecs",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategorySpecs_Categories_CategoryId",
                table: "CategorySpecs");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "CategorySpecs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_CategorySpecs_Categories_CategoryId",
                table: "CategorySpecs",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
