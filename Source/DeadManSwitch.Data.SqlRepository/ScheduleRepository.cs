using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeadManSwitch.Data.SqlRepository.EntityMappers;

namespace DeadManSwitch.Data.SqlRepository
{
    public class ScheduleRepository : IScheduleRepository
    {
        public List<ISchedule> SearchByUserId(int userId)
        {
            List<ISchedule> allSchedules = new List<ISchedule>();

            DeadManSwitchEntities context = new DeadManSwitchEntities();
            try
            {
                allSchedules.AddRange(AllDailySchedules(context, userId));

                return allSchedules;
            }
            finally
            {
                context.Dispose();
            }
        }

        private IEnumerable<ISchedule> AllDailySchedules(DeadManSwitchEntities context, int userId)
        {
            var repository = new DailyScheduleRepository();

            var schedules = repository.GetAllDailySchedulesForUser(context, userId);
            return schedules.ToInterface();
        }

    }
}
