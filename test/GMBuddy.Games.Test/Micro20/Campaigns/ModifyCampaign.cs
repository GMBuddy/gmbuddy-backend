using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GMBuddy.Games.Micro20.Data;
using GMBuddy.Games.Micro20.GameService;
using GMBuddy.Games.Micro20.InputModels;
using GMBuddy.Games.Micro20.Models;
using GMBuddy.Games.Test.Micro20.TestUtilities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GMBuddy.Games.Test.Micro20.Campaigns
{
    public class ModifyCampaign
    {
        /// <summary>
        /// Ensures that a GM can add any characters to a campaign that they are the GM of
        /// </summary>
        [Fact]
        public async Task GM_AddCharacters()
        {
            // Arrange
            var options = DatabaseSetup.CreateContextOptions();
            const string gm = "User1";
            const string pleb = "User2";
            var games = new GameService(options);
            var user1Campaigns = await InsertTestCampaigns(options, gm);
            var campaign = user1Campaigns.First();
            var characters = new List<Character>();
            characters.AddRange(await InsertTestCharacters(options, campaign.CampaignId, gm));
            characters.AddRange(await InsertTestCharacters(options, campaign.CampaignId, pleb));
            var lonelyCharacters = characters.Where(c => c.CampaignId == null).ToList();

            // Act
            // lonelyCharacters will contain two characters with null campaigns, only one belongs to pleb
            var change = await games.ModifyCampaign(campaign.CampaignId, gm, new CampaignModification
            {
                AddCharacters = lonelyCharacters.Select(c => c.CharacterId).ToList()
            });

            // Assert
            using (var db = new DatabaseContext(options))
            {
                // characters got moved to new campaign
                var camChars = await db.Characters.Where(c => c.CampaignId == campaign.CampaignId).ToListAsync();
                Assert.Equal(4, camChars.Count);

                // characters were removed from past campaigns
                var nullChars = await db.Characters.Where(c => c.CampaignId == null).ToListAsync();
                Assert.Equal(0, nullChars.Count);

                // Name is not modified
                string camName = (await db.Campaigns.SingleAsync(c => c.CampaignId == campaign.CampaignId)).Name;
                Assert.Equal(campaign.Name, camName);

                // correct view is returned
                Assert.Equal(campaign.CampaignId, change.CampaignId);
            }
        }

        /// <summary>
        /// Ensures that a gm can remove any characters from a campaign that they are the GM of.
        /// </summary>
        [Fact]
        public async Task GM_RemoveCharacters()
        {
            // Arrange
            var options = DatabaseSetup.CreateContextOptions();
            const string gm = "User1";
            const string pleb = "User2";
            var games = new GameService(options);
            var user1Campaigns = await InsertTestCampaigns(options, gm);
            var campaign = user1Campaigns.First();
            var characters = new List<Character>();
            characters.AddRange(await InsertTestCharacters(options, campaign.CampaignId, gm));
            characters.AddRange(await InsertTestCharacters(options, campaign.CampaignId, pleb));
            var removals = characters.Where(c => c.CampaignId == campaign.CampaignId).ToList();

            // Act
            // lonelyCharacters will contain two characters with null campaigns, only one belongs to pleb
            var change = await games.ModifyCampaign(campaign.CampaignId, gm, new CampaignModification
            {
                RemoveCharacters = removals.Select(c => c.CharacterId).ToList()
            });

            // Assert
            using (var db = new DatabaseContext(options))
            {
                // characters got moved to new campaign
                var camChars = await db.Characters.Where(c => c.CampaignId == campaign.CampaignId).ToListAsync();
                Assert.Equal(0, camChars.Count);

                // characters were removed from past campaigns
                var nullChars = await db.Characters.Where(c => c.CampaignId == null).ToListAsync();
                Assert.Equal(4, nullChars.Count);

                // Name is not modified
                string camName = (await db.Campaigns.SingleAsync(c => c.CampaignId == campaign.CampaignId)).Name;
                Assert.Equal(campaign.Name, camName);

                // correct view is returned
                Assert.Equal(campaign.CampaignId, change.CampaignId);
            }
        }

        /// <summary>
        /// Ensures that a user can add their own characters to a campaign that they dont own.
        /// Ensures that they can not add characters they dont own to a campaign.
        /// Ensures that a campaign modification that does not intend to modify the name does not do so.
        /// </summary>
        [Fact]
        public async Task User_AddCharacters()
        {
            // Arrange
            var options = DatabaseSetup.CreateContextOptions();
            const string gm = "User1";
            const string pleb = "User2";
            var games = new GameService(options);
            var user1Campaigns = await InsertTestCampaigns(options, gm);
            var campaign = user1Campaigns.First();
            var characters = new List<Character>();
            characters.AddRange(await InsertTestCharacters(options, campaign.CampaignId, gm));
            characters.AddRange(await InsertTestCharacters(options, campaign.CampaignId, pleb));
            var lonelyCharacters = characters.Where(c => c.CampaignId == null).ToList();

            // Act
            // lonelyCharacters will contain two characters with null campaigns, only one belongs to pleb
            var change = await games.ModifyCampaign(campaign.CampaignId, pleb, new CampaignModification
            {
                AddCharacters = lonelyCharacters.Select(c => c.CharacterId).ToList()
            });

            // Assert
            using (var db = new DatabaseContext(options))
            {
                // characters got moved to new campaign
                var camChars = await db.Characters.Where(c => c.CampaignId == campaign.CampaignId).ToListAsync();
                Assert.Equal(3, camChars.Count);

                // characters were removed from past campaigns
                var nullChars = await db.Characters.Where(c => c.CampaignId == null).ToListAsync();
                Assert.Equal(1, nullChars.Count);

                // Name is not modified
                string camName = (await db.Campaigns.SingleAsync(c => c.CampaignId == campaign.CampaignId)).Name;
                Assert.Equal(campaign.Name, camName);

                // correct view is returned
                Assert.Equal(campaign.CampaignId, change.CampaignId);
            }
        }

        /// <summary>
        /// Adds two campaigns that the expectedUser is the GM of
        /// </summary>
        /// <param name="options"></param>
        /// <param name="expectedUser"></param>
        /// <returns>The list of campaigns added</returns>
        private async Task<List<Campaign>> InsertTestCampaigns(DbContextOptions options, string expectedUser = "A user")
        {
            var campaigns = new List<Campaign>
            {
                new Campaign
                {
                    Name = "A campaign",
                    GmUserId = expectedUser
                },
                new Campaign
                {
                    Name = "Another campaign",
                    GmUserId = expectedUser
                }
            };

            using (var db = new DatabaseContext(options))
            {
                db.Campaigns.AddRange(campaigns);
                int changes = await db.SaveChangesAsync();
                Assert.Equal(2, changes);
            }

            return campaigns;
        }

        /// <summary>
        /// Adds two characters to the database, one of which is assigned to the given campaign 
        /// and both of which are assigned to the given user
        /// </summary>
        /// <param name="options"></param>
        /// <param name="campaignId"></param>
        /// <param name="expectedUser"></param>
        /// <returns>The list of characters added</returns>
        private async Task<List<Character>> InsertTestCharacters(DbContextOptions options, Guid campaignId, string expectedUser = "A user")
        {
            var characters = new List<Character>
            {
                new Character
                {
                    BaseStrength = 10,
                    BaseDexterity = 10,
                    BaseMind = 10,
                    Class = Micro20ClassType.Cleric,
                    Race = Micro20RaceType.Dwarf,
                    Level = 1,
                    CampaignId = campaignId,
                    UserId = expectedUser
                },
                new Character
                {
                    BaseStrength = 10,
                    BaseDexterity = 10,
                    BaseMind = 10,
                    Class = Micro20ClassType.Cleric,
                    Race = Micro20RaceType.Dwarf,
                    Level = 1,
                    CampaignId = null,
                    UserId = expectedUser
                }
            };

            using (var db = new DatabaseContext(options))
            {
                db.Characters.AddRange(characters);
                int changes = await db.SaveChangesAsync();
                Assert.Equal(2, changes);
            }

            return characters;
        }
    }
}
