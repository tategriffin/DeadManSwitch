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

                allSchedules.Sort(DefaultSort);
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

        private int DefaultSort(ISchedule current, ISchedule other)
        {
            return CompareByEnabledAndName(current, other);
        }

        private int CompareByEnabledAndName(ISchedule current, ISchedule other)
        {
            if (current == null && other == null) return 0;
            if (current == null && other != null) return -1;
            if (current != null && other == null) return 1;

            //neither parm is null
            int enabledFlag = CompareByEnabled(current, other);
            if (enabledFlag != 0)
            {
                return enabledFlag;
            }
            else
            {
                return CompareByName(current, other);
            }

        }

        private int CompareByEnabled(ISchedule current, ISchedule other)
        {
            int result = current.Enabled.CompareTo(other.Enabled);

            //Show enabled first
            return (result == 0 ? result : (-1) * result);
        }

        private int CompareByName(ISchedule current, ISchedule other)
        {
            return current.Name.CompareTo(other.Name);
        }

    }
}
