using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeadManSwitch.UI.Web.AspNet
{
    public class DMSPage : System.Web.UI.Page
    {
        protected const string SessionKeyReauthenticatedDuringSession = "ReauthenticatedDuringSession";

        private bool AllowUnauthenticatedUser;
        private bool RequireReauthentication;

        public CurrentUser CurrentUser { get; private set; }

        protected DMSPage(bool allowUnauthenticatedUser = false, bool requireReauthentication = false)
        {
            this.AllowUnauthenticatedUser = allowUnauthenticatedUser;
            this.RequireReauthentication = requireReauthentication;
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);

            this.CurrentUser = GetCurrentUser();
            this.RedirectToLoginIfRequired();
        }

        private void RedirectToLoginIfRequired()
        {
            if (this.AllowUnauthenticatedUser == false)
            {
                if (this.CurrentUser.IsAuthenticated == false)
                {
                    HttpContext.Current.Response.Redirect(BuildLoginUrlWithRedirect());
                }
                else if (this.RequireReauthentication == true)
                {
                    object hasReauthenticatedDuringSession = HttpContext.Current.Session[SessionKeyReauthenticatedDuringSession];
                    if (hasReauthenticatedDuringSession == null)
                    {
                        HttpContext.Current.Response.Redirect(BuildLoginUrlWithRedirect());
                    }
                }
            }
        }

        private string BuildLoginUrlWithRedirect()
        {
            System.Text.StringBuilder redirectUrl = new System.Text.StringBuilder();

            redirectUrl.Append("~/Account/Login");

            redirectUrl.Append("?");
            redirectUrl.Append("ReturnUrl=").Append(HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath);

            return redirectUrl.ToString();
        }

        protected void UserReauthenticated()
        {
            HttpContext.Current.Session[SessionKeyReauthenticatedDuringSession] = true;
        }

        protected static CurrentUser GetCurrentUser()
        {
            return GetCurrentUserFromContext(HttpContext.Current);
        }

        private static CurrentUser GetCurrentUserFromContext(HttpContext context)
        {
            CurrentUser user =
                new CurrentUser(context.User.Identity.Name, context.User.Identity.IsAuthenticated, ClientIpAddress);

            return user;
        }

        protected static string ClientIpAddress 
        {
            get { return HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]; }
        }

    }
}