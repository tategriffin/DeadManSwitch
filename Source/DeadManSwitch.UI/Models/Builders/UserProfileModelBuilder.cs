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
            profileModel.EarlyCheckinDesc = preferences.EarlyCheckInOffset.TotalMinutes.ToString() + " minutes";

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

        #region Duplicate code
        private Dictionary<string, string> BuildEarlyCheckInOptions()
        {
            Dictionary<string, string> options = new Dictionary<string, string>();

            List<KeyValuePair<string, string>> optionPairs = BuildEarlyCheckInOptionPairs();
            foreach (var item in optionPairs)
            {
                options.Add(item.Key, item.Value);
            }

            return options;
        }

        private List<KeyValuePair<string, string>> BuildEarlyCheckInOptionPairs()
        {
            List<KeyValuePair<string, string>> optionKvps = new List<KeyValuePair<string, string>>();

            optionKvps.Add(BuildMinuteOption(15));
            optionKvps.Add(BuildMinuteOption(30));
            optionKvps.Add(BuildMinuteOption(45));
            optionKvps.Add(BuildMinuteOption(60));
            optionKvps.Add(BuildMinuteOption(90));
            optionKvps.Add(BuildMinuteOption(120));

            return optionKvps;
        }

        private KeyValuePair<string, string> BuildMinuteOption(int totalMinutes)
        {
            //TODO: Update this to allow hours (30 minutes, 1 hour, 1.5 hours, etc.)

            KeyValuePair<string, string> option = new KeyValuePair<string, string>(totalMinutes.ToString(), totalMinutes.ToString() + " minutes");

            return option;
        }
        #endregion

    }
}
