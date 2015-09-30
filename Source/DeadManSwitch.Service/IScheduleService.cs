using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Service
{
    public interface IScheduleService
    {
        Task<List<ISchedule>> SearchAllSchedulesByUserAsync(string userName);

        Task DeleteScheduleAsync(string userName, int scheduleTypeId, int scheduleId);

        IDailyScheduleService DailyScheduleService { get; }
    }
}
