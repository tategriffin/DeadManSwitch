using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace DeadManSwitch.UI.Web.AspNetMvc
{
    /// <summary>
    /// Force user to reauthenticate if they haven't recently.
    /// Useful when performing sensitive operations, such as
    /// changing the email address associated with an account.
    /// </summary>
    public class MustReauthenticateAttribute : AuthorizeAttribute
    {
        private readonly int ReauthenticationExpiresInMinutes;

        public MustReauthenticateAttribute(int expiresInMinutes)
        {
            ReauthenticationExpiresInMinutes = expiresInMinutes;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool isAuthorized = base.AuthorizeCore(httpContext);
            if (isAuthorized)
            {
                var hasReauthenticated = Reauthenticator.HasUserReauthenticatedRecently(httpContext.User.Identity.GetUserName(), httpContext.Request.Cookies);
                if (hasReauthenticated)
                {
                    Reauthenticator.SlideReauthenticatedExpiration(httpContext, ReauthenticationExpiresInMinutes);
                }

                isAuthorized = hasReauthenticated;
            }

            return isAuthorized;
        }

    }
}