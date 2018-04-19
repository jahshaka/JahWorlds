using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Jahshaka.AuthServer.Migrations
{
    public partial class AddUserToCollectionEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Collections",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Collections_UserId",
                table: "Collections",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Collections_Users_UserId",
                table: "Collections",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Collections_Users_UserId",
                table: "Collections");

            migrationBuilder.DropIndex(
                name: "IX_Collections_UserId",
                table: "Collections");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Collections");
        }
    }
}
