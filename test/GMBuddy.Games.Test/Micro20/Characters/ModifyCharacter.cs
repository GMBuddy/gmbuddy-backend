using System;
using System.Collections.Generic;
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
    public class ModifyCharacter
    {
        [Fact]
        public async Task Success()
        {
            // Arrange
            var options = DatabaseSetup.CreateContextOptions();
            const string userId = "The Best ID";
            var campaign = new Campaign
            {
                GmUserId = userId,
                Name = "A campaign"
            };
            var character = new Character
            {
                BaseStrength = 10,
                BaseDexterity = 10,
                BaseMind = 10,
                Class = Micro20ClassType.Cleric,
                Race = Micro20RaceType.Dwarf,
                Level = 1,
                UserId = userId
            };
            bool? result = null;
            Type eType = null;
            using (var db = new DatabaseContext(options))
            {
                db.Campaigns.Add(campaign);
                db.Characters.Add(character);
                int changes = await db.SaveChangesAsync();
                Assert.Equal(2, changes);
            }

            // Act
            var games = new GameService(options);

            try
            {
                var m1 = new CharacterModification { CharacterId = character.CharacterId, Dexterity = 12 };
                result = await games.ModifyCharacter(m1, userId, true);
                Assert.True(result);

                var m2 = new CharacterModification { CharacterId = character.CharacterId, CampaignId = campaign.CampaignId };
                result = await games.ModifyCharacter(m2, userId, true);
                Assert.True(result);
            }
            catch (Exception e) when (e is DataNotFoundException || e is ValidationException)
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
                Assert.Equal(campaign.CampaignId, dbCharacter.CampaignId);
                Assert.Equal(10, dbCharacter.BaseStrength);
                Assert.Equal(12, dbCharacter.BaseDexterity);
                Assert.Equal(10, dbCharacter.BaseMind);
                Assert.Equal(Micro20RaceType.Dwarf, dbCharacter.Race);
                Assert.Equal(Micro20ClassType.Cleric, dbCharacter.Class);
                Assert.Equal(userId, dbCharacter.UserId);
                Assert.Equal(1, dbCharacter.Level);
            }
        }
    }
}
