using Microsoft.EntityFrameworkCore.Migrations;

namespace SportsSocialNetwork.Migrations
{
    public partial class AddedSportIdForAppointment1905 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SportId",
                table: "Appointments",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.Sql("Update Appointments set SportId = 1");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_SportId",
                table: "Appointments",
                column: "SportId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Sports_SportId",
                table: "Appointments",
                column: "SportId",
                principalTable: "Sports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Sports_SportId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_SportId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "SportId",
                table: "Appointments");
        }
    }
}
