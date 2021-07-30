using Microsoft.EntityFrameworkCore.Migrations;

namespace SportPlannerIngestion.DataLayer.Migrations
{
    public partial class IsOwnerColumnAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOwner",
                table: "EventUsers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOwner",
                table: "EventUsers");
        }
    }
}
