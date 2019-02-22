using System.ComponentModel.DataAnnotations;
using Fanvest.Web.Framework.Models;
namespace Fanvest.Web.Models.Users
{
    public partial class ForgotPasswordModel : BaseModel
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email address is required.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string Result { get; set; }
    }
}