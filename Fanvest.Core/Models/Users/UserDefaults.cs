namespace Fanvest.Core.Models.Users
{
    public static partial class UserDefaults
    {
        public static string Administrators => "Administrators";
        public static string Registered => "Registered";
        public static string Visitors => "Visitors";
        public static string Moderators => "Moderators";
        public static int PasswordSaltKeySize => 5;
    }
}