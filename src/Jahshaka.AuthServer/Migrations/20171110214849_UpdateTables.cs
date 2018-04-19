using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Jahshaka.AuthServer.Migrations
{
    public partial class UpdateTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationVersion_OpenIddictApplications_ApplicationId",
                table: "ApplicationVersion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationVersion",
                table: "ApplicationVersion");

            migrationBuilder.RenameTable(
                name: "ApplicationVersion",
                newName: "ApplicationVersions");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationVersion_ApplicationId",
                table: "ApplicationVersions",
                newName: "IX_ApplicationVersions_ApplicationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationVersions",
                table: "ApplicationVersions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationVersions_OpenIddictApplications_ApplicationId",
                table: "ApplicationVersions",
                column: "ApplicationId",
                principalTable: "OpenIddictApplications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationVersions_OpenIddictApplications_ApplicationId",
                table: "ApplicationVersions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationVersions",
                table: "ApplicationVersions");

            migrationBuilder.RenameTable(
                name: "ApplicationVersions",
                newName: "ApplicationVersion");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationVersions_ApplicationId",
                table: "ApplicationVersion",
                newName: "IX_ApplicationVersion_ApplicationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationVersion",
                table: "ApplicationVersion",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationVersion_OpenIddictApplications_ApplicationId",
                table: "ApplicationVersion",
                column: "ApplicationId",
                principalTable: "OpenIddictApplications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
