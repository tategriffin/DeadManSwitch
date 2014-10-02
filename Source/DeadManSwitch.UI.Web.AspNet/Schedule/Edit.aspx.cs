using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DeadManSwitch.UI.Web.AspNet.Schedule
{
    public partial class Edit : DMSPage
    {
        private EditDailySchedulePresenter Presenter;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Presenter = new EditDailySchedulePresenter(this.CurrentUser, Request.QueryString);

            if (Page.IsPostBack == false)
            {
                PopulateControls(this.Presenter.Model);
            }
        }

        protected void Button_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button == null) return;

            switch (button.CommandName)
            {
                case "Save":
                    SaveChanges();
                    break;
                case "Cancel":
                    RedirectToScheduleView();
                    break;
            }

        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            this.InvalidSchedule.Visible = true;
            this.EditScheduleSection.Visible = false;

            if (this.Presenter.IsAuthorizedUser())
            {
                this.InvalidSchedule.Visible = false;
                this.EditScheduleSection.Visible = true;
                if (Page.IsPostBack == false)
                {
                    PopulateUIFromModel(this.Presenter.Model);
                }
            }
        }

        private void PopulateControls(EditDailyScheduleModel model)
        {
            PopulateTimeOptions(model);

            this.UserTimeZone.Text = model.UserTimeZone;
        }

        private void PopulateTimeOptions(EditDailyScheduleModel model)
        {
            PopulateDDL(this.CheckInStartHour, model.CheckInHourOptions);
            PopulateDDL(this.CheckInStartMinute, model.CheckInMinuteOptions);
            PopulateDDL(this.CheckInStartAmPm, model.CheckInAmPmOptions);

            PopulateDDL(this.CheckInEndHour, model.CheckInHourOptions);
            PopulateDDL(this.CheckInEndMinute, model.CheckInMinuteOptions);
            PopulateDDL(this.CheckInEndAmPm, model.CheckInAmPmOptions);
        }

        private void PopulateDDL(DropDownList ddl, Dictionary<string, string> items)
        {
            ddl.Items.Clear();
            foreach (var kvp in items)
            {
                ddl.Items.Add(new ListItem(kvp.Value, kvp.Key));
            }
        }

        private void PopulateUIFromModel(EditDailyScheduleModel model)
        {
            this.ScheduleName.Text = model.ScheduleName;
            this.ScheduleEnabled.Checked = model.IsEnabled;

            this.SundayCheckbox.Checked = model.Sunday;
            this.MondayCheckbox.Checked = model.Monday;
            this.TuesdayCheckbox.Checked = model.Tuesday;
            this.WednesdayCheckbox.Checked = model.Wednesday;
            this.ThursdayCheckbox.Checked = model.Thursday;
            this.FridayCheckbox.Checked = model.Friday;
            this.SaturdayCheckbox.Checked = model.Saturday;

            this.CheckInStartHour.SelectedValue = model.EarlyCheckIn.Hour.ToString();
            this.CheckInStartMinute.SelectedValue = model.EarlyCheckIn.Minute.ToString();
            this.CheckInStartAmPm.SelectedValue = model.EarlyCheckIn.AMPM;

            this.CheckInEndHour.SelectedValue = model.CheckIn.Hour.ToString();
            this.CheckInEndMinute.SelectedValue = model.CheckIn.Minute.ToString();
            this.CheckInEndAmPm.SelectedValue = model.CheckIn.AMPM;
        }

        private EditDailyScheduleModel PopulateModelFromUI()
        {
            EditDailyScheduleModel model = this.Presenter.Model;

            model.ScheduleName = this.ScheduleName.Text;
            model.IsEnabled = this.ScheduleEnabled.Checked;

            model.Sunday = this.SundayCheckbox.Checked;
            model.Monday = this.MondayCheckbox.Checked;
            model.Tuesday = this.TuesdayCheckbox.Checked;
            model.Wednesday = this.WednesdayCheckbox.Checked;
            model.Thursday = this.ThursdayCheckbox.Checked;
            model.Friday = this.FridayCheckbox.Checked;
            model.Saturday = this.SaturdayCheckbox.Checked;

            model.EarlyCheckIn = new TimeModel(CheckInStartHour.SelectedValue, CheckInStartMinute.SelectedValue, CheckInStartAmPm.SelectedValue);
            model.CheckIn = new TimeModel(CheckInEndHour.SelectedValue, CheckInEndMinute.SelectedValue, CheckInEndAmPm.SelectedValue);

            return model;
        }

        private void SaveChanges()
        {
            EditDailyScheduleModel model = PopulateModelFromUI();

            List<string> messages = this.Presenter.SaveSchedule(model);
            if (messages.Count > 0)
            {
                this.ShowErrorMessages(messages);
            }
            else
            {
                RedirectToScheduleView();
            }
        }

        private void ShowErrorMessages(IEnumerable<string> messages)
        {
            if (messages != null && messages.Any())
            {
                this.ErrorMessage.Text = string.Join("<br />", messages);
            }
        }

        private void RedirectToScheduleView()
        {
            Response.Redirect("~/Schedule/View");
        }
    }
}