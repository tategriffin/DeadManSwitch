using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeadManSwitch.Service;

namespace DeadManSwitch.UI.Models.Builders
{
    public class UserProfileModelBuilder
    {
        private readonly IAccountService AccountSvc;

        public UserProfileModelBuilder(IAccountService accountService)
        {
            AccountSvc = accountService;
        }

        public UserProfileViewModel BuildUserProfileViewModel(string userName)
        {
            User user = AccountSvc.FindUser(userName);
            UserProfileViewModel profileModel = user.ToUiViewModel();

            UserPreferences preferences = AccountSvc.FindUserPreferences(userName);
            profileModel.TimeZone = preferences.TzInfo.DisplayName;
            profileModel.EarlyCheckinDesc = preferences.EarlyCheckInOffset.TotalMinutes + " minutes";

            return profileModel;
        }

        public UserProfileEditModel BuildUserProfileEditModel(string userName)
        {
            User user = AccountSvc.FindUser(userName);
            UserProfileEditModel profileModel = user.ToUiEditModel();

            return profileModel;
        }

        public UserPreferenceEditModel BuildUserPreferenceEditModel(string userName)
        {
            var preferences = AccountSvc.FindUserPreferences(userName);
            var preferenceModel = preferences.ToUiEditModel();

            preferenceModel.TimeZoneOptions = AccountSvc.GetSupportedTimeZones();
            preferenceModel.EarlyCheckInOptions = BuildEarlyCheckInOptions();

            return preferenceModel;
        }

        private Dictionary<string, string> BuildEarlyCheckInOptions()
        {
            return AccountSvc.GetCheckInWindowOptions();
        }

    }
}
