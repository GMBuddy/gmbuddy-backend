using System;
using System.Collections.Generic;

namespace GMBuddy.Games.Micro20.InputModels
{
    public class CampaignModification
    {
        public string Name { get; set; }
        
        public List<Guid> AddCharacters { get; set; } 

        public List<Guid> RemoveCharacters { get; set; }
    }
}
