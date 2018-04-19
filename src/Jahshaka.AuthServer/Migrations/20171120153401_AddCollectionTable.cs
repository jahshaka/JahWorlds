using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Jahshaka.AuthServer.Migrations
{
    public partial class AddCollectionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CollectionId",
                table: "Assets",
                type: "int4",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Collection",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CollectionId = table.Column<int>(type: "int4", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Collection_Collection_CollectionId",
                        column: x => x.CollectionId,
                        principalTable: "Collection",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assets_CollectionId",
                table: "Assets",
                column: "CollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Collection_CollectionId",
                table: "Collection",
                column: "CollectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_Collection_CollectionId",
                table: "Assets",
                column: "CollectionId",
                principalTable: "Collection",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assets_Collection_CollectionId",
                table: "Assets");

            migrationBuilder.DropTable(
                name: "Collection");

            migrationBuilder.DropIndex(
                name: "IX_Assets_CollectionId",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "CollectionId",
                table: "Assets");
        }
    }
}
