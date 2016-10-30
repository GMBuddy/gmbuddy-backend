using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GMBuddy.Games.Micro20.Data;
using GMBuddy.Games.Micro20.Models;
using Microsoft.EntityFrameworkCore;

namespace GMBuddy.Games.Micro20
{
    public class Micro20GameService
    {
        private readonly DbContextOptions options;

        public Micro20GameService(DbContextOptions options)
        {
            this.options = options;
        }

        /// <summary>
        /// Get all campaigns (eventually with filtering options)
        /// </summary>
        /// <returns>Returns a list of campaigns. If none exist, an empty array is returned. If an error occurs, an exception is thrown</returns>
        public async Task<IEnumerable<Micro20Campaign>> GetCampaignsAsync()
        {
            using (var db = new Micro20DataContext(options))
            {
                return await db.Campaigns.ToListAsync();
            }
        }

        /// <summary>
        /// Adds a campaign with the given campaign name and GM's userId to the database
        /// </summary>
        /// <param name="name">The name of the campaign</param>
        /// <param name="userId">The GM's userId address (uniquely identifies the GM)</param>
        /// <returns>The ID of the added campaign</returns>
        public async Task<Guid> AddCampaignAsync(string name, string userId)
        {
            using (var db = new Micro20DataContext(options))
            {
                var campaign = new Micro20Campaign
                {
                    Name = name,
                    GmUserId = userId
                };

                db.Campaigns.Add(campaign);

                int changes = await db.SaveChangesAsync();

                if (changes != 1)
                {
                    throw new Exception("Could not save campaign");
                }

                return campaign.CampaignId;
            }
        }
    }
}
