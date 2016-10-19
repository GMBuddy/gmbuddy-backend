using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GMBuddyData.Models.DND5
{
    public class Spell
    {
        public Guid SpellId { get; set; }
        [Required]
        public string SpellName { get; set; }
        [Required]
        public string SpellDescription { get; set; }

        public Guid SheetId { get; set; }
        public Sheet Sheet { get; set; }
        
        public int CharacterID { get; set; }

        public bool HasVisualComponent { get; set; }
        public bool HasSomaticComponent { get; set; }
        public bool HasMaterialComponent { get; set; }
        public bool HasCostComponent { get; set; }

        public bool IsConcentration { get; set; }
        public bool IsRitual { get; set; }

        public string CastingTime { get; set; }

        public int Level { get; set; } //Can only be 0-9, with 0 being Cantrip
        
        //TODO: somehow list which Classes have access to this spell.
    }

    public enum SpellSchool
    {
        Abjuration,
        Conjuration,
        Divination,
        Enchantment,
        Evocation,
        Illusion,
        Necromancy,
        Transmutation
    }

}
