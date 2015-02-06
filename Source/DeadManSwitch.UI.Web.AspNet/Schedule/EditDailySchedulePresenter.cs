using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeadManSwitch.Service;

namespace DeadManSwitch.UI.Web.AspNet.Schedule
{
    public class EditDailySchedulePresenter : EditSchedulePresenter
    {
        private static NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        public EditDailySchedulePresenter(CurrentUser user, System.Collections.Specialized.NameValueCollection qryString)
            : base(user, qryString)
        {
            this.PopulateModel();
        }

        public DailyScheduleEditModel Model { get; private set; }

        public bool IsAuthorizedUser()
        {
            bool isAuthorized = false;

            if (this.Action == EditScheduleAction.Add)
            {
                isAuthorized = true;
            }
            else if (this.Action == EditScheduleAction.Change)
            {
                var existingUserSchedule = this.FindSchedule(this.ScheduleId);
                isAuthorized = (existingUserSchedule == null ? false : true);
            }

            return isAuthorized;
        }

        public List<string> SaveSchedule(DailyScheduleEditModel model)
        {
            List<string> messages = new List<string>();

            try
            {
                DailySchedule schedule = model.ToDailySchedule();

                ScheduleSvc.DailyScheduleService.Save(this.CurrentUser.UserName, schedule);
            }
            catch (Exception ex)
            {
                Log.Error("User {0} could not save schedule ID: {1}. {2}", this.CurrentUser.UserName, this.ScheduleId, ex);
                messages.Add("The schedule could not be saved.");
            }

            return messages;
        }

        private void PopulateModel()
        {
            UserPreferences preferences = this.AccountSvc.FindUserPreferences(CurrentUser.UserName);

            switch (this.Action)
            {
                case EditScheduleAction.Add:
                    this.Model = BuildModelForAdd(preferences);
                    break;
                case EditScheduleAction.Change:
                    this.Model = BuildModelForChange(preferences);
                    break;
            }
        }

        private DailyScheduleEditModel BuildModelForAdd(UserPreferences preferences)
        {
            DailyScheduleEditModel model = new DailyScheduleEditModel(setAllDays: true, isEnabled: true);

            TimeZoneInfo userTimeZoneInfo = preferences.TzInfo;
            DateTime userLocalTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, userTimeZoneInfo);
            DateTime nextHour = userLocalTime.AddHours(1);

            TimeSpan userLocalNextHour = new TimeSpan(nextHour.Hour, 0, 0);
            model.CheckIn = userLocalNextHour.ToTimeModel();
            model.EarlyCheckIn = userLocalNextHour.Add(preferences.EarlyCheckInOffset.Negate()).ToTimeModel();

            SetReadOnlyModelValues(model, userTimeZoneInfo);

            return model;
        }

        private DailyScheduleEditModel BuildModelForChange(UserPreferences preferences)
        {
            DailyScheduleEditModel model;

            DailySchedule schedule = FindSchedule(this.ScheduleId);
            if (schedule == null)
            {
                Log.Warn(string.Format("Schedule ID: {0} was not found for user {1}.", this.ScheduleId, CurrentUser.UserName));
                return BuildModelForAdd(preferences);
            }

            model = schedule.ToEditDailyScheduleModel();

            SetReadOnlyModelValues(model, preferences.TzInfo);

            return model;
        }

        private DailySchedule FindSchedule(int scheduleId)
        {
            DailySchedule schedule = null;
            if (this.CurrentUser.IsAuthenticated)
            {
                schedule = this.ScheduleSvc.DailyScheduleService.FindByScheduleId(CurrentUser.UserName, scheduleId);
            }

            return schedule;
        }

        private void SetReadOnlyModelValues(DailyScheduleEditModel model, TimeZoneInfo userTimeZoneInfo)
        {
            model.CheckInHourOptions = BuildCheckInHourOptions();
            model.CheckInMinuteOptions = BuildCheckInMinuteOptions();
            model.CheckInAmPmOptions = BuildCheckInAmPmOptions();

            model.UserTimeZone = userTimeZoneInfo.DisplayName;
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

//            options.Add("0", "00");
//            options.Add("5", "05");
//            options.Add("10", "10");
//            options.Add("15", "15");
//            options.Add("20", "20");
//            options.Add("25", "25");
//            options.Add("30", "30");
//            options.Add("35", "35");
//            options.Add("40", "40");
//            options.Add("45", "45");
//            options.Add("50", "50");
//            options.Add("55", "55");

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
