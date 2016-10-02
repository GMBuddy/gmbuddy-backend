using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GMBuddyData.Models.DND35;

namespace GMBuddyData.Data.DND35
{
    public static class DataInitializer
    {
        public static void Init(GameContext context)
        {
            if (context.Campaigns.Any())
            {
                return;
            }

            var campaign = new Campaign
            {
                Name = "My Awesome Campaign"
            };

            var character = new Character
            {
                UserEmail = "testing@user.com",
                Name = "Generic Fantasy Character Name",
                Bio = "Was a program once, until he took an error to the runtime"
            };

            context.Campaigns.Add(campaign);
            context.Characters.Add(character);
            int changes = context.SaveChanges();

            var campaignCharacter = new CampaignCharacter
            {
                CampaignCharacterId = new Guid(),
                CharacterId = character.CharacterId,
                CampaignId = campaign.CampaignId,
                IsGameMaster = true
            };

            context.CampaignCharacters.Add(campaignCharacter);
            changes += context.SaveChanges();

            var sheet = new Sheet
            {
                CampaignCharacterId = campaignCharacter.CampaignCharacterId,
                Charisma = 5,
                Endurance = 5,
                Stength = 5
            };
            
            context.Sheets.Add(sheet);
            changes += context.SaveChanges();

            if (changes != 4)
            {
                Console.Error.WriteLine("Did not save initialization data correctly");
                Environment.Exit(1);
            }
        }
    }
}
