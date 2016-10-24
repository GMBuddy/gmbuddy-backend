using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GMBuddy.Games.Dnd35.Models
{
    public class Dnd35Item
    {
        //how do I set attributes as key/required?
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public Dictionary<string, string> Stats { get; set; }

        //TODO: Implement the actual 3.5 stats
    }
}
