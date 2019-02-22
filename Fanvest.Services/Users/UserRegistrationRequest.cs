using Fanvest.Core.Models.Users;
namespace Fanvest.Services.Users
{
    public partial class UserRegistrationRequest
    {
        public UserRegistrationRequest(User user, string email,
            string username, string password, PasswordFormat passwordFormat,
            bool approved = true)
        {
            User = user;
            Email = email;
            Username = username;
            Password = password;
            PasswordFormat = passwordFormat;
            Approved = approved;
        }

        public User User { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public PasswordFormat PasswordFormat { get; set; }
        public bool Approved { get; set; }
    }
}