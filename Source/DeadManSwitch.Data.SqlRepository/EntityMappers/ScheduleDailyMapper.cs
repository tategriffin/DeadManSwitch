using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Data.SqlRepository.EntityMappers
{
    internal static class ScheduleDailyMapper
    {
        public static List<ISchedule> ToInterface(this IEnumerable<DeadManSwitch.Schedule.DailySchedule> dataItems)
        {
            List<ISchedule> domainItems = new List<ISchedule>();

            foreach (var item in dataItems)
            {
                domainItems.Add(item);
            }

            return domainItems;
        }

        public static List<DeadManSwitch.Schedule.DailySchedule> ToDomain(this IEnumerable<DeadManSwitch.Data.SqlRepository.ScheduleDaily> dataItems)
        {
            List<DeadManSwitch.Schedule.DailySchedule> domainItems = new List<DeadManSwitch.Schedule.DailySchedule>();

            foreach (var item in dataItems)
            {
                domainItems.Add(item.ToDomain());
            }

            return domainItems;
        }

        public static DeadManSwitch.Schedule.DailySchedule ToDomain(this DeadManSwitch.Data.SqlRepository.ScheduleDaily data)
        {
            DeadManSwitch.Schedule.DailySchedule domain = new Schedule.DailySchedule();

            domain.Id = data.ScheduleDailyId;
            domain.UserId = data.UserId;
            domain.Name = data.Name;
            domain.Enabled = data.IsEnabled;

            domain.Sunday = data.Sunday;
            domain.Monday = data.Monday;
            domain.Tuesday = data.Tuesday;
            domain.Wednesday = data.Wednesday;
            domain.Thursday = data.Thursday;
            domain.Friday = data.Friday;
            domain.Saturday = data.Saturday;

            domain.CheckInTime = new TimeSpan(data.CheckInHour, data.CheckInMinute, 0);
            domain.CheckInWindowStartTime = new TimeSpan(data.CheckInWindowStart);

            return domain;
        }

        public static void MapDomainToData(DeadManSwitch.Schedule.DailySchedule domain, DeadManSwitch.Data.SqlRepository.ScheduleDaily data)
        {
            data.UserId = domain.UserId;
            data.ModDate = DateTime.UtcNow;

            data.Name = domain.Name;
            data.IsEnabled = domain.Enabled;
            
            data.Sunday = domain.Sunday;
            data.Monday = domain.Monday;
            data.Tuesday = domain.Tuesday;
            data.Wednesday = domain.Wednesday;
            data.Friday = domain.Friday;
            data.Saturday = domain.Saturday;
            data.Thursday = domain.Thursday;

            data.CheckInHour = domain.CheckInTime.Hours;
            data.CheckInMinute = domain.CheckInTime.Minutes;
            data.CheckInWindowStart = domain.CheckInWindowStartTime.Ticks;

        }

    }
}
