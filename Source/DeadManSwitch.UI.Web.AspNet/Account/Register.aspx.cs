using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Membership.OpenAuth;
using Microsoft.Practices.Unity;

namespace DeadManSwitch.UI.Web.AspNet.Account
{
    public partial class Register : DMSPage
    {
        private UserRegistrationPresenter Presenter { get; set; }

        public Register()
            : base(allowUnauthenticatedUser: true, requireReauthentication: false) { }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.ReturnToUrl = Request.QueryString["ReturnUrl"];

            this.Presenter = new UserRegistrationPresenter(this.CurrentUser);
        }

        private string ReturnToUrlValue;
        private string ReturnToUrl
        {
            get { return ReturnToUrlValue; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    this.ReturnToUrlValue = "~/";
                }
                else
                {
                    this.ReturnToUrlValue = value;
                }
            }
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            this.RegistrationSection.Visible = this.Presenter.Model.IsRegistrationOpen;

            if (this.Presenter.Model.IsRegistrationOpen == false)
            {
                this.ErrorMessage.Text = "Registration is currently closed.";
            }
        }

        protected void Button_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button == null) return;

            switch (button.CommandName)
            {
                case "Register":
                    RegisterNewUser();
                    break;
            }

        }

        private void RegisterNewUser()
        {
            PopulateModelFromUI();

            List<string> failureMsgs = this.Presenter.RegisterNewUser();
            if (failureMsgs.Count == 0)
            {
                bool persistentCookie = this.RememberMe.Checked;
                //TODO: Use OpenAuth instead
                FormsAuthentication.SetAuthCookie(UserName.Text, persistentCookie);

                this.UserReauthenticated();
                Response.Redirect(this.ReturnToUrl);
            }
            else
            {
                this.ErrorMessage.Text = string.Join("<br />", failureMsgs);
            }
        }

        private void PopulateModelFromUI()
        {
            this.Presenter.Model.UserName = this.UserName.Text;
            this.Presenter.Model.Email = this.Email.Text;
            this.Presenter.Model.FirstName = this.FirstName.Text;
            this.Presenter.Model.LastName = this.LastName.Text;
            this.Presenter.Model.Password = this.Password.Text;
            this.Presenter.Model.PasswordConfirmation = this.ConfirmPassword.Text;
        }

    }
}