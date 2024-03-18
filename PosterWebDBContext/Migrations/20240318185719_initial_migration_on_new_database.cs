using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PosterWebDBLibrary.Migrations
{
    /// <inheritdoc />
    public partial class initial_migration_on_new_database : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Posters",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Artist = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImgPath = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    BinaryVersionOfImage = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posters", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Posters_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Posters_CategoryId",
                table: "Posters",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Posters");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
