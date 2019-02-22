using Fanvest.Web.Framework.Models;
namespace Fanvest.Web.Models.Common
{
    public partial class SocialMediaModel : BaseModel
    {
        public string FacebookLink { get; set; }
        public string TwitterLink { get; set; }
        public string InstagramLink { get; set; }
    }
}