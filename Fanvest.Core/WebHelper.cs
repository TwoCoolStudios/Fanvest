using System;
using System.Linq;
using Fanvest.Core.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.WebUtilities;
namespace Fanvest.Core
{
    public partial class WebHelper : IWebHelper
    {
        #region Fields 
        private readonly HostingConfig _hostingConfig;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Ctors
        public WebHelper(HostingConfig hostingConfig,
            IHttpContextAccessor httpContextAccessor)
        {
            _hostingConfig = hostingConfig;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region Utilities
        protected virtual bool IsRequestAvailable()
        {
            if (_httpContextAccessor?.HttpContext == null)
                return false;
            try
            {
                if (_httpContextAccessor.HttpContext.Request == null)
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        #endregion

        #region Methods
        public virtual bool IsCurrentConnectionSecured()
        {
            if (!IsRequestAvailable())
                return false;
            if (_hostingConfig.UseHttpClusterHttps)
                return _httpContextAccessor.HttpContext.Request.Headers[HttpDefaults.HttpClusterHttpsHeader]
                    .ToString().Equals("on", StringComparison.OrdinalIgnoreCase);
            if (_hostingConfig.UseHttpXForwardedProto)
                return _httpContextAccessor.HttpContext.Request.Headers[HttpDefaults.HttpXForwardedProtoHeader]
                    .ToString().Equals("https", StringComparison.OrdinalIgnoreCase);
            return _httpContextAccessor.HttpContext.Request.IsHttps;
        }

        public virtual string ModifyQueryString(string url, string key, params string[] values)
        {
            if (string.IsNullOrEmpty(url))
                return string.Empty;
            if (string.IsNullOrEmpty(key))
                return url;
            var uri = new Uri(url);
            var queryParameters = QueryHelpers.ParseQuery(uri.Query);
            queryParameters[key] = string.Join(",", values);
            var queryBuilder = new QueryBuilder(queryParameters
                .ToDictionary(parameter => parameter.Key, parameter => parameter.Value.ToString()));
            url = $"{uri.GetLeftPart(UriPartial.Path)}{queryBuilder.ToQueryString()}{uri.Fragment}";
            return url;
        }

        public virtual string CurrentRequestProtocol => IsCurrentConnectionSecured()
            ? Uri.UriSchemeHttps : Uri.UriSchemeHttp;
        #endregion
    }
}