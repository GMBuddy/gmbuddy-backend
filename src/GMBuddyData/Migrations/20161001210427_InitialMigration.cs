using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GMBuddyData.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Campaigns",
                columns: table => new
                {
                    CampaignId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campaigns", x => x.CampaignId);
                });

            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    CharacterId = table.Column<Guid>(nullable: false),
                    Bio = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    UserEmail = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.CharacterId);
                });

            migrationBuilder.CreateTable(
                name: "CampaignCharacters",
                columns: table => new
                {
                    CampaignCharacterId = table.Column<Guid>(nullable: false),
                    CampaignId = table.Column<Guid>(nullable: false),
                    CharacterId = table.Column<Guid>(nullable: false),
                    IsGameMaster = table.Column<bool>(nullable: false),
                    SheetId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignCharacters", x => x.CampaignCharacterId);
                    table.ForeignKey(
                        name: "FK_CampaignCharacters_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "CampaignId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CampaignCharacters_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "CharacterId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterAttributes",
                columns: table => new
                {
                    CharacterAttributesId = table.Column<Guid>(nullable: false),
                    Charisma = table.Column<uint>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    Endurance = table.Column<uint>(nullable: false),
                    Stength = table.Column<uint>(nullable: false),
                    CampaignCharacterId = table.Column<Guid>(nullable: true),
                    ItemDescription = table.Column<string>(nullable: true),
                    ItemName = table.Column<string>(nullable: true),
                    ItemType = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterAttributes", x => x.CharacterAttributesId);
                    table.ForeignKey(
                        name: "FK_CharacterAttributes_CampaignCharacters_CampaignCharacterId",
                        column: x => x.CampaignCharacterId,
                        principalTable: "CampaignCharacters",
                        principalColumn: "CampaignCharacterId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterAttributes_CampaignCharacters_CampaignCharacterId",
                        column: x => x.CampaignCharacterId,
                        principalTable: "CampaignCharacters",
                        principalColumn: "CampaignCharacterId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CampaignCharacters_CampaignId",
                table: "CampaignCharacters",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignCharacters_CharacterId",
                table: "CampaignCharacters",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterAttributes_CampaignCharacterId",
                table: "CharacterAttributes",
                column: "CampaignCharacterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacterAttributes");

            migrationBuilder.DropTable(
                name: "CampaignCharacters");

            migrationBuilder.DropTable(
                name: "Campaigns");

            migrationBuilder.DropTable(
                name: "Characters");
        }
    }
}
