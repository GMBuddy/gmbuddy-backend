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
                Name = "My Awesome Campaign",
                GmEmail = "testing@user.com"
            };

            var character = new Character
            {
                UserEmail = "testing@user.com",
                Name = "Generic Fantasy Character Name",
                Bio = "Was a program once, until he took an error to the runtime",
                Age = 50,
                Alignment = "Chaotic Neutral",
                Charisma = 5,
                Constitution = 5,
                Strength = 5,
                Wisdom = 5,
                Size = "Large",
                Class = "Paladin",
                Deity = "Yogg Saron",
                Dexterity = 5,
                Eyes = "Brown",
                Gender = "Foxkin",
                Height = 70,
                Intelligence = 5,
                Skin = "Red",
                Weight = 200
            };

            context.Campaigns.Add(campaign);
            context.Characters.Add(character);
            int changes = context.SaveChanges();

            var campaignCharacter = new CampaignCharacter
            {
                CampaignCharacterId = new Guid(),
                CharacterId = character.CharacterId,
                CampaignId = campaign.CampaignId
            };

            context.CampaignCharacters.Add(campaignCharacter);
            changes += context.SaveChanges();

            if (changes != 3)
            {
                Console.Error.WriteLine("Did not save initialization data correctly");
                Environment.Exit(1);
            }
        }
    }
}
