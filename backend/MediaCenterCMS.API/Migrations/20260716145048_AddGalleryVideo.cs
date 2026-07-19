using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MediaCenterCMS.API.Migrations
{
    /// <inheritdoc />
    public partial class AddGalleryVideo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GalleryVideos",
                columns: table => new
                {
                    GalleryVideoId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    MediaId = table.Column<int>(type: "integer", nullable: false),
                    ApprovalRequestId = table.Column<int>(type: "integer", nullable: true),
                    VisibilityStatus = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GalleryVideos", x => x.GalleryVideoId);
                    table.ForeignKey(
                        name: "FK_GalleryVideos_ApprovalRequests_ApprovalRequestId",
                        column: x => x.ApprovalRequestId,
                        principalTable: "ApprovalRequests",
                        principalColumn: "ApprovalRequestId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GalleryVideos_Media_MediaId",
                        column: x => x.MediaId,
                        principalTable: "Media",
                        principalColumn: "MediaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GalleryVideos_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GalleryVideos_ApprovalRequestId",
                table: "GalleryVideos",
                column: "ApprovalRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_GalleryVideos_CreatedBy",
                table: "GalleryVideos",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_GalleryVideos_MediaId",
                table: "GalleryVideos",
                column: "MediaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GalleryVideos");
        }
    }
}
