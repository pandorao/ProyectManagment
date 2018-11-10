using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ProyectManagement.Data.Migrations
{
    public partial class AddProyectTableApplicaationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Proyects",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Proyects_ApplicationUserId",
                table: "Proyects",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Proyects_AspNetUsers_ApplicationUserId",
                table: "Proyects",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Proyects_AspNetUsers_ApplicationUserId",
                table: "Proyects");

            migrationBuilder.DropIndex(
                name: "IX_Proyects_ApplicationUserId",
                table: "Proyects");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Proyects");
        }
    }
}
