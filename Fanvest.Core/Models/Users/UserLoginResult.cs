namespace Fanvest.Core.Models.Users
{
    public enum UserLoginResult
    {
        Successful = 1,
        UserDoesntExist = 2,
        WrongPassword = 3,
        NotActive = 4,
        NotRegistered = 5,
        LockedOut = 6,
        Deleted = 7
    }
}