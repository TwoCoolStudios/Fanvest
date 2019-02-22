using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Fanvest.Core.Models;
using Fanvest.Core.Models.Users;
using Fanvest.Services.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
namespace Fanvest.Services.Authentication
{
    public partial class CookieAuthenticationService : IAuthenticationService
    {
        #region Fields
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;
        private readonly SiteSettings _siteSettings;

        private User _currentUser;
        #endregion

        #region Ctors
        public CookieAuthenticationService(IHttpContextAccessor httpContextAccessor,
            IUserService userService, IOptions<SiteSettings> siteSettings)
        {
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            _siteSettings = siteSettings.Value;
        }
        #endregion

        #region Methods
        public virtual async Task SignIn(User user, bool isPersistent)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            var claims = new List<Claim>();
            if (!string.IsNullOrEmpty(user.Email))
                claims.Add(new Claim(ClaimTypes.Email, user.Email, ClaimValueTypes.Email,
                    AuthenticationDefaults.ClaimsIssuer));
            if (!string.IsNullOrEmpty(user.Username))
                claims.Add(new Claim(ClaimTypes.Name, user.Username, ClaimValueTypes.String,
                    AuthenticationDefaults.ClaimsIssuer));
            var userIdentity = new ClaimsIdentity(claims, AuthenticationDefaults.AuthenticationScheme);
            var userPrincipal = new ClaimsPrincipal(userIdentity);
            var authenticationProperties = new AuthenticationProperties
            {
                IsPersistent = isPersistent,
                IssuedUtc = DateTime.Now
            };
            await _httpContextAccessor.HttpContext.SignInAsync(AuthenticationDefaults.AuthenticationScheme,
                userPrincipal, authenticationProperties);
            _currentUser = user;
        }

        public virtual async Task SignOut()
        {
            _currentUser = null;
            await _httpContextAccessor.HttpContext.SignOutAsync
                (AuthenticationDefaults.AuthenticationScheme);
        }

        public virtual async Task<User> GetAuthenticatedUser()
        {
            if (_currentUser != null)
                return _currentUser;
            var result = await _httpContextAccessor.HttpContext.AuthenticateAsync
                (AuthenticationDefaults.AuthenticationScheme);
            if (!result.Succeeded)
                return null;
            User user = null;
            if (_siteSettings.UsernamesEnabled)
            {
                var usernameClaim = result.Principal.FindFirst(claim => claim.Type == ClaimTypes.Name
                    && claim.Issuer.Equals(AuthenticationDefaults.ClaimsIssuer,
                    StringComparison.InvariantCultureIgnoreCase));
                if (usernameClaim != null)
                    user = await _userService.GetUserByUsername(usernameClaim.Value);
            }
            else
            {
                var emailClaim = result.Principal.FindFirst(claim => claim.Type == ClaimTypes.Email
                    && claim.Issuer.Equals(AuthenticationDefaults.ClaimsIssuer,
                    StringComparison.InvariantCultureIgnoreCase));
                if (emailClaim != null)
                    user = await _userService.GetUserByEmail(emailClaim.Value);
            }
            if (user == null || user.Deleted || !user.Active || !await _userService.IsInRole(user,
                    UserDefaults.Registered))
                return null;
            _currentUser = user;
            return _currentUser;
        }
        #endregion
    }
}