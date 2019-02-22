namespace Fanvest.Core
{
    public partial interface IWebHelper
    {
        bool IsCurrentConnectionSecured();

        string ModifyQueryString(string url, string key, params string[] values);

        string CurrentRequestProtocol { get; }
    }
}