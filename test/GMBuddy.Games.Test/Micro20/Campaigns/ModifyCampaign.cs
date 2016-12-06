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
        [Fact]
        public async Task GM_AddCharacters()
        {
            // Arrange
            var options = DatabaseSetup.CreateContextOptions();
            string userId = new Guid().ToString();
            var games = new GameService(options);
            var cam1 = await games.AddCampaign("A campaign", userId);
            var cam2 = await games.AddCampaign("Another campaign", userId);
            var char1 = await games.CreateCharacter(new NewCharacter
            {
                Name = "char1",
                Strength = 10,
                Dexterity = 10,
                Mind = 10,
                CampaignId = cam1.CampaignId,
                Class = Micro20ClassType.Fighter,
                Race = Micro20RaceType.Human
            }, userId);
            var char2 = await games.CreateCharacter(new NewCharacter
            {
                Name = "char2",
                Strength = 10,
                Dexterity = 10,
                Mind = 10,
                CampaignId = null,
                Class = Micro20ClassType.Fighter,
                Race = Micro20RaceType.Human
            }, userId);
            var char3 = await games.CreateCharacter(new NewCharacter
            {
                Name = "char3",
                Strength = 10,
                Dexterity = 10,
                Mind = 10,
                CampaignId = cam2.CampaignId,
                Class = Micro20ClassType.Fighter,
                Race = Micro20RaceType.Human
            }, userId);

            // Act
            var change = await games.ModifyCampaign(cam1.CampaignId, userId, new CampaignModification
            {
                Name = "Modified campaign",
                AddCharacters = new List<Guid> {char2, char3}
            });

            // Assert
            using (var db = new DatabaseContext(options))
            {
                // characters got moved to new campaign
                var cam1Chars = await db.Characters.Where(c => c.CampaignId == cam1.CampaignId).ToListAsync();
                Assert.Equal(3, cam1Chars.Count);

                // characters were removed from past campaigns
                var cam2Chars = await db.Characters.Where(c => c.CampaignId == cam2.CampaignId).ToListAsync();
                var nullChars = await db.Characters.Where(c => c.CampaignId == null).ToListAsync();
                Assert.Equal(0, cam2Chars.Count);
                Assert.Equal(0, nullChars.Count);

                // Name gets modified
                string cam1Name = (await db.Campaigns.SingleAsync(c => c.CampaignId == cam1.CampaignId)).Name;
                Assert.Equal("Modified campaign", cam1Name);

                // correct view is returned
                Assert.Equal(cam1Chars.First().CampaignId, change.CampaignId);
            }
        }

        [Fact]
        public async Task GM_RemoveCharacters()
        {
            // Arrange
            var options = DatabaseSetup.CreateContextOptions();
            string userId = new Guid().ToString();
            var games = new GameService(options);
            var cam1 = await games.AddCampaign("A campaign", userId);
            var cam2 = await games.AddCampaign("Another campaign", userId);
            var char1 = await games.CreateCharacter(new NewCharacter
            {
                Name = "char1",
                Strength = 10,
                Dexterity = 10,
                Mind = 10,
                CampaignId = cam1.CampaignId,
                Class = Micro20ClassType.Fighter,
                Race = Micro20RaceType.Human
            }, userId);
            var char2 = await games.CreateCharacter(new NewCharacter
            {
                Name = "char2",
                Strength = 10,
                Dexterity = 10,
                Mind = 10,
                CampaignId = null,
                Class = Micro20ClassType.Fighter,
                Race = Micro20RaceType.Human
            }, userId);
            var char3 = await games.CreateCharacter(new NewCharacter
            {
                Name = "char3",
                Strength = 10,
                Dexterity = 10,
                Mind = 10,
                CampaignId = cam2.CampaignId,
                Class = Micro20ClassType.Fighter,
                Race = Micro20RaceType.Human
            }, userId);

            // Act
            var change = await games.ModifyCampaign(cam1.CampaignId, userId, new CampaignModification
            {
                Name = "Modified campaign",
                RemoveCharacters = new List<Guid> { char1, char2 }
            });

            // Assert
            using (var db = new DatabaseContext(options))
            {
                // characters got moved to new campaign
                var cam1Chars = await db.Characters.Where(c => c.CampaignId == cam1.CampaignId).ToListAsync();
                Assert.Equal(0, cam1Chars.Count);

                // characters were removed from past campaigns
                var cam2Chars = await db.Characters.Where(c => c.CampaignId == cam2.CampaignId).ToListAsync();
                var nullChars = await db.Characters.Where(c => c.CampaignId == null).ToListAsync();
                Assert.Equal(1, cam2Chars.Count);
                Assert.Equal(2, nullChars.Count);

                // Name gets modified
                string cam1Name = (await db.Campaigns.SingleAsync(c => c.CampaignId == cam1.CampaignId)).Name;
                Assert.Equal("Modified campaign", cam1Name);

                // correct view is returned
                Assert.Equal(cam1.CampaignId, change.CampaignId);
            }
        }

        [Fact]
        public async Task User_AddCharacters()
        {
            // Arrange
            var options = DatabaseSetup.CreateContextOptions();
            const string gm = "GM";
            const string user1 = "User1";
            const string user2 = "User2";
            var games = new GameService(options);
            var cam1 = await games.AddCampaign("A campaign", gm);
            var cam2 = await games.AddCampaign("Another campaign", gm);
            var char1 = await games.CreateCharacter(new NewCharacter
            {
                Name = "char1",
                Strength = 10,
                Dexterity = 10,
                Mind = 10,
                CampaignId = null,
                Class = Micro20ClassType.Fighter,
                Race = Micro20RaceType.Human
            }, user1);
            var char2 = await games.CreateCharacter(new NewCharacter
            {
                Name = "char2",
                Strength = 10,
                Dexterity = 10,
                Mind = 10,
                CampaignId = null,
                Class = Micro20ClassType.Fighter,
                Race = Micro20RaceType.Human
            }, user1);
            var char3 = await games.CreateCharacter(new NewCharacter
            {
                Name = "char3",
                Strength = 10,
                Dexterity = 10,
                Mind = 10,
                CampaignId = null,
                Class = Micro20ClassType.Fighter,
                Race = Micro20RaceType.Human
            }, user2);

            // Act

            // user1 only owns char1 and char2, so the result of this will only add char2 to cam1
            var change = await games.ModifyCampaign(cam1.CampaignId, user1, new CampaignModification
            {
                AddCharacters = new List<Guid> { char2, char3 }
            });

            // Assert
            using (var db = new DatabaseContext(options))
            {
                // characters got moved to new campaign
                var cam1Chars = await db.Characters.Where(c => c.CampaignId == cam1.CampaignId).ToListAsync();
                Assert.Equal(1, cam1Chars.Count);

                // characters were removed from past campaigns
                var cam2Chars = await db.Characters.Where(c => c.CampaignId == cam2.CampaignId).ToListAsync();
                var nullChars = await db.Characters.Where(c => c.CampaignId == null).ToListAsync();
                Assert.Equal(0, cam2Chars.Count);
                Assert.Equal(2, nullChars.Count);

                // Name gets modified
                string cam1Name = (await db.Campaigns.SingleAsync(c => c.CampaignId == cam1.CampaignId)).Name;
                Assert.Equal("A campaign", cam1Name);

                // correct view is returned
                Assert.Equal(cam1Chars.First().CampaignId, change.CampaignId);
            }
        }
    }
}
