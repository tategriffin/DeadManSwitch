using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Principal;
using System.Web;
using Microsoft.AspNet.Identity;

namespace DeadManSwitch.UI.Web.AspNetMvc
{
    public static class Reauthenticator
    {
        private const string LastLoginCookieKey = "LastAddress";

        /// <remarks>
        /// When initially logging in, httpContext.User.Identity.GetUserName()
        /// returns an empty string, so pass in the userName, rather than
        /// relying on it being populated in httpContext.
        /// </remarks>>
        public static void SlideReauthenticatedExpiration(HttpContextBase httpContext, string userName, int expirationInMinutes)
        {
            var cookie = Reauthenticator.BuildReauthenticationCookie(userName, DateTime.UtcNow.AddMinutes(expirationInMinutes));
            if (cookie != null)
            {
                httpContext.Response.SetCookie(cookie);
            }
        }

        public static bool HasUserReauthenticatedRecently(string userName, HttpCookieCollection cookieCollection)
        {
            var cookie = FindReauthenticationCookie(userName, cookieCollection);

            return (cookie != null);
        }

        private static HttpCookie BuildReauthenticationCookie(string userName, DateTime expiration)
        {
            HttpCookie cookie = null;

            string value = EncryptCookieValue(userName);
            if (!string.IsNullOrWhiteSpace(value))
            {
                cookie = new HttpCookie(LastLoginCookieKey, value) { Expires = expiration };
            }

            return cookie;
        }

        private static HttpCookie FindReauthenticationCookie(string userName, HttpCookieCollection cookieCollection)
        {
            HttpCookie cookie = null;

            var reauthCookie = cookieCollection[LastLoginCookieKey];
            if (reauthCookie != null)
            {
                string value = DecryptCookieValue(reauthCookie.Value, userName);
                if (!string.IsNullOrWhiteSpace(value))
                {
                    cookie = reauthCookie;
                }
            }

            return cookie;
        }

        private static string EncryptCookieValue(string userName)
        {
            string purpose = BuildCookiePurpose(userName);
            string cookieValue = userName.Protect(purpose);

            return cookieValue;
        }

        private static string DecryptCookieValue(string encryptedValue, string userName)
        {
            string purpose = BuildCookiePurpose(userName);
            string cookieValue = encryptedValue.Unprotect(purpose);

            return cookieValue;
        }

        private static string BuildCookiePurpose(string userName)
        {
            return string.Format("Reauthenticate {0}", userName);
        }

    }
}