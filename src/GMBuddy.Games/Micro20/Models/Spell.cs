using System;
using System.ComponentModel.DataAnnotations;

namespace GMBuddy.Games.Micro20.Models
{
    public class Spell
    {
        [Key]
        public Guid SpellID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string School { get; set; }

        [Required, Range(3, 18)]
        public int Level { get; set; }
    }
}