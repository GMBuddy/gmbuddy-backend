using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GMBuddy.Exceptions;
using GMBuddy.Games.Micro20.Data;
using GMBuddy.Games.Micro20.GameService;
using GMBuddy.Games.Micro20.Models;
using GMBuddy.Games.Test.Micro20.TestUtilities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GMBuddy.Games.Test.Micro20.Characters
{
    public class ListCharacters
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
            IEnumerable<CharacterSheet> sheets = null;
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
                    sheets = await games.ListCharacters(userId);
                }
            }
            catch (DataNotCreatedException e)
            {
                eType = e.GetType();
            }

            // Assert
            Assert.NotNull(sheets);
            Assert.Null(eType);
            Assert.Equal(1, sheets.Count());
        }
    }
}
