using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace API.Migrations
{
    public partial class ictlabV3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PiID",
                table: "Classrooms",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PI",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    MacAdress = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PI", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Classrooms_PiID",
                table: "Classrooms",
                column: "PiID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Classrooms_PI_PiID",
                table: "Classrooms",
                column: "PiID",
                principalTable: "PI",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classrooms_PI_PiID",
                table: "Classrooms");

            migrationBuilder.DropTable(
                name: "PI");

            migrationBuilder.DropIndex(
                name: "IX_Classrooms_PiID",
                table: "Classrooms");

            migrationBuilder.DropColumn(
                name: "PiID",
                table: "Classrooms");
        }
    }
}
