using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DeadManSwitch.UI.Web.AspNet.Account
{
    public partial class ChangePassword : DMSPage
    {
        private ChangePasswordPresenter Presenter;

        public ChangePassword()
            : base(allowUnauthenticatedUser: false, requireReauthentication: true) { }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Presenter = new ChangePasswordPresenter(this.CurrentUser);
        }

        protected void Button_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button == null) return;

            switch (button.CommandName)
            {
                case "ChangePassword":
                    ChangeUserPassword();
                    break;
            }

        }

        private void ChangeUserPassword()
        {
            ChangePasswordResultModel result = this.Presenter.ChangePassword(this.CurrentUser.UserName, this.CurrentPassword.Text, this.Password.Text);

            this.ErrorMessage.Visible = !(result.PasswordWasChanged);
            this.ErrorMessage.Text = result.ResultMessage;
            this.SuccessMessage.Visible = result.PasswordWasChanged;
            this.SuccessMessage.Text = result.ResultMessage;
        }

    }
}