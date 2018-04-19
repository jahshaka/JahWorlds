using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Jahshaka.AuthServer.Migrations
{
    public partial class UpdateApplicationVersionReleaseDateTB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ReleaseDate",
                table: "ApplicationVersions",
                type: "timestamp",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReleaseDate",
                table: "ApplicationVersions");
        }
    }
}
