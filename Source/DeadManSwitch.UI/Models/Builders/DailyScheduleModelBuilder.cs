using System;
using System.Collections.Generic;
using System.Linq;
using DeadManSwitch.Service;

namespace DeadManSwitch.UI.Models.Builders
{
    public class DailyScheduleModelBuilder
    {
        private readonly IAccountService AccountSvc;

        public DailyScheduleModelBuilder(IAccountService accountService)
        {
            AccountSvc = accountService;
        }

        public DailyScheduleEditModel BuildModelForCreate(string userName)
        {
            var preferences = AccountSvc.FindUserPreferences(userName);
            DailyScheduleEditModel model = new DailyScheduleEditModel(setAllDays: true, isEnabled: true);
            model.SubmitActionText = "Create Schedule";

            TimeZoneInfo userTimeZoneInfo = preferences.TzInfo;
            DateTime userLocalTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, userTimeZoneInfo);
            DateTime nextHour = userLocalTime.AddHours(1);

            TimeSpan userLocalNextHour = new TimeSpan(nextHour.Hour, 0, 0);
            model.CheckIn = userLocalNextHour.ToTimeModel();
            model.EarlyCheckIn = userLocalNextHour.Add(preferences.EarlyCheckInOffset.Negate()).ToTimeModel();

            PopulateModelNonPersistentInfo(preferences, model);

            return model;
        }

        public DailyScheduleEditModel BuildModelForEdit(string userName, DailySchedule schedule)
        {
            DailyScheduleEditModel model = schedule.ToEditDailyScheduleModel();
            model.SubmitActionText = "Save Changes";

            var prefs = AccountSvc.FindUserPreferences(userName);
            PopulateModelNonPersistentInfo(prefs, model);
            return model;
        }

        public void PopulateModelNonPersistentInfo(string userName, DailyScheduleEditModel model)
        {
            var preferences = AccountSvc.FindUserPreferences(userName);
            PopulateModelNonPersistentInfo(preferences, model);
        }

        public void PopulateModelNonPersistentInfo(UserPreferences preferences, DailyScheduleEditModel model)
        {
            model.UserTimeZone = preferences.TzInfo.DisplayName;
            PopulateCheckInOptions(model);
        }

        private void PopulateCheckInOptions(DailyScheduleEditModel model)
        {
            model.CheckInHourOptions = BuildCheckInHourOptions();
            model.CheckInMinuteOptions = BuildCheckInMinuteOptions();
            model.CheckInAmPmOptions = BuildCheckInAmPmOptions();
        }

        private Dictionary<string, string> BuildCheckInHourOptions()
        {
            Dictionary<string, string> options = new Dictionary<string, string>();

            for (int i = 1; i < 13; i++)
            {
                options.Add(i.ToString(), i.ToString("00"));
            }

            return options;
        }

        private Dictionary<string, string> BuildCheckInMinuteOptions()
        {
            Dictionary<string, string> options = new Dictionary<string, string>();

            options.Add("0", "00");
            options.Add("15", "15");
            options.Add("30", "30");
            options.Add("45", "45");

            return options;
        }

        private Dictionary<string, string> BuildCheckInAmPmOptions()
        {
            Dictionary<string, string> options = new Dictionary<string, string>();

            options.Add("AM", "AM");
            options.Add("PM", "PM");

            return options;
        }

    }
}