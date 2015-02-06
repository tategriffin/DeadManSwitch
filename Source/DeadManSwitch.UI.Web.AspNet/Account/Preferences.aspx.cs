using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DeadManSwitch.UI.Web.AspNet.Account
{
    public partial class Preferences : DMSPage
    {
        private UserPreferencesPresenter Presenter;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Presenter = new UserPreferencesPresenter(this.CurrentUser);

            if (Page.IsPostBack == false)
            {
                PopulatePreferenceOptions();
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            this.UserName.Text = this.CurrentUser.UserName;

            this.TimeZone.SelectedValue = this.Presenter.Model.TimeZoneId;
            this.EarlyCheckInMinutes.SelectedValue = this.Presenter.Model.EarlyCheckInMinutes.ToString();
        }

        protected void Button_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button == null) return;

            switch (button.CommandName)
            {
                case "Save":
                    SavePreferences();
                    break;
            }

        }

        private void PopulatePreferenceOptions()
        {
            PopulateTimeZoneOptions();
            PopulateEarlyCheckInOptions();
        }

        private void PopulateTimeZoneOptions()
        {
            TimeZone.Items.AddRange(this.Presenter.Model.TimeZoneOptions.ToListItems());
        }

        private void PopulateEarlyCheckInOptions()
        {
            EarlyCheckInMinutes.Items.AddRange(this.Presenter.Model.EarlyCheckInOptions.ToListItems());
        }

        private void SavePreferences()
        {
            PopulateModelFromUI();

            this.Presenter.SavePreferences();
        }

        private void PopulateModelFromUI()
        {
            PopulateEarlyCheckInMinutesFromUI(this.Presenter.Model);
            PopulateUserTimeZoneFromUI(this.Presenter.Model);
        }

        private void PopulateEarlyCheckInMinutesFromUI(UserPreferenceEditModel model)
        {
            int mins;
            if (int.TryParse(this.EarlyCheckInMinutes.SelectedValue, out mins))
            {
                model.EarlyCheckInMinutes = mins;
            }
        }

        private void PopulateUserTimeZoneFromUI(UserPreferenceEditModel model)
        {
            if (this.TimeZone.SelectedIndex >= 0)
            {
                model.TimeZoneId = this.TimeZone.SelectedValue;
            }
        }

    }
}