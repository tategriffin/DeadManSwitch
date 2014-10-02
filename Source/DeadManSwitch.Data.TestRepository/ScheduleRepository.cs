using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeadManSwitch.Data.TestRepository;

namespace DeadManSwitch.Data.TestRepository
{
    public class ScheduleRepository : RepositoryWithContext, IScheduleRepository
    {
        private IDailyScheduleRepository DailyScheduleRepository;

        public ScheduleRepository(RepositoryContext context)
            : base(context)
        {
            this.DailyScheduleRepository = new DailyScheduleRepository(context);
        }

        public List<ISchedule> SearchByUserId(int userId)
        {
            List<ISchedule> userSchedules = new List<ISchedule>();

            userSchedules.AddRange(AllDailySchedules(userId));

            return userSchedules;
        }

        private IEnumerable<ISchedule> AllDailySchedules(int userId)
        {
            var schedules = this.DailyScheduleRepository.GetAllDailySchedulesForUser(userId);

            var result = new List<ISchedule>();

            foreach (var schedule in schedules)
            {
                result.Add(schedule);
            }

            return result;
        }


    }
}
