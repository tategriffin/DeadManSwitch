using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeadManSwitch.Service;

namespace DeadManSwitch.UI
{
    public static class ScheduleMapper
    {
        public static List<ScheduleViewModel> ToScheduleViewModel(this IEnumerable<ISchedule> svcSchedule)
        {
            List<ScheduleViewModel> model = new List<ScheduleViewModel>();

            foreach (var item in svcSchedule)
            {
                model.Add(item.ToScheduleViewModel());
            }

            return model;
        }

        public static ScheduleViewModel ToScheduleViewModel(this ISchedule svcSchedule)
        {
            ScheduleViewModel model = new ScheduleViewModel();

            model.Id = svcSchedule.Id;
            model.Enabled = svcSchedule.Enabled;
            model.Description = svcSchedule.Description;
            model.Interval = svcSchedule.Interval;
            model.Name = svcSchedule.Name;

            return model;
        }

        public static DailyScheduleEditModel ToEditDailyScheduleModel(this DeadManSwitch.Service.DailySchedule schedule)
        {
            DailyScheduleEditModel model = new DailyScheduleEditModel();

            model.Id = schedule.Id;
            model.IsEnabled = schedule.Enabled;
            model.ScheduleName = schedule.Name;

            model.Sunday = schedule.Sunday;
            model.Monday = schedule.Monday;
            model.Tuesday = schedule.Tuesday;
            model.Wednesday = schedule.Wednesday;
            model.Thursday = schedule.Thursday;
            model.Friday = schedule.Friday;
            model.Saturday = schedule.Saturday;

            model.CheckIn = schedule.CheckInTime.ToTimeModel();
            model.EarlyCheckIn = schedule.CheckInWindowStartTime.ToTimeModel();

            return model;
        }

        public static DeadManSwitch.Service.DailySchedule ToDailySchedule(this DailyScheduleEditModel model)
        {
            var schedule = new DeadManSwitch.Service.DailySchedule();

            schedule.Id = model.Id;
            schedule.Enabled = model.IsEnabled;
            schedule.Name = model.ScheduleName;

            schedule.Sunday = model.Sunday;
            schedule.Monday = model.Monday;
            schedule.Tuesday = model.Tuesday;
            schedule.Wednesday = model.Wednesday;
            schedule.Thursday = model.Thursday;
            schedule.Friday = model.Friday;
            schedule.Saturday = model.Saturday;

            schedule.CheckInTime = model.CheckIn.ToTimeSpan();
            schedule.CheckInWindowStartTime = model.EarlyCheckIn.ToTimeSpan();

            return schedule;
        }

        public static TimeModel ToTimeModel(this TimeSpan ts)
        {
            TimeModel model = new TimeModel(ts);

            return model;
        }

    }
}
