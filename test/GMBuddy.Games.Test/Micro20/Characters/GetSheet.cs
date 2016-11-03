using System;
using System.Collections.Generic;
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
    public class GetSheet
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
                Name = "A campaign",
                Characters = new List<Character>
                {
                    new Character
                    {
                        Name = name,
                        UserId = userId,
                        BaseStrength = 10,
                        BaseDexterity = 10,
                        BaseMind = 10,
                        Class = Micro20ClassType.Fighter,
                        Race = Micro20RaceType.Human,
                        Level = 1
                    }
                }
            };
            CharacterSheet sheet = null;
            Type eType = null;
            using (var db = new DatabaseContext(options))
            {
                db.Campaigns.Add(campaign);
                int changes = await db.SaveChangesAsync();
                Assert.Equal(2, changes);
            }

            // Act
            var games = new GameService(options);

            try
            {
                using (var db = new DatabaseContext(options))
                {
                    var character = await db.Characters.SingleAsync();
                    sheet = await games.GetSheet(character.CharacterId, userId);
                }
            }
            catch (DataNotCreatedException e)
            {
                eType = e.GetType();
            }

            // Assert
            Assert.NotNull(sheet);
            Assert.Null(eType);
            using (var db = new DatabaseContext(options))
            {
                Assert.Equal(1, db.Characters.Count());

                var character = await db.Characters.SingleAsync();
                Assert.Equal(character.CharacterId, sheet.CharacterId);
                Assert.Equal(character.BaseStrength + 1, sheet.Strength);
                Assert.Equal(character.BaseDexterity + 1, sheet.Dexterity);
                Assert.Equal(character.BaseMind + 1, sheet.Mind);
                Assert.Equal(character.Race, sheet.Race);
                Assert.Equal(character.Class, sheet.Class);
                Assert.Equal(userId, sheet.UserId);
                Assert.Equal(1, sheet.Level);
            }
        }
    }
}
