﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GMBuddyData.Data.DND35;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GMBuddyData.Models.DND35;
using GMBuddyData.Models.DND35.CampaignViewModels;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

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
        public async Task<IActionResult> Index(string email = null)
        {
            // get a full list of campaigns, making sure to include related CampaignCharacters and Characters
            // dont issue the query yet with ToListAsync()
            var campaigns = db.Campaigns
                .Include(campaign => campaign.CampaignCharacters)
                .ThenInclude(cc => cc.Character);

            // Add conditional where if we have filter criteria
            var filtered = email == null
                ? await campaigns.ToListAsync()
                : await campaigns.Where(c => c.CampaignCharacters.Any(cc => cc.Character.UserEmail == email)).ToListAsync();

            // return a pretty version of the campaigns
            // TODO: Return actual serializable models
            return Json(filtered.Select(c => new
            {
                c.Name,
                GameMaster = c.GmEmail,
                Characters = c.CampaignCharacters.Select(cc => new
                {
                    cc.Character.Name,
                    cc.Character.Bio,
                    Email = cc.Character.UserEmail
                })
            }));
        }

        // POST: dnd35/campaigns
        // POST: dnd35/campaigns/index
        [HttpPost]
        public async Task<IActionResult> Index([FromBody] CreateCampaignVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var campaign = new Campaign
            {
                Name = model.Name,
                GmEmail = model.GameMaster
            };

            db.Campaigns.Add(campaign);

            var changes = await db.SaveChangesAsync();
            if (changes != 1)
            {
                return BadRequest();
            }

            return new CreatedResult($"/dnd35/campaigns/{campaign.CampaignId}", null);
        }
    }
}