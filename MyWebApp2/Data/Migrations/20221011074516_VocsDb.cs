using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyWebApp2.Data.Migrations
{
    public partial class VocsDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Voc",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Vocabulary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartOfSpeeach = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Meaning = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sentence = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StackId = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voc", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Voc");
        }
    }
}
