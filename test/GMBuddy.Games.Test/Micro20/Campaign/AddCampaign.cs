﻿using System;
using System.Linq;
using System.Threading.Tasks;
using GMBuddy.Games.Micro20;
using GMBuddy.Games.Micro20.Data;
using GMBuddy.Games.Test.Micro20.TestUtilities;
using Xunit;
using Microsoft.EntityFrameworkCore;

namespace GMBuddy.Games.Test.Micro20.Campaign
{
    public class AddCampaign
    {
        [Fact]
        public async Task Success()
        {
            // Arrange
            var options = DatabaseSetup.CreateContextOptions();
            string userId = new Guid().ToString();

            // Act
            var games = new Micro20GameService(options);
            var first = await games.AddCampaignAsync("A campaign", userId);
            Assert.NotNull(first);

            var second = await games.AddCampaignAsync("Another campaign", userId);
            Assert.NotNull(second);

            // Assert
            using (var db = new Micro20DataContext(options))
            {
                Assert.Equal(2, db.Campaigns.Count());

                var firstCampaign = await db.Campaigns.FirstAsync();
                Assert.Equal("A campaign", firstCampaign.Name);
                Assert.Equal(userId, firstCampaign.GmUserId);

                var secondCampaign = await db.Campaigns.LastAsync();
                Assert.Equal("Another campaign", secondCampaign.Name);
                Assert.Equal(userId, secondCampaign.GmUserId);
            }
        }
    }
}
