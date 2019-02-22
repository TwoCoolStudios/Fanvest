namespace Fanvest.Core.Configuration
{
    public static partial class HttpDefaults
    {
        public static string HttpClusterHttpsHeader => "HTTP_CLUSTER_HTTPS";
        public static string HttpXForwardedProtoHeader => "X-Forwarded-Proto";
        public static string Prefix => ".Fanvest";
        public static string UserCookie => ".User";
        public static string AntiforgeryCookie => ".Antiforgery";
        public static string SessionCookie => ".Session";
        public static string AuthenticationCookie => ".Authentication";
    }
}