using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GMBuddyData.Models.DND35
{
    //public abstract class CharacterAttributes
    //{
    //    public Guid CharacterAttributesId { get; set; }
    //    public int Stength { get; set; }
    //    public int Charisma { get; set; }
    //    public int Endurance { get; set; }

    //    //public Guid CampaignCharacterId { get; set; }
    //    //public CampaignCharacter CampaignCharacter { get; set; }
    //}

    public interface ICharacterAttributes
    {
        int Strength { get; set; }
        int Charisma { get; set; }
        int Endurance { get; set; }
    }
}
