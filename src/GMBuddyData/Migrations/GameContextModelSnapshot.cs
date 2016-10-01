using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using GMBuddyData.Data.DND35;

namespace GMBuddyData.Migrations
{
    [DbContext(typeof(GameContext))]
    partial class GameContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1");

            modelBuilder.Entity("GMBuddyData.Models.DND35.Campaign", b =>
                {
                    b.Property<Guid>("CampaignId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("CampaignId");

                    b.ToTable("Campaigns");
                });

            modelBuilder.Entity("GMBuddyData.Models.DND35.CampaignCharacter", b =>
                {
                    b.Property<Guid>("CampaignCharacterId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CampaignId");

                    b.Property<Guid>("CharacterId");

                    b.Property<bool>("IsGameMaster");

                    b.Property<Guid>("SheetId");

                    b.HasKey("CampaignCharacterId");

                    b.HasIndex("CampaignId");

                    b.HasIndex("CharacterId");

                    b.ToTable("CampaignCharacters");
                });

            modelBuilder.Entity("GMBuddyData.Models.DND35.Character", b =>
                {
                    b.Property<Guid>("CharacterId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Bio");

                    b.Property<string>("Name");

                    b.Property<string>("UserEmail");

                    b.HasKey("CharacterId");

                    b.ToTable("Characters");
                });

            modelBuilder.Entity("GMBuddyData.Models.DND35.CharacterAttributes", b =>
                {
                    b.Property<Guid>("CharacterAttributesId")
                        .ValueGeneratedOnAdd();

                    b.Property<uint>("Charisma");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<uint>("Endurance");

                    b.Property<uint>("Stength");

                    b.HasKey("CharacterAttributesId");

                    b.ToTable("CharacterAttributes");

                    b.HasDiscriminator<string>("Discriminator").HasValue("CharacterAttributes");
                });

            modelBuilder.Entity("GMBuddyData.Models.DND35.Item", b =>
                {
                    b.HasBaseType("GMBuddyData.Models.DND35.CharacterAttributes");

                    b.Property<Guid>("CampaignCharacterId");

                    b.Property<string>("ItemDescription");

                    b.Property<string>("ItemName")
                        .IsRequired();

                    b.Property<int>("ItemType");

                    b.HasIndex("CampaignCharacterId");

                    b.ToTable("Item");

                    b.HasDiscriminator().HasValue("Item");
                });

            modelBuilder.Entity("GMBuddyData.Models.DND35.Sheet", b =>
                {
                    b.HasBaseType("GMBuddyData.Models.DND35.CharacterAttributes");

                    b.Property<Guid>("CampaignCharacterId");

                    b.HasIndex("CampaignCharacterId")
                        .IsUnique();

                    b.ToTable("Sheet");

                    b.HasDiscriminator().HasValue("Sheet");
                });

            modelBuilder.Entity("GMBuddyData.Models.DND35.CampaignCharacter", b =>
                {
                    b.HasOne("GMBuddyData.Models.DND35.Campaign", "Campaign")
                        .WithMany("CampaignCharacters")
                        .HasForeignKey("CampaignId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GMBuddyData.Models.DND35.Character", "Character")
                        .WithMany("CampaignCharacters")
                        .HasForeignKey("CharacterId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GMBuddyData.Models.DND35.Item", b =>
                {
                    b.HasOne("GMBuddyData.Models.DND35.CampaignCharacter")
                        .WithMany("Items")
                        .HasForeignKey("CampaignCharacterId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GMBuddyData.Models.DND35.Sheet", b =>
                {
                    b.HasOne("GMBuddyData.Models.DND35.CampaignCharacter", "CampaignCharacter")
                        .WithOne("Sheet")
                        .HasForeignKey("GMBuddyData.Models.DND35.Sheet", "CampaignCharacterId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
