using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Service
{
    public interface IDailyScheduleService
    {
        Task<DailySchedule> FindByScheduleIdAsync(string userName, int scheduleId);

        Task SaveAsync(string userName, DailySchedule schedule);

        Task DeleteAsync(string userName, int scheduleId);

        Task<Dictionary<int, string>> CheckInHourOptionsAsync();

        Task<Dictionary<int, string>> CheckInMinuteOptionsAsync();

        Task<Dictionary<string, string>> CheckInAmPmOptionsAsync();

    }
}
