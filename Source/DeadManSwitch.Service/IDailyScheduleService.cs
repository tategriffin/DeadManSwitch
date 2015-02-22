using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Service
{
    public interface IDailyScheduleService
    {
        DailySchedule FindByScheduleId(string userName, int scheduleId);

        void Save(string userName, DailySchedule schedule);

        void Delete(string userName, int scheduleId);

        Dictionary<int, string> CheckInHourOptions();

        Dictionary<int, string> CheckInMinuteOptions();

        Dictionary<string, string> CheckInAmPmOptions();

    }
}
