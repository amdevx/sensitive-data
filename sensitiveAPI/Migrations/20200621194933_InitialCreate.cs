using Microsoft.EntityFrameworkCore.Migrations;

namespace sensitiveAPI.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SensitiveDataEntities",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Data = table.Column<string>(nullable: true),
                    EncryptionKeyName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensitiveDataEntities", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SensitiveDataEntities_EncryptionKeyName",
                table: "SensitiveDataEntities",
                column: "EncryptionKeyName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SensitiveDataEntities");
        }
    }
}
