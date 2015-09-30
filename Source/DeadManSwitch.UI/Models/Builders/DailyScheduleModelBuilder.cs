using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeadManSwitch.Service;

namespace DeadManSwitch.UI.Models.Builders
{
    public class DailyScheduleModelBuilder
    {
        private readonly IAccountService AccountSvc;
        private readonly IScheduleService ScheduleSvc;

        public DailyScheduleModelBuilder(IAccountService accountService, IScheduleService scheduleService)
        {
            AccountSvc = accountService;
            ScheduleSvc = scheduleService;
        }

        public async Task<DailyScheduleEditModel> BuildModelForCreateAsync(string userName)
        {
            var preferences = await AccountSvc.FindUserPreferencesAsync(userName);
            DailyScheduleEditModel model = new DailyScheduleEditModel(setAllDays: true, isEnabled: true) {SubmitActionText = "Create Schedule"};

            TimeZoneInfo userTimeZoneInfo = preferences.TzInfo;
            DateTime userLocalTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, userTimeZoneInfo);
            DateTime nextHour = userLocalTime.AddHours(1);

            TimeSpan userLocalNextHour = new TimeSpan(nextHour.Hour, 0, 0);
            model.CheckIn = userLocalNextHour.ToTimeModel();
            model.EarlyCheckIn = userLocalNextHour.Add(preferences.EarlyCheckInOffset.Negate()).ToTimeModel();

            await PopulateModelNonPersistentInfoAsync(preferences, model);

            return model;
        }

        public async Task<DailyScheduleEditModel> BuildModelForEditAsync(string userName, DailySchedule schedule)
        {
            DailyScheduleEditModel model = schedule.ToEditDailyScheduleModel();
            model.SubmitActionText = "Save Changes";

            var prefs = await AccountSvc.FindUserPreferencesAsync(userName);
            await PopulateModelNonPersistentInfoAsync(prefs, model);

            return model;
        }

        public async Task PopulateModelNonPersistentInfoAsync(string userName, DailyScheduleEditModel model)
        {
            var preferences = await AccountSvc.FindUserPreferencesAsync(userName);
            await PopulateModelNonPersistentInfoAsync(preferences, model);
        }

        public async Task PopulateModelNonPersistentInfoAsync(UserPreferences preferences, DailyScheduleEditModel model)
        {
            model.UserTimeZone = preferences.TzInfo.DisplayName;
            await PopulateCheckInOptionsAsync(model);
        }

        private async Task PopulateCheckInOptionsAsync(DailyScheduleEditModel model)
        {
            model.CheckInHourOptions = await BuildCheckInHourOptionsAsync();
            model.CheckInMinuteOptions = await BuildCheckInMinuteOptionsAsync();
            model.CheckInAmPmOptions = await BuildCheckInAmPmOptionsAsync();
        }

        private async Task<Dictionary<string, string>> BuildCheckInHourOptionsAsync()
        {
            var hourOptions = await ScheduleSvc.DailyScheduleService.CheckInHourOptionsAsync();
            return hourOptions.ToDictionary(kvp => kvp.Key.ToString(), kvp => kvp.Value);
        }

        private async Task<Dictionary<string, string>> BuildCheckInMinuteOptionsAsync()
        {
            var minuteOptions = await ScheduleSvc.DailyScheduleService.CheckInMinuteOptionsAsync();
            return minuteOptions.ToDictionary(kvp => kvp.Key.ToString(), kvp => kvp.Value);
        }

        private async Task<Dictionary<string, string>> BuildCheckInAmPmOptionsAsync()
        {
            return await ScheduleSvc.DailyScheduleService.CheckInAmPmOptionsAsync();
        }

    }
}