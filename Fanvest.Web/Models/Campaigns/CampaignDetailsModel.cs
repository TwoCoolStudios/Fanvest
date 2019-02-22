using System;
using Fanvest.Web.Framework.Models;
namespace Fanvest.Web.Models.Campaigns
{
    public partial class CampaignDetailsModel : BaseEntityModel
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public decimal Price { get; set; }
        public int TokenCount { get; set; }
        public bool AllowBuying { get; set; }
        public bool AllowSelling { get; set; }
        public DateTime? EndDate { get; set; }
    }
}