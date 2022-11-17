using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyWebApp2.Data.Migrations
{
    public partial class Vocstack : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vocstack",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StackName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumOfVoc = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vocstack", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vocstack");
        }
    }
}
