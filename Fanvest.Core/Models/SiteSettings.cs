using Fanvest.Core.Models.Users;
namespace Fanvest.Core.Models
{
    public partial class SiteSettings
    {
        //site settings
        public string SiteName { get; set; } = "Fanvest";
        public string SiteUrl { get; set; } = "http://www.fanvest.io/";
        public string FacebookLink { get; set; } = "https://www.facebook.com/fanvestio";
        public string TwitterLink { get; set; } = "https://www.twitter.com/fanvestio";
        public string InstagramLink { get; set; } = "https://www.instagram.com/fanvestio";

        //user settings
        public bool UsernamesEnabled { get; set; } = true;
        public bool AllowUsernameChanges { get; set; } = false;
        public UserRegistrationType UserRegistrationType { get; set; } = UserRegistrationType.EmailValidation;
        public UserNameFormat UserNameFormat { get; set; } = UserNameFormat.ShowEmails;
        public PasswordFormat DefaultPasswordFormat { get; set; } = PasswordFormat.Hashed;
        public string HashedPasswordFormat { get; set; } = "SHA512";
        public int FailedPasswordAttemptsAllowed { get; set; } = 0;
        public int FailedPasswordLockOutMinutes { get; set; } = 30;
        public int DaysPasswordRecoveryLinkValid { get; set; } = 7;
        public bool SuffixDeletedUsers { get; set; } = true;

        //security settings
        public string EncryptionKey { get; set; } = CommonHelper.GenerateRandomDigitCode(16);

        //common settings
        public bool SubjectFieldOnContactUsPage { get; set; } = true;
    }
}