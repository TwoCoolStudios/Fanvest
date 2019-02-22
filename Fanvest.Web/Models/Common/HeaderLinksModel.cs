using Fanvest.Web.Framework.Models;
namespace Fanvest.Web.Models.Common
{
    public partial class HeaderLinksModel : BaseModel
    {
        public bool IsRegistered { get; set; }
        public string UserName { get; set; }
        public decimal Balance { get; set; }
    }
}