using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GMBuddy.Games.Micro20;
using GMBuddy.Games.Micro20.Data;
using GMBuddy.Games.Micro20.Models;
using GMBuddy.Games.Test.Micro20.TestUtilities;
using Xunit;

namespace GMBuddy.Games.Test.Micro20.Campaigns
{
    public class ListCampaigns
    {
        [Fact]
        public async Task Success_ValidUser()
        {
            // Arrange
            var options = DatabaseSetup.CreateContextOptions();
            const string expectedUser = "A User ID";
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
            var games = new GameService(options);
            var result = await games.ListCampaigns(expectedUser);

            // Assert
            Assert.Equal(campaigns.Count, result.Count());
        }

        [Fact]
        public async Task Success_InvalidUser()
        {
            // Arrange
            var options = DatabaseSetup.CreateContextOptions();
            const string creator = "A User ID";
            const string unauthorized = "A different ID";
            var campaigns = new List<Campaign>
            {
                new Campaign
                {
                    Name = "A campaign",
                    GmUserId = creator
                },
                new Campaign
                {
                    Name = "Another campaign",
                    GmUserId = creator
                }
            };

            using (var db = new DatabaseContext(options))
            {
                db.Campaigns.AddRange(campaigns);
                int changes = await db.SaveChangesAsync();
                Assert.Equal(campaigns.Count, changes);
            }

            // Act
            var games = new GameService(options);
            var result = await games.ListCampaigns(unauthorized);

            // Assert
            Assert.Equal(0, result.Count());
        }
    }
}
