using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DeadManSwitch.UI.Web.AspNet.Account
{
    public partial class Login : DMSPage
    {
        private LoginPresenter Presenter;

        public Login()
            : base(allowUnauthenticatedUser:true) { }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Presenter = new LoginPresenter(ClientIpAddress);

            RegisterHyperLink.NavigateUrl = "Register";
            //OpenAuthLogin.ReturnUrl = Request.QueryString["ReturnUrl"];

            var returnUrl = HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
            if (!String.IsNullOrEmpty(returnUrl))
            {
                RegisterHyperLink.NavigateUrl += "?ReturnUrl=" + returnUrl;
            }

            this.ReturnToUrl = Request.QueryString["ReturnUrl"];
        }

        private string ReturnToUrlValue;
        private string ReturnToUrl
        {
            get { return ReturnToUrlValue; }
            set
            {
                this.ReturnToUrlValue = (string.IsNullOrWhiteSpace(value) ? "~/" : value);
            }
        }

        protected void Button_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button == null) return;

            switch (button.CommandName)
            {
                case "Login":
                    AuthenticateUser();
                    break;
            }

        }

        private void AuthenticateUser()
        {
            string failedLoginMsg;
            if (this.Presenter.TryLogin(UserName.Text, Password.Text, out failedLoginMsg))
            {
                HttpCookie userCookie = BuildUserCookie(UserName.Text, RememberMe.Checked);
                Response.Cookies.Add(userCookie);

                this.UserReauthenticated();

                Response.Redirect(this.ReturnToUrl);
            }
            else
            {
                this.FailureText.Text = failedLoginMsg;
            }
        }

        private HttpCookie BuildUserCookie(string userName, bool persistentCookie)
        {
            int timeoutMinutes = (int)FormsAuthentication.Timeout.TotalMinutes;
            FormsAuthenticationTicket authTicket =
                new FormsAuthenticationTicket(1, userName, DateTime.Now, DateTime.Now.AddMinutes(timeoutMinutes), persistentCookie, string.Empty);
            
            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            HttpCookie userCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            if (persistentCookie)
            {
                userCookie.Expires = authTicket.Expiration;
            }
            userCookie.Path = FormsAuthentication.FormsCookiePath;

            return userCookie;
        }

    }
}