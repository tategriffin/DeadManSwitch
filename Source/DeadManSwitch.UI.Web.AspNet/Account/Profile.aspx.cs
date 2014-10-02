using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DeadManSwitch.UI.Web.AspNet.Account
{
    public partial class Profile : DMSPage
    {
        private UserProfilePresenter Presenter;

        public Profile()
            : base(allowUnauthenticatedUser: false, requireReauthentication: true) { }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Presenter = new UserProfilePresenter(this.CurrentUser);
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            this.UserName.Text = this.Presenter.Model.UserName;
            this.Email.Text = this.Presenter.Model.Email;
            this.FirstName.Text = this.Presenter.Model.FirstName;
            this.LastName.Text = this.Presenter.Model.LastName;
        }

        protected void Button_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button == null) return;

            switch (button.CommandName)
            {
                case "Save":
                    SaveProfile();
                    break;
            }

        }

        private void SaveProfile()
        {
            this.Presenter.Model.Email = this.Email.Text;
            this.Presenter.Model.FirstName = this.FirstName.Text;
            this.Presenter.Model.LastName = this.LastName.Text;

            this.Presenter.SaveProfile();
        }

    }
}