using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DeadManSwitch.UI.Web.AspNet
{
    public partial class _Default : DMSPage
    {
        private HomePresenter Presenter;

        public _Default()
            : base(true, false) { }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Presenter = new HomePresenter(this.CurrentUser);
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            this.FeaturedMessage.Text = this.Presenter.FeaturedMessage;
            this.CheckInNow.Visible = this.Presenter.IsCheckInNowVisible;
        }

        protected void Button_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) throw new ArgumentException(string.Format("Expected sender of type {0}.", typeof(Button)));

            switch (btn.CommandName)
            {
                case "CheckInNow":
                    Response.Redirect("~/CheckIn");
                    break;
            }
        }

    }
}