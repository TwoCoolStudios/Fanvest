using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanvest.Core.Models.Campaigns;
using Fanvest.Data;
using Microsoft.EntityFrameworkCore;
namespace Fanvest.Services.Campaigns
{
    public partial class CampaignService : ICampaignService
    {
        #region Fields
        private readonly CustomDbContext _context;
        #endregion

        #region Ctors
        public CampaignService(CustomDbContext context)
            => _context = context;
        #endregion

        #region Methods
        public virtual async Task DeleteCampaign(Campaign campaign)
        {
            if (campaign == null)
                throw new ArgumentNullException(nameof(campaign));
            campaign.Deleted = true;
            await UpdateCampaign(campaign);
        }

        public virtual async Task<IList<Campaign>> GetAllCampaigns()
            => await _context.Set<Campaign>().Where(c => !c.Deleted)
            .OrderByDescending(c => c.CreatedOn).Select(c => c)
            .ToListAsync();

        public virtual async Task<Campaign> GetCampaignById(int campaignId)
        {
            if (campaignId == 0)
                return null;
            return await _context.Set<Campaign>().FindAsync(campaignId);
        }

        public virtual async Task<Campaign> GetCampaignBySlug(string campaignSlug)
        {
            if (string.IsNullOrWhiteSpace(campaignSlug))
                return null;
            var query = from c in _context.Set<Campaign>()
                        where c.Slug == campaignSlug
                        orderby c.Id
                        select c;
            return await query.FirstOrDefaultAsync();
        }

        public virtual async Task InsertCampaign(Campaign campaign)
        {
            if (campaign == null)
                throw new ArgumentNullException(nameof(campaign));
            _context.Set<Campaign>().Add(campaign);
            await _context.SaveChangesAsync();
        }

        public virtual async Task UpdateCampaign(Campaign campaign)
        {
            if (campaign == null)
                throw new ArgumentNullException(nameof(campaign));
            _context.Set<Campaign>().Update(campaign);
            await _context.SaveChangesAsync();
        }

        public virtual bool IsCampaignAvailable(Campaign campaign,
            DateTime? dateTime = null)
        {
            if (campaign == null)
                throw new ArgumentNullException(nameof(campaign));
            if (campaign.StartDate.HasValue && campaign.StartDate.Value > dateTime)
                return false;
            if (campaign.EndDate.HasValue && campaign.EndDate.Value < dateTime)
                return false;
            return true;
        }

        public virtual async Task<IList<Token>> GetAllTokens(int campaignId = 0,
            int userId = 0)
        {
            IQueryable<Token> query = _context.Set<Token>();
            if (campaignId > 0)
                query = query.Where(t => t.CampaignId == campaignId);
            if (userId > 0)
                query = query.Where(t => t.UserId == userId);
            query = query.OrderByDescending(t => t.CreatedOn);
            return await query.ToListAsync();
        }
        #endregion
    }
}