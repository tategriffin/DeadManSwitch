using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Service
{
    public interface IScheduleService
    {
        List<ISchedule> SearchAllSchedulesByUser(string userName);

        void DeleteSchedule(string userName, int scheduleTypeId, int scheduleId);

        IDailyScheduleService DailyScheduleService { get; }
    }
}
