using DeadManSwitch.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Data
{
    public interface IScheduleRepository
    {
        List<ISchedule> SearchByUserId(int userId);
    }
}
