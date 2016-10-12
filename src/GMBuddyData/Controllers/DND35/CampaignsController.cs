using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GMBuddyData.Data.DND35;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GMBuddyData.Models.DND35;
using GMBuddyData.ViewModels;
using Newtonsoft.Json;

namespace GMBuddyData.Controllers.DND35
{
    [Area("DND35")]
    public class CampaignsController : Controller
    {
        private GameContext db;

        public CampaignsController(GameContext db)
        {
            this.db = db;
        }

        // GET: dnd35/campaigns
        // GET: dnd35/campaigns/index
        [HttpGet]
        public async Task<IActionResult> Index(string email = null, string campaignId = null, string characterId = null)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            // get a full list of campaigns, making sure to include related CampaignCharacters and Characters
            // dont issue the query yet with ToListAsync()
            var campaigns = db.Campaigns
                .Include(campaign => campaign.CampaignCharacters)
                .ThenInclude(cc => cc.Character);

            // returns all campaigns that have a character owned by a given email in them,
            // or where the campaign is GM'd by that email
            if (email != null)
            {
                IEnumerable<ICampaignView> filtered = await campaigns
                    .Where(c => c.CampaignCharacters.Any(cc => cc.Character.UserEmail == email) 
                             || c.GmEmail == email)
                    .ToListAsync();
                return OkOrNotFound(filtered);
            }

            // returns the campaign with the given campaignId
            if (campaignId != null)
            {
                ICampaignView filtered = await campaigns.FirstOrDefaultAsync(c => c.CampaignId.ToString().Equals(campaignId));
                return OkOrNotFound(filtered);
            }

            // returns all campaigns that have a character with a given characterId in them
            if (characterId != null)
            {
                IEnumerable<ICampaignView> filtered = await campaigns
                    .Where(c => c.CampaignCharacters.Any(cc => cc.CharacterId.ToString().Equals(characterId)))
                    .ToListAsync();
                return OkOrNotFound(filtered);
            }

            // if we have gotten here, there are no filters, and that is bad
            return BadRequest();
        }

        // POST: dnd35/campaigns
        // POST: dnd35/campaigns/index
        [HttpPost]
        public async Task<IActionResult> Index([FromBody] ICampaignCreationView model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var campaign = new Campaign
            {
                Name = model.Name,
                GmEmail = model.GmEmail
            };

            db.Campaigns.Add(campaign);

            var changes = await db.SaveChangesAsync();
            if (changes != 1)
            {
                return BadRequest();
            }

            return new CreatedResult($"/dnd35/campaigns/{campaign.CampaignId}", null);
        }

        private IActionResult OkOrNotFound<T>(T result)
        {
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}
