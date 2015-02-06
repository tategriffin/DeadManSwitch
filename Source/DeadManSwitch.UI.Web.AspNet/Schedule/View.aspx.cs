using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DeadManSwitch.UI.Web.AspNet.Schedule
{
    public partial class View : DMSPage
    {
        private ViewSchedulesPresenter Presenter;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Presenter = new ViewSchedulesPresenter(this.CurrentUser);
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ShowExistingSchedules(this.Presenter.Schedules);

            NoSchedulesLabel.Visible = (this.Presenter.Schedules.Count == 0);
            NextCheckIn.Text = this.Presenter.NextCheckInText;

            AddHyperLink.NavigateUrl = BuildAddLink();
            AddHyperLink2.NavigateUrl = AddHyperLink.NavigateUrl;
        }

        private void ShowExistingSchedules(IEnumerable<ScheduleViewModel> allSchedules)
        {
            if (!Page.IsPostBack)
            {
                ScheduleRepeater.DataSource = allSchedules;
                ScheduleRepeater.DataBind();
            }
        }

        protected void ScheduleRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ScheduleViewModel sched = (ScheduleViewModel)e.Item.DataItem;

                HyperLink deleteScheduleLink = (HyperLink)e.Item.FindControl("DeleteHyperLink");
                HyperLink editScheduleLink = (HyperLink)e.Item.FindControl("EditHyperLink");
                Literal nameLiteral = (Literal)e.Item.FindControl("ScheduleName");

                deleteScheduleLink.NavigateUrl = BuildDeleteLink(sched.Id, sched.Interval);
                editScheduleLink.NavigateUrl = BuildEditLink(sched.Id);
                nameLiteral.Text = string.Format("{0} ({1})", sched.Name, sched.Description);
            }
        }

        private string BuildAddLink()
        {
            return "~/Schedule/Edit.aspx";
        }

        private string BuildEditLink(int scheduleId)
        {
            return "~/Schedule/Edit.aspx?" + scheduleId;
        }

        private string BuildDeleteLink(int scheduleId, RecurrenceInterval interval)
        {
            string url = "~/Schedule/Delete.aspx";
            url += "?" + DeadManSwitch.UI.Web.AspNet.Schedule.Delete.QryParmScheduleId + "=" + scheduleId;
            url += "&" + DeadManSwitch.UI.Web.AspNet.Schedule.Delete.QryParmScheduleType + "=" + (int)interval;

            return url;
        }

    }
}