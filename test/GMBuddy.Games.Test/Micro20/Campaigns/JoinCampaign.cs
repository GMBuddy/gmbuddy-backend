using System;
using System.Linq;
using System.Threading.Tasks;
using GMBuddy.Games.Micro20;
using GMBuddy.Games.Micro20.Data;
using GMBuddy.Games.Micro20.Models;
using GMBuddy.Games.Test.Micro20.TestUtilities;
using Xunit;
using Microsoft.EntityFrameworkCore;
using GMBuddy.Games.Micro20.InputModels;
using System.ComponentModel.DataAnnotations;

namespace GMBuddy.Games.Test.Micro20.Campaigns
{
    public class JoinCampaign
    {
        [Fact]
        public async Task Success()
        {
            // Arrange
            var options = DatabaseSetup.CreateContextOptions();
            string userId = "The Best ID";
            var campaign = new Campaign
            {
                GmUserId = userId,
                Name = "A campaign"
            };

            using (var db = new DatabaseContext(options))
            {
                db.Campaigns.Add(campaign);
                int changes = await db.SaveChangesAsync();
                Assert.Equal(1, changes);
            }

            // Act
            var games = new GameService(options);
            var character = new NewCharacter
            {
                Strength = 10,
                Dexterity = 10,
                Mind = 10,
                CampaignId = campaign.CampaignId,
                Class = Micro20ClassType.Fighter,
                Race = Micro20RaceType.Human,
                UserId = userId
            };
            var result = await games.AddCharacter(character, true);

            // Assert
            using (var db = new DatabaseContext(options))
            {
                Assert.Equal(1, db.Characters.Count());

                var dbCharacter = await db.Characters.SingleAsync();
                Assert.Equal(character.Strength, dbCharacter.BaseStrength);
                Assert.Equal(character.Dexterity, dbCharacter.BaseDexterity);
                Assert.Equal(character.Mind, dbCharacter.BaseMind);
                Assert.Equal(character.Race, dbCharacter.Race);
                Assert.Equal(character.Class, dbCharacter.Class);
                Assert.Equal(campaign.CampaignId, dbCharacter.CampaignId);
                Assert.Equal(userId, dbCharacter.UserId);
                Assert.Equal(1, dbCharacter.Level);
            }
        }

        [Theory]
        [InlineData(0, 10, 10, Micro20ClassType.Cleric, Micro20RaceType.Dwarf, "ID")]
        [InlineData(10, 0, 10, Micro20ClassType.Cleric, Micro20RaceType.Dwarf, "ID")]
        [InlineData(10, 10, 0, Micro20ClassType.Cleric, Micro20RaceType.Dwarf, "ID")]
        [InlineData(10, 10, 10, -1, Micro20RaceType.Dwarf, "ID")]
        [InlineData(10, 10, 10, Micro20ClassType.Cleric, -1, "ID")]
        [InlineData(10, 10, 10, Micro20ClassType.Cleric, Micro20RaceType.Dwarf, null)]
        public async Task Fail_BadCharacterData(int str, int dex, int mind, int classType, int raceType, string userId)
        {
            // Arrange
            var options = DatabaseSetup.CreateContextOptions();
            var campaign = new Campaign
            {
                GmUserId = userId,
                Name = "A campaign"
            };
            Type ex = null;
            Guid result;
            using (var db = new DatabaseContext(options))
            {
                db.Campaigns.Add(campaign);
                int changes = await db.SaveChangesAsync();
                Assert.Equal(1, changes);
            }

            // Act
            var games = new GameService(options);
            var character = new NewCharacter
            {
                Strength = str,
                Dexterity = dex,
                Mind = mind,
                CampaignId = campaign.CampaignId,
                Class = (Micro20ClassType) classType,
                Race = (Micro20RaceType) raceType,
                UserId = userId
            };

            try
            {
                result = await games.AddCharacter(character, true);
            }
            catch (ValidationException e)
            {
                ex = e.GetType();
            }

            // Assert
            Assert.Equal(new Guid(), result);
            using (var db = new DatabaseContext(options))
            {
                Assert.False(db.Characters.Any());
            }
        }
    }
}
