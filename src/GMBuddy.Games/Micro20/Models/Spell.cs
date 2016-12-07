using System;
using System.ComponentModel.DataAnnotations;
using GMBuddy.Games.Micro20.InputModels;

namespace GMBuddy.Games.Micro20.Models
{
    public class Spell
    {
        public Spell()
        {
            
        }

        public Spell(NewSpell model)
        {
            Name = model.Name;
            School = model.School;
            Level = model.Level;
        }
     
        [Key]
        public Guid SpellId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string School { get; set; }

        [Required, Range(3, 18)]
        public int Level { get; set; }
    }
}