using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Jahshaka.AuthServer.Migrations
{
    public partial class AddWorldVersionAssetTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorldVersionAssets",
                columns: table => new
                {
                    AssetId = table.Column<Guid>(nullable: false),
                    WorldVersionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorldVersionAssets", x => new { x.AssetId, x.WorldVersionId });
                    table.ForeignKey(
                        name: "FK_WorldVersionAssets_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorldVersionAssets_WorldVersions_WorldVersionId",
                        column: x => x.WorldVersionId,
                        principalTable: "WorldVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorldVersionAssets_WorldVersionId",
                table: "WorldVersionAssets",
                column: "WorldVersionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorldVersionAssets");
        }
    }
}
