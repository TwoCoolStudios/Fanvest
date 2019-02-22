using System.ComponentModel.DataAnnotations;
using Fanvest.Web.Framework.Models;
namespace Fanvest.Web.Models.Users
{
    public partial class DepositModel : BaseModel
    {
        [Display(Name = "Amount")]
        [Required(ErrorMessage = "Amount is required.")]
        [DataType(DataType.Currency)]
        [Range(1, 1000, ErrorMessage = "Minimum amount is $1. Maximum amount is $1,000.")]
        public decimal Amount { get; set; }

        public bool SuccessfullySent { get; set; }
        public string Result { get; set; }
    }
}