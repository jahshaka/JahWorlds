using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Jahshaka.AuthServer.Migrations
{
    public partial class UpdateApplicationVersionTB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DownloadUrl",
                table: "ApplicationVersions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "ApplicationVersions",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DownloadUrl",
                table: "ApplicationVersions");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "ApplicationVersions");
        }
    }
}
