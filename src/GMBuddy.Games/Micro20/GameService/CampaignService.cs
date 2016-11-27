using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GMBuddy.Exceptions;
using GMBuddy.Games.Micro20.Data;
using GMBuddy.Games.Micro20.Models;
using GMBuddy.Games.Micro20.OutputModels;
using Microsoft.EntityFrameworkCore;

namespace GMBuddy.Games.Micro20.GameService
{
    public partial class GameService
    {
        /// <summary>
        /// Get all campaigns (eventually with filtering options)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Returns a list of campaigns. If none exist, an empty array is returned. If an error occurs, an exception is thrown</returns>
        public async Task<IEnumerable<CampaignView>> ListCampaigns(string userId)
        {
            using (var db = new DatabaseContext(options))
            {
                var campaigns = await db.Campaigns
                    .Include(c => c.Characters)
                    .Where(c => c.GmUserId == userId || c.Characters.Any(ch => ch.UserId == userId))
                    .ToListAsync();

                return campaigns.Select(c => new CampaignView(c));
            }
        }

        /// <summary>
        /// Get a single campaign
        /// </summary>
        /// <param name="campaignId">The ID of the campaign to retrieve</param>
        /// <param name="userId">A user ID, which currently must be the campaign's GM</param>
        /// <exception cref="DataNotFoundException">If no such campaign exists</exception>
        /// <exception cref="UnauthorizedException">If the campaign exists but the user is unauthorized to view it</exception>
        /// <returns></returns>
        public async Task<CampaignView> GetCampaign(Guid campaignId, string userId)
        {
            using (var db = new DatabaseContext(options))
            {
                var campaign = await db.Campaigns
                    .Include(c => c.Characters)
                    .SingleOrDefaultAsync(c => c.CampaignId == campaignId);

                if (campaign == null)
                {
                    throw new DataNotFoundException($"Could not find campaign {campaignId}");
                }

                // ReSharper disable once SimplifyLinqExpression
                if (campaign.GmUserId != userId && !campaign.Characters.Any(c => c.UserId == userId))
                {
                    throw new UnauthorizedException($"User {userId} does not have permission to access {campaignId}");
                }

                return new CampaignView(campaign);
            }
        }

        /// <summary>
        /// Adds a campaign with the given campaign name and GM's userId to the database
        /// </summary>
        /// <param name="name">The name of the campaign</param>
        /// <param name="userId">The GM's userId address (uniquely identifies the GM)</param>
        /// <exception cref="ArgumentException">If name is invalid</exception>
        /// <exception cref="DataNotCreatedException">If the campaign could not be saved to the database</exception>
        /// <returns>The ID of the added campaign</returns>
        public async Task<CampaignView> AddCampaign(string name, string userId)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name is invalid", nameof(name));
            }

            using (var db = new DatabaseContext(options))
            {
                var campaign = new Campaign
                {
                    Name = name,
                    GmUserId = userId
                };

                db.Campaigns.Add(campaign);

                int changes = await db.SaveChangesAsync();
                if (changes != 1)
                {
                    throw new DataNotCreatedException("Could not save campaign");
                }

                return new CampaignView(campaign);
            }
        }
    }
}
