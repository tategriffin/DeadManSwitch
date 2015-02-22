using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeadManSwitch.Service;

namespace DeadManSwitch.UI.Web.AspNet.Account
{
    public class UserPreferencesPresenter : DMSPagePresenter
    {
        private IAccountService AccountSvc;

        public UserPreferencesPresenter(CurrentUser user)
            : base(user)
        {
            this.AccountSvc = GetService<IAccountService>();

            this.PopulateModel();
        }

        public UserPreferenceEditModel Model { get; private set; }

        public void SavePreferences()
        {
            if (String.IsNullOrWhiteSpace(this.Model.TimeZoneId)) throw new ArgumentNullException("TimeZoneId", "TimeZoneId cannot be null or empty.");
            if (this.Model.EarlyCheckInMinutes < 0) throw new ArgumentException("EarlyCheckInMinutes", "EarlyCheckInMinutes cannot be less than zero.");

            this.AccountSvc.UpdatePreferences(this.CurrentUser.UserName, this.Model.ToServiceEntity());
        }

        private void PopulateModel()
        {
            UserPreferenceEditModel model = new UserPreferenceEditModel();
            UserPreferences existingPrefs = this.AccountSvc.FindUserPreferences(this.CurrentUser.UserName);

            model.EarlyCheckInMinutes = (int)existingPrefs.EarlyCheckInOffset.TotalMinutes;
            model.TimeZoneId = existingPrefs.TzInfo.Id;

            PopulateReadOnlyModelValues(model);

            this.Model = model;
        }

        private void PopulateReadOnlyModelValues(UserPreferenceEditModel model)
        {
            model.TimeZoneOptions = BuildTimeZoneOptions();
            model.EarlyCheckInOptions = BuildEarlyCheckInOptions();
        }

        private Dictionary<string, string> BuildTimeZoneOptions()
        {
            return this.AccountSvc.GetSupportedTimeZones();
        }

        private Dictionary<string, string> BuildEarlyCheckInOptions()
        {
            return AccountSvc.GetCheckInWindowOptions();
        }
    }
}
