using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fanvest.Core.Models.Campaigns;
namespace Fanvest.Services.Campaigns
{
    public partial interface ICampaignService
    {
        Task DeleteCampaign(Campaign campaign);

        Task<IList<Campaign>> GetAllCampaigns();

        Task<Campaign> GetCampaignById(int campaignId);

        Task<Campaign> GetCampaignBySlug(string campaignSlug);

        Task InsertCampaign(Campaign campaign);

        Task UpdateCampaign(Campaign campaign);

        bool IsCampaignAvailable(Campaign campaign,
            DateTime? dateTime = null);

        Task<IList<Token>> GetAllTokens(int campaignId = 0,
            int userId = 0);
    }
}