using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GMBuddyData.ViewModels
{
    #region interfaces

    /// <summary>
    /// Represents the data needed for a campaign
    /// </summary>
    public interface ICampaignView
    {
        Guid CampaignId { get; set; }
        string Name { get; set; }
        string GmEmail { get; set; }
        IEnumerable<IBasicCharacterView> Characters { get; set; }
    }

    /// <summary>
    /// Represents the data needed to create a new campaign
    /// </summary>
    public interface ICampaignCreationView
    {
        string Name { get; set; }
        string GmEmail { get; set; }
    }

    /// <summary>
    /// Represents the data for a list of campaigns
    /// </summary>
    public interface ICampaignListView
    {
        IEnumerable<ICampaignView> Campaigns { get; set; }
    }

    #endregion

    #region implementations

    /// <summary>
    /// Backing class for ICampaignListView
    /// </summary>
    public class CampaignListView : ICampaignListView
    {
        public IEnumerable<ICampaignView> Campaigns { get; set; }
    }

    #endregion

}
