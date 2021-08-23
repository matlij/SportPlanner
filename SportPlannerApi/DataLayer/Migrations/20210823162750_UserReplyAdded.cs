using Microsoft.EntityFrameworkCore.Migrations;

namespace SportPlannerIngestion.DataLayer.Migrations
{
    public partial class UserReplyAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAttending",
                table: "EventUsers");

            migrationBuilder.AddColumn<int>(
                name: "UserReply",
                table: "EventUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserReply",
                table: "EventUsers");

            migrationBuilder.AddColumn<bool>(
                name: "IsAttending",
                table: "EventUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
