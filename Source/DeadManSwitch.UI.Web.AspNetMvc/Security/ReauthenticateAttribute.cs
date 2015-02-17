using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace DeadManSwitch.UI.Web.AspNetMvc
{
    /// <summary>
    /// Remember that the user has authenticated
    /// </summary>
    public class ReauthenticateAttribute : AuthorizeAttribute
    {
//        private readonly int ReauthenticationFrequencyInMinutes;
//
//        public ReauthenticateAttribute(int frequencyInMinutes = 30)
//        {
//            ReauthenticationFrequencyInMinutes = frequencyInMinutes;
//        }
//
//        protected override bool AuthorizeCore(HttpContextBase httpContext)
//        {
//            bool isAuthorized = base.AuthorizeCore(httpContext);
//            if (isAuthorized)
//            {
//                var hasReauthenticated = Reauthenticator.HasUserReauthenticatedRecently(httpContext.User.Identity.GetUserName(), httpContext.Request.Cookies);
//                if (hasReauthenticated)
//                {
//                    Reauthenticator.SlideReauthenticatedExpiration(httpContext, ReauthenticationFrequencyInMinutes);
//                }
//
//                isAuthorized = hasReauthenticated;
//            }
//
//            return isAuthorized;
//        }

    }
}