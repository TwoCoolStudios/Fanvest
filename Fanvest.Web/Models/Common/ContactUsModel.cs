using System.ComponentModel.DataAnnotations;
using Fanvest.Web.Framework.Models;
namespace Fanvest.Web.Models.Common
{
    public partial class ContactUsModel : BaseModel
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email address is required.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        public bool SubjectEnabled { get; set; }

        [Display(Name = "Subject")]
        public string Subject { get; set; }

        [Display(Name = "Enquiry")]
        [Required(ErrorMessage = "Enquiry is required.")]
        public string Enquiry { get; set; }

        public bool SuccessfullySent { get; set; }
        public string Result { get; set; }
    }
}