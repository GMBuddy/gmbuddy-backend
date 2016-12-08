using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GMBuddy.Exceptions;
using GMBuddy.Games.Micro20.Data;
using GMBuddy.Games.Micro20.InputModels;
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
        /// Modifies a campaign to fit the updates given in CampaignModification
        /// </summary>
        /// <param name="campaignId"></param>
        /// <param name="userId"></param>
        /// <param name="model"></param>
        /// <exception cref="DataNotFoundException"></exception>
        /// <exception cref="DataNotCreatedException"></exception>
        /// <returns></returns>
        public async Task<CampaignView> ModifyCampaign(Guid campaignId, string userId, CampaignModification model)
        {
            Campaign campaign;

            using (var db = new DatabaseContext(options))
            {
                campaign = await db.Campaigns
                    .Include(c => c.Characters)
                    .SingleOrDefaultAsync(c => c.CampaignId == campaignId);

                if (campaign == null)
                {
                    throw new DataNotFoundException($"Could not find campaign {campaignId}");
                }

                // only update the name if it is set, valid, coming from the GM, and different than the current name
                if (!string.IsNullOrWhiteSpace(model.Name) 
                    && userId == campaign.GmUserId 
                    && campaign.Name != model.Name)
                {
                    campaign.Name = model.Name;
                    int changes = await db.SaveChangesAsync();
                    if (changes != 1)
                    {
                        throw new DataNotCreatedException("Could not update campaign name");
                    }
                }
            }
            
            if (campaign.GmUserId == userId)
            {
                await ModifyCampaignAsGm(campaignId, model);
            }
            else
            {
                await ModifyCampaignBasic(campaignId, userId, model);
            }

            // reload the updated campaign, the existing reference isnt updated
            using (var db = new DatabaseContext(options))
            {
                campaign = await db.Campaigns
                    .Include(c => c.Characters)
                    .SingleAsync(c => c.CampaignId == campaignId);

                return new CampaignView(campaign);
            }
        }

        /// <summary>
        /// Adds all of the AddCharacters to the given campaign and removes all of the RemoveCharacters from the given 
        /// campaign if they were originally part of the given campaign
        /// </summary>
        /// <param name="campaignId"></param>
        /// <param name="model"></param>
        /// <exception cref="DataNotFoundException">If one of the CharacterIds does not point to a character in the database</exception>
        /// <exception cref="DataNotCreatedException">
        /// If one or more of the changes was not persisted to the database.
        /// If this exception is thrown, the campaign may be in an invalid state.
        /// </exception>
        /// <returns></returns>
        private async Task ModifyCampaignAsGm(Guid campaignId, CampaignModification model)
        {
            using (var db = new DatabaseContext(options))
            {
                int should = 0;

                if (model.AddCharacters != null)
                {
                    foreach (var cid in model.AddCharacters)
                    {
                        var character = await db.Characters.SingleOrDefaultAsync(c => c.CharacterId == cid);

                        if (character == null)
                        {
                            throw new DataNotFoundException($"Could not find character {cid}");
                        }

                        // only update if we need to
                        if (character.CampaignId != campaignId)
                        {
                            character.CampaignId = campaignId;
                            should += 1;
                        }
                    }
                }

                if (model.RemoveCharacters != null)
                {
                    foreach (var cid in model.RemoveCharacters)
                    {
                        var character = await db.Characters.SingleOrDefaultAsync(c => c.CharacterId == cid);

                        if (character == null)
                        {
                            throw new DataNotFoundException($"Could not find character {cid}");
                        }

                        // only set the character's campaign to null if it was originally the id of the campaign being modified
                        if (character.CampaignId == campaignId)
                        {
                            character.CampaignId = null;
                            should += 1;
                        }
                    }
                }

                int actual = await db.SaveChangesAsync();
                if (actual != should)
                {
                    throw new DataNotCreatedException($"Error saving one or more of the updates: Expected {should}, recieved {actual}");
                }
            }
        }

        /// <summary>
        /// Adds all of the AddCharacters that belong to the user to the given campaign
        /// and removes all of the RemoveCharacters that belong to the user and are in the given campaign from that campaign
        /// </summary>
        /// <param name="campaignId"></param>
        /// <param name="userId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task ModifyCampaignBasic(Guid campaignId, string userId, CampaignModification model)
        {
            // nothing to do, dont bother going further
            if ((model.AddCharacters == null || model.AddCharacters.Count == 0) &&
                (model.RemoveCharacters == null || model.RemoveCharacters.Count == 0))
            {
                return;
            }

            using (var db = new DatabaseContext(options))
            {
                var userCharacters = await db.Characters
                    .Where(c => c.UserId == userId)
                    .ToListAsync();
                int should = 0;

                if (model.AddCharacters != null)
                {
                    // only the characters whose ids are in the list to add to the given campaign
                    var toAdd = userCharacters.Where(c => model.AddCharacters.Any(id => id == c.CharacterId));
                    foreach (var character in toAdd)
                    {
                        character.CampaignId = campaignId;
                        should += 1;
                    }
                }

                if (model.RemoveCharacters != null)
                {
                    // only the characters whose ids are in the list to remove from the given campaign and are currently in the given campaign
                    var toRemove = userCharacters.Where(c => c.CampaignId == campaignId && model.RemoveCharacters.Any(id => id == c.CharacterId));
                    foreach (var character in toRemove)
                    {
                        character.CampaignId = null;
                        should += 1;
                    }
                }

                int actual = await db.SaveChangesAsync();
                if (should != actual)
                {
                    throw new DataNotCreatedException($"Error saving one or more of the updates: Expected {should}, recieved {actual}");
                }
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
