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

        public async Task<UserProfileViewModel> BuildUserProfileViewModelAsync(string userName)
        {
            User user = await AccountSvc.FindUserAsync(userName);
            UserProfileViewModel profileModel = user.ToUiViewModel();

            UserPreferences preferences = await AccountSvc.FindUserPreferencesAsync(userName);
            profileModel.TimeZone = preferences.TzInfo.DisplayName;
            profileModel.EarlyCheckinDesc = preferences.EarlyCheckInOffset.TotalMinutes + " minutes";

            return profileModel;
        }

        public async Task<UserProfileEditModel> BuildUserProfileEditModelAsync(string userName)
        {
            User user = await AccountSvc.FindUserAsync(userName);
            UserProfileEditModel profileModel = user.ToUiEditModel();

            return profileModel;
        }

        public async Task<UserPreferenceEditModel> BuildUserPreferenceEditModelAsync(string userName)
        {
            var preferences = await AccountSvc.FindUserPreferencesAsync(userName);
            var preferenceModel = preferences.ToUiEditModel();

            preferenceModel.TimeZoneOptions = await AccountSvc.GetSupportedTimeZonesAsync();
            preferenceModel.EarlyCheckInOptions = await BuildEarlyCheckInOptionsAsync();

            return preferenceModel;
        }

        private async Task<Dictionary<string, string>> BuildEarlyCheckInOptionsAsync()
        {
            return await AccountSvc.GetCheckInWindowOptionsAsync();
        }

    }
}
