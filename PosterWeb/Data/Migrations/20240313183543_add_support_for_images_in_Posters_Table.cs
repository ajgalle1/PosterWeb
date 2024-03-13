using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PosterWeb.Data.Migrations
{
    /// <inheritdoc />
    public partial class add_support_for_images_in_Posters_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Posters",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<byte[]>(
                name: "BinaryVersionOfImage",
                table: "Posters",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImgPath",
                table: "Posters",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BinaryVersionOfImage",
                table: "Posters");

            migrationBuilder.DropColumn(
                name: "ImgPath",
                table: "Posters");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Posters",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);
        }
    }
}
