using System.Collections.Generic;
using System.Threading.Tasks;
using GMBuddy.Games.Dnd35.Data;
using GMBuddy.Games.Dnd35.Models;
using Microsoft.EntityFrameworkCore;

namespace GMBuddy.Games.Dnd35
{
    public class Dnd35GameService
    {
        /// <summary>
        /// Get all campaigns (eventually with filtering options)
        /// </summary>
        /// <returns>Returns a list of campaigns. If none exist, an empty array is returned. If an error occurs, an exception is thrown</returns>
        public async Task<IEnumerable<Dnd35Campaign>> GetCampaignsAsync()
        {
            using (var db = new Dnd35DataContext())
            {
                return await db.Campaigns.ToListAsync();
            }
        }

        /// <summary>
        /// Adds a campaign with the given campaign name and GM's email to the database
        /// </summary>
        /// <param name="name">The name of the campaign</param>
        /// <param name="email">The GM's email address (uniquely identifies the GM)</param>
        /// <returns>If the campaign successfully saved or not</returns>
        public async Task<bool> AddCampaignAsync(string name, string email)
        {
            using (var db = new Dnd35DataContext())
            {
                var campaign = new Dnd35Campaign
                {
                    Name = name,
                    GmEmail = email
                };

                db.Campaigns.Add(campaign);

                int changes = await db.SaveChangesAsync();

                return changes == 1;
            }
        }
    }
}
