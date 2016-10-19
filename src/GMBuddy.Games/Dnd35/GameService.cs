using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GMBuddy.Games.Dnd35.Data;
using GMBuddy.Games.Dnd35.Models;
using Microsoft.EntityFrameworkCore;

namespace GMBuddy.Games.Dnd35
{
    public class GameService
    {
        public async Task<IEnumerable<Campaign>> GetCampaigns()
        {
            using (var db = new Dnd35DataContext())
            {
                return await db.Campaigns.ToListAsync();
            }
        }

        public async Task<bool> AddCampaignAsync(string name, string email)
        {
            using (var db = new Dnd35DataContext())
            {
                var campaign = new Campaign
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
