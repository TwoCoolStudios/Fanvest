using System;
namespace Fanvest.Core.Models.Users
{
    public partial class UserPassword : BaseEntity
    {
        public UserPassword() => PasswordFormat = PasswordFormat.Clear;

        public int UserId { get; set; }
        public int PasswordFormatId { get; set; }
        public string Password { get; set; }
        public string SaltKey { get; set; }
        public DateTime CreatedOn { get; set; }

        public virtual User User { get; set; }

        public PasswordFormat PasswordFormat
        {
            get => (PasswordFormat)PasswordFormatId;
            set => PasswordFormatId = (int)value;
        }
    }
}