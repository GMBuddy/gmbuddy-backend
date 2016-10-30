using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GMBuddy.Games.Micro20.Models
{
    public class Micro20Character
    {
        [Range(3, 18)]
        public int Strength { get; set; }
    }
}
