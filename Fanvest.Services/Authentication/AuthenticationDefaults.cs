using Microsoft.AspNetCore.Http;
namespace Fanvest.Services.Authentication
{
    public static partial class AuthenticationDefaults
    {
        public static string AuthenticationScheme => "Authentication";
        public static string ClaimsIssuer => "Fanvest";
        public static PathString LoginPath => new PathString("/login");
        public static PathString LogoutPath => new PathString("/logout");
        public static PathString AccessDeniedPath => new PathString("/error");
        public static string ReturnUrlParameter => string.Empty;
    }
}