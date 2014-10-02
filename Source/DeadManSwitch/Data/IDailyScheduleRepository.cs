using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Data
{
    public interface IDailyScheduleRepository
    {
        List<DeadManSwitch.Schedule.DailySchedule> GetAllDailySchedulesForUser(int userId);
        DeadManSwitch.Schedule.DailySchedule FindDailyScheduleById(int scheduleId);

        void UpsertDailySchedule(DeadManSwitch.Schedule.DailySchedule schedule, DateTime? nextCheckInDateTime);
        void DeleteDailySchedule(DeadManSwitch.Schedule.DailySchedule schedule, DateTime? nextCheckInDateTime);
    }
}
