using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GMBuddy.Exceptions;
using GMBuddy.Games.Micro20;
using GMBuddy.Games.Micro20.Data;
using GMBuddy.Games.Micro20.GameService;
using GMBuddy.Games.Micro20.InputModels;
using GMBuddy.Games.Micro20.Models;
using GMBuddy.Games.Test.Micro20.TestUtilities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GMBuddy.Games.Test.Micro20.Characters
{
    public class CreateCharacter
    {
        [Fact]
        public async Task Success_WithCampaign()
        {
            // Arrange
            var options = DatabaseSetup.CreateContextOptions();
            const string userId = "The Best ID";
            const string name = "My Character";
            var campaign = new Campaign
            {
                GmUserId = userId,
                Name = "A campaign"
            };
            Guid? result = null;
            Type eType = null;
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
                Name = name,
                Strength = 10,
                Dexterity = 10,
                Mind = 10,
                CampaignId = campaign.CampaignId,
                Class = Micro20ClassType.Fighter,
                Race = Micro20RaceType.Human
            };

            try
            {
                result = await games.CreateCharacter(character, userId, true);
            }
            catch (DataNotCreatedException e)
            {
                eType = e.GetType();
            }

            // Assert
            Assert.NotNull(result);
            Assert.Null(eType);
            using (var db = new DatabaseContext(options))
            {
                Assert.Equal(1, db.Characters.Count());

                var dbCharacter = await db.Characters.SingleAsync();
                Assert.Equal(result, dbCharacter.CharacterId);
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

        public async Task Success_WithoutCampaign()
        {
            // Arrange
            var options = DatabaseSetup.CreateContextOptions();
            const string userId = "The Best ID";
            const string name = "My Character";
            Guid? result = null;
            Type eType = null;

            // Act
            var games = new GameService(options);
            var character = new NewCharacter
            {
                Name = name,
                Strength = 10,
                Dexterity = 10,
                Mind = 10,
                CampaignId = null,
                Class = Micro20ClassType.Fighter,
                Race = Micro20RaceType.Human
            };

            try
            {
                result = await games.CreateCharacter(character, userId, true);
            }
            catch (DataNotCreatedException e)
            {
                eType = e.GetType();
            }

            // Assert
            Assert.NotNull(result);
            Assert.Null(eType);
            using (var db = new DatabaseContext(options))
            {
                Assert.Equal(1, db.Characters.Count());

                var dbCharacter = await db.Characters.SingleAsync();
                Assert.Equal(result, dbCharacter.CharacterId);
                Assert.Equal(character.Strength, dbCharacter.BaseStrength);
                Assert.Equal(character.Dexterity, dbCharacter.BaseDexterity);
                Assert.Equal(character.Mind, dbCharacter.BaseMind);
                Assert.Equal(character.Race, dbCharacter.Race);
                Assert.Equal(character.Class, dbCharacter.Class);
                Assert.Null(dbCharacter.CampaignId);
                Assert.Equal(userId, dbCharacter.UserId);
                Assert.Equal(1, dbCharacter.Level);
            }
        }

        [Theory]
        [InlineData(null, 10, 10, 10, Micro20ClassType.Cleric, Micro20RaceType.Dwarf)]
        [InlineData("My Character", 0, 10, 10, Micro20ClassType.Cleric, Micro20RaceType.Dwarf)]
        [InlineData("My Character", 10, 0, 10, Micro20ClassType.Cleric, Micro20RaceType.Dwarf)]
        [InlineData("My Character", 10, 10, 0, Micro20ClassType.Cleric, Micro20RaceType.Dwarf)]
        [InlineData("My Character", 10, 10, 10, -1, Micro20RaceType.Dwarf)]
        [InlineData("My Character", 10, 10, 10, Micro20ClassType.Cleric, -1)]
        public async Task Fail_BadCharacterData(string name, int str, int dex, int mind, int classType, int raceType)
        {
            // Arrange
            var options = DatabaseSetup.CreateContextOptions();
            const string userId = "A user id";
            var campaign = new Campaign
            {
                GmUserId = userId,
                Name = "A campaign"
            };
            Type ex = null;
            Guid? result = null;
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
                Name = name,
                Strength = str,
                Dexterity = dex,
                Mind = mind,
                CampaignId = campaign.CampaignId,
                Class = (Micro20ClassType) classType,
                Race = (Micro20RaceType) raceType
            };

            try
            {
                result = await games.CreateCharacter(character, userId, true);
            }
            catch (ValidationException e)
            {
                ex = e.GetType();
            }

            // Assert
            Assert.Null(result);
            Assert.NotNull(ex);
            using (var db = new DatabaseContext(options))
            {
                Assert.False(db.Characters.Any());
            }
        }
    }
}
