using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DeadManSwitch.UI.Web.AspNet
{
    public partial class CheckIn : DMSPage
    {
        private CheckInPresenter Presenter;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Presenter = new CheckInPresenter(this.CurrentUser);
            CheckInResultModel result = this.Presenter.CheckIn();

            PopulateUIFromModel(result);
        }

        private void PopulateUIFromModel(CheckInResultModel model)
        {
            CheckInStatus.Text = model.CheckInStatusMessage;
            NextScheduledCheckIn.Text = model.NextCheckInText;
            if (model.NextCheckIn.HasValue)
            {
                NextScheduledCheckIn.ToolTip = model.NextCheckIn.Value.ToLongDateString();
            }
        }

    }
}