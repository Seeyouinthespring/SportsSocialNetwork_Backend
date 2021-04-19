using Microsoft.EntityFrameworkCore.Migrations;

namespace SportsSocialNetwork.Migrations
{
    public partial class SomeFieldsRenamedAndRetyped : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentRequests_AspNetUsers_ApplicantId",
                table: "RentRequests");

            migrationBuilder.DropIndex(
                name: "IX_RentRequests_ApplicantId",
                table: "RentRequests");

            migrationBuilder.DropColumn(
                name: "ApplicantId",
                table: "RentRequests");

            migrationBuilder.DropColumn(
                name: "ApplicantName",
                table: "ConfirmedRents");

            migrationBuilder.AddColumn<string>(
                name: "RenterId",
                table: "RentRequests",
                nullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "Fee",
                table: "ConfirmedRents",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<string>(
                name: "RenterName",
                table: "ConfirmedRents",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RentRequests_RenterId",
                table: "RentRequests",
                column: "RenterId");

            migrationBuilder.AddForeignKey(
                name: "FK_RentRequests_AspNetUsers_RenterId",
                table: "RentRequests",
                column: "RenterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentRequests_AspNetUsers_RenterId",
                table: "RentRequests");

            migrationBuilder.DropIndex(
                name: "IX_RentRequests_RenterId",
                table: "RentRequests");

            migrationBuilder.DropColumn(
                name: "RenterId",
                table: "RentRequests");

            migrationBuilder.DropColumn(
                name: "RenterName",
                table: "ConfirmedRents");

            migrationBuilder.AddColumn<string>(
                name: "ApplicantId",
                table: "RentRequests",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Fee",
                table: "ConfirmedRents",
                type: "bit",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AddColumn<string>(
                name: "ApplicantName",
                table: "ConfirmedRents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RentRequests_ApplicantId",
                table: "RentRequests",
                column: "ApplicantId");

            migrationBuilder.AddForeignKey(
                name: "FK_RentRequests_AspNetUsers_ApplicantId",
                table: "RentRequests",
                column: "ApplicantId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
