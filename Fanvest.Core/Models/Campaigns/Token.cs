using System;
using Fanvest.Core.Models.Users;
namespace Fanvest.Core.Models.Campaigns
{
    public partial class Token : BaseEntity
    {
        public int CampaignId { get; set; }
        public int UserId { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
        public bool Bought { get; set; }
        public bool Sold { get; set; }
        public DateTime CreatedOn { get; set; }

        public virtual User User { get; set; }
        public virtual Campaign Campaign { get; set; }
    }
}