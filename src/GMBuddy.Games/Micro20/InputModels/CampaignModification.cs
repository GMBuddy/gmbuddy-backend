using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GMBuddy.Games.Micro20.InputModels
{
    public class CampaignModification
    {
        public string Name { get; set; }
        
        public List<Guid> AddCharacters { get; set; } 

        public List<Guid> RemoveCharacters { get; set; }
    }
}
