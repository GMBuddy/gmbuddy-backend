using System;
using System.ComponentModel.DataAnnotations;

namespace GMBuddy.Games.Micro20.InputModels
{
    public class SpellModification
    {
        [Required]
        public Guid? SpellId { get; set; }
        
        public string Name { get; set; }

        public string School { get; set; }

        public string Level { get; set; }
    }
}