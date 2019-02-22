using System;
using System.Collections.Generic;
using Fanvest.Web.Framework.Models;
namespace Fanvest.Web.Models.Users
{
    public partial class UserTokensModel : BaseModel
    {
        public UserTokensModel() => Tokens = new List<TokenDetailsModel>();

        public IList<TokenDetailsModel> Tokens { get; set; }

        public partial class TokenDetailsModel : BaseModel
        {
            public int CampaignId { get; set; }
            public string CampaignName { get; set; }
            public string CampaignSlug { get; set; }
            public string CampaignPrice { get; set; }
            public int Tokens { get; set; }
            public DateTime CreatedOn { get; set; }
        }
    }
}