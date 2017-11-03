using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Jahshaka.AuthServer.Migrations
{
    public partial class UpdateTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WorldId",
                table: "WorldVersionAssets",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_WorldVersionAssets_WorldId",
                table: "WorldVersionAssets",
                column: "WorldId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorldVersionAssets_Worlds_WorldId",
                table: "WorldVersionAssets",
                column: "WorldId",
                principalTable: "Worlds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorldVersionAssets_Worlds_WorldId",
                table: "WorldVersionAssets");

            migrationBuilder.DropIndex(
                name: "IX_WorldVersionAssets_WorldId",
                table: "WorldVersionAssets");

            migrationBuilder.DropColumn(
                name: "WorldId",
                table: "WorldVersionAssets");
        }
    }
}
