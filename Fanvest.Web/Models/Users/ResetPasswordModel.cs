﻿using System.ComponentModel.DataAnnotations;
using Fanvest.Web.Framework.Models;
namespace Fanvest.Web.Models.Users
{
    public partial class ResetPasswordModel : BaseModel
    {
        [Display(Name = "New Password")]
        [Required(ErrorMessage = "New password is required.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Display(Name = "Confirm New Password")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Entered passwords do not match.")]
        public string ConfirmNewPassword { get; set; }

        public bool ChangingPasswordsDisabled { get; set; }
        public string Result { get; set; }
    }
}