using Microsoft.EntityFrameworkCore.Migrations;

namespace SportsSocialNetwork.Migrations
{
    public partial class ConnectionTableRenamed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlaygroundSportsConnections");

            migrationBuilder.CreateTable(
                name: "PlaygroundSportConnections",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlaygroundId = table.Column<long>(nullable: false),
                    SportId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaygroundSportConnections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlaygroundSportConnections_Playgrounds_PlaygroundId",
                        column: x => x.PlaygroundId,
                        principalTable: "Playgrounds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlaygroundSportConnections_Sports_SportId",
                        column: x => x.SportId,
                        principalTable: "Sports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlaygroundSportConnections_PlaygroundId",
                table: "PlaygroundSportConnections",
                column: "PlaygroundId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaygroundSportConnections_SportId",
                table: "PlaygroundSportConnections",
                column: "SportId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlaygroundSportConnections");

            migrationBuilder.CreateTable(
                name: "PlaygroundSportsConnections",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlaygroundId = table.Column<long>(type: "bigint", nullable: false),
                    SportId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaygroundSportsConnections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlaygroundSportsConnections_Playgrounds_PlaygroundId",
                        column: x => x.PlaygroundId,
                        principalTable: "Playgrounds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlaygroundSportsConnections_Sports_SportId",
                        column: x => x.SportId,
                        principalTable: "Sports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlaygroundSportsConnections_PlaygroundId",
                table: "PlaygroundSportsConnections",
                column: "PlaygroundId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaygroundSportsConnections_SportId",
                table: "PlaygroundSportsConnections",
                column: "SportId");
        }
    }
}
