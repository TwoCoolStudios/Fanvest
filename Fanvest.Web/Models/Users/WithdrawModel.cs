using System.ComponentModel.DataAnnotations;
using Fanvest.Web.Framework.Models;
namespace Fanvest.Web.Models.Users
{
    public partial class WithdrawModel : BaseModel
    {
        [Display(Name = "Amount")]
        [Required(ErrorMessage = "Amount is required.")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        public bool SuccessfullySent { get; set; }
        public string Result { get; set; }
    }
}