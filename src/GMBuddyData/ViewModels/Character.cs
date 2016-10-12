using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GMBuddyData.Models.DND35;

namespace GMBuddyData.ViewModels
{
    public interface IBasicCharacterView : ICharacterAbilities
    {
        Guid CharacterId { get; set; }
        string UserEmail { get; set; }
        string Name { get; set; }
        string Bio { get; set; }
        string Class { get; set; }
        string Deity { get; set; }
        string Alignment { get; set; }
        string Gender { get; set; }
        int Age { get; set; }
        int Height { get; set; }
        int Weight { get; set; }
        string Size { get; set; }
        string Eyes { get; set; }
        string Skin { get; set; }
    }

    public interface ICharacterView : IBasicCharacterView
    {
        ICollection<Item> Items { get; set; }
    }
}
