using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GMBuddyData.Models.DND35
{
    public abstract class CharacterAttributes
    {
        public Guid CharacterAttributesId { get; set; }
        public uint Stength { get; set; }
        public uint Charisma { get; set; }
        public uint Endurance { get; set; }
    }
}
