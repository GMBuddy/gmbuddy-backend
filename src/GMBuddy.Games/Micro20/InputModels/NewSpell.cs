using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GMBuddy.Games.Micro20.Models;

namespace GMBuddy.Games.Micro20.InputModels
{
    public class NewSpell
    {
        [Required]
        public string Name { get; set; }

        public string School { get; set; }

        public string Level { get; set; }
    }
}
