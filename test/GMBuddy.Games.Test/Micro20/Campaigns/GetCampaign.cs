using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GMBuddy.Exceptions;
using GMBuddy.Games.Micro20;
using GMBuddy.Games.Micro20.Data;
using GMBuddy.Games.Micro20.GameService;
using GMBuddy.Games.Micro20.Models;
using GMBuddy.Games.Test.Micro20.TestUtilities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GMBuddy.Games.Test.Micro20.Campaigns
{
    public class GetCampaign
    {
        [Fact]
        public async Task Success_IsGM()
        {
            // Arrange
            var options = DatabaseSetup.CreateContextOptions();
            const string expectedUser = "A User ID";
            Campaign result = null;
            Type eType = null;
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
                Assert.Equal(campaigns.Count, changes);
            }

            // Act
            using (var db = new DatabaseContext(options))
            {
                var games = new GameService(options);
                var campaignId = (await db.Campaigns.FirstAsync()).CampaignId;

                try
                {
                    result = await games.GetCampaign(campaignId, expectedUser);
                }
                catch (Exception e)
                {
                    eType = e.GetType();
                }
            }
            

            // Assert
            Assert.NotNull(result);
            Assert.Null(eType);
        }

        [Fact]
        public async Task Success_HasCharacter()
        {
            // Arrange
            var options = DatabaseSetup.CreateContextOptions();
            const string expectedUser = "A User ID";
            Campaign result = null;
            Type eType = null;

            using (var db = new DatabaseContext(options))
            {
                db.Campaigns.Add(new Campaign
                {
                    Name = "A campaign",
                    GmUserId = "Some random dude",
                    Characters = new List<Character>
                    {
                        new Character
                        {
                            BaseStrength = 10,
                            BaseDexterity = 10,
                            BaseMind = 10,
                            Class = Micro20ClassType.Cleric,
                            Race = Micro20RaceType.Dwarf,
                            Level = 1,
                            UserId = expectedUser
                        }
                    }
                });
                int changes = await db.SaveChangesAsync();
                Assert.Equal(2, changes);
            }

            // Act
            using (var db = new DatabaseContext(options))
            {
                var games = new GameService(options);
                var campaignId = (await db.Campaigns.FirstAsync()).CampaignId;

                try
                {
                    result = await games.GetCampaign(campaignId, expectedUser);
                }
                catch (Exception e)
                {
                    eType = e.GetType();
                }
            }


            // Assert
            Assert.NotNull(result);
            Assert.Null(eType);
        }

        [Fact]
        public async Task Success_InvalidUser()
        {
            // Arrange
            var options = DatabaseSetup.CreateContextOptions();
            const string expectedUser = "A User ID";
            const string unauthorized = "A different ID";
            Campaign result = null;
            Type eType = null;
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
                Assert.Equal(campaigns.Count, changes);
            }

            // Act
            using (var db = new DatabaseContext(options))
            {
                var games = new GameService(options);
                var campaignId = (await db.Campaigns.FirstAsync()).CampaignId;

                try
                {
                    result = await games.GetCampaign(campaignId, unauthorized);
                }
                catch (Exception e)
                {
                    eType = e.GetType();
                }
            }
            
            // Assert
            Assert.Null(result);
            Assert.NotNull(eType);
            Assert.Equal(typeof(UnauthorizedException), eType);
        }
    }
}
