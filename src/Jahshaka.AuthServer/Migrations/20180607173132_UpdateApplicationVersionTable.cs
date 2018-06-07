using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Jahshaka.AuthServer.Migrations
{
    public partial class UpdateApplicationVersionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LinuxUrl",
                table: "ApplicationVersions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MacUrl",
                table: "ApplicationVersions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WindowsUrl",
                table: "ApplicationVersions",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LinuxUrl",
                table: "ApplicationVersions");

            migrationBuilder.DropColumn(
                name: "MacUrl",
                table: "ApplicationVersions");

            migrationBuilder.DropColumn(
                name: "WindowsUrl",
                table: "ApplicationVersions");
        }
    }
}
