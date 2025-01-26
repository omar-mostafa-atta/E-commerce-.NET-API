using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_commerce.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class imagetest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Product",
                newName: "Img5");

            migrationBuilder.AddColumn<string>(
                name: "Img1",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Img2",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Img3",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Img4",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Img1",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Img2",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Img3",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Img4",
                table: "Product");

            migrationBuilder.RenameColumn(
                name: "Img5",
                table: "Product",
                newName: "Image");
        }
    }
}
