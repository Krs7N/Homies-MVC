﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Homies.Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Types",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Type Identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false, comment: "The name of the Type")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Types", x => x.Id);
                },
                comment: "Type table");

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Event Identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "The name of the Event"),
                    Description = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "The description of the Event"),
                    OrganiserId = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "The identifier of the Organiser of the Event"),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "The date and time when the Event was created"),
                    Start = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "The date and time when the Event starts"),
                    End = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "The date and time when the Event ends"),
                    TypeId = table.Column<int>(type: "int", nullable: false, comment: "The identifier of the Type of the Event")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_AspNetUsers_OrganiserId",
                        column: x => x.OrganiserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Events_Types_TypeId",
                        column: x => x.TypeId,
                        principalTable: "Types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Event table");

            migrationBuilder.CreateTable(
                name: "EventsParticipants",
                columns: table => new
                {
                    HelperId = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "User Identifier"),
                    EventId = table.Column<int>(type: "int", nullable: false, comment: "Event Identifier")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventsParticipants", x => new { x.HelperId, x.EventId });
                    table.ForeignKey(
                        name: "FK_EventsParticipants_AspNetUsers_HelperId",
                        column: x => x.HelperId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventsParticipants_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "EventParticipant table, mapping table between the participants that are in current event");

            migrationBuilder.InsertData(
                table: "Types",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Animals" },
                    { 2, "Fun" },
                    { 3, "Discussion" },
                    { 4, "Work" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_OrganiserId",
                table: "Events",
                column: "OrganiserId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_TypeId",
                table: "Events",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EventsParticipants_EventId",
                table: "EventsParticipants",
                column: "EventId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventsParticipants");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Types");
        }
    }
}
