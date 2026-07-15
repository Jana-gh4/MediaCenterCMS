using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediaCenterCMS.API.Migrations
{
    /// <inheritdoc />
    public partial class MakeApprovalGeneric : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApprovalRequests_News_NewsId",
                table: "ApprovalRequests");

            migrationBuilder.DropIndex(
                name: "IX_ApprovalRequests_NewsId",
                table: "ApprovalRequests");

            migrationBuilder.DropColumn(
                name: "NewsId",
                table: "ApprovalRequests");

            migrationBuilder.AddColumn<int>(
                name: "EntityType",
                table: "ApprovalRequests",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EntityId",
                table: "ApprovalRequests",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EntityType",
                table: "ApprovalRequests");

            migrationBuilder.DropColumn(
                name: "EntityId",
                table: "ApprovalRequests");

            migrationBuilder.AddColumn<int>(
                name: "NewsId",
                table: "AdotpprovalRequests",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalRequests_NewsId",
                table: "ApprovalRequests",
                column: "NewsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovalRequests_News_NewsId",
                table: "ApprovalRequests",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "NewsId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
