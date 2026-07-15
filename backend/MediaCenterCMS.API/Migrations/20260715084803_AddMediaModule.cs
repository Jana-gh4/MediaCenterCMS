using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MediaCenterCMS.API.Migrations
{
    /// <inheritdoc />
    public partial class AddMediaModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CoverMediaId",
                table: "NewsVersions",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Media",
                columns: table => new
                {
                    MediaId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FileName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    FilePath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ContentType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    MediaType = table.Column<int>(type: "integer", nullable: false),
                    UploadedBy = table.Column<int>(type: "integer", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Media", x => x.MediaId);
                    table.ForeignKey(
                        name: "FK_Media_Users_UploadedBy",
                        column: x => x.UploadedBy,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NewsVersions_CoverMediaId",
                table: "NewsVersions",
                column: "CoverMediaId");

            migrationBuilder.CreateIndex(
                name: "IX_Media_UploadedBy",
                table: "Media",
                column: "UploadedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsVersions_Media_CoverMediaId",
                table: "NewsVersions",
                column: "CoverMediaId",
                principalTable: "Media",
                principalColumn: "MediaId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsVersions_Media_CoverMediaId",
                table: "NewsVersions");

            migrationBuilder.DropTable(
                name: "Media");

            migrationBuilder.DropIndex(
                name: "IX_NewsVersions_CoverMediaId",
                table: "NewsVersions");

            migrationBuilder.DropColumn(
                name: "CoverMediaId",
                table: "NewsVersions");
        }
    }
}
