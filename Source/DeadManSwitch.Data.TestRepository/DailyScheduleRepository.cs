using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeadManSwitch.Schedule;

namespace DeadManSwitch.Data.TestRepository
{
    public class DailyScheduleRepository : RepositoryWithContext, IDailyScheduleRepository
    {
        public DailyScheduleRepository(RepositoryContext context)
            :base(context) { }

        public List<DailySchedule> GetAllDailySchedulesForUser(int userId)
        {
            return
                Context.DailySchedules
                .Where(r => r.UserId == userId)
                .ToList();
        }

        public Schedule.DailySchedule FindDailyScheduleById(int scheduleId)
        {
            return
                Context.DailySchedules
                .Where(r => r.Id == scheduleId)
                .SingleOrDefault();
        }

        public void UpsertDailySchedule(Schedule.DailySchedule schedule, DateTime? nextCheckInDateTime)
        {
            var existing = Context.DailySchedules.SingleOrDefault(s => s.Id == schedule.Id);
            if (existing != null)
            {
                int idx = Context.DailySchedules.IndexOf(existing);
                Context.DailySchedules[idx] = schedule;
            }
            else
            {
                Context.DailySchedules.Add(schedule);
            }

            UpdateUserNextCheckIn(schedule.UserId, nextCheckInDateTime);
        }

        public void DeleteDailySchedule(Schedule.DailySchedule schedule, DateTime? nextCheckInDateTime)
        {
            var existingSchedule =
                Context.DailySchedules
                .Where(r => r.Id == schedule.Id)
                .SingleOrDefault();

            Context.DailySchedules.Remove(existingSchedule);

            UpdateUserNextCheckIn(schedule.UserId, nextCheckInDateTime);
        }

        private void UpdateUserNextCheckIn(int userId, DateTime? nextCheckIn)
        {
            CheckInRepository checkInRepository = new DeadManSwitch.Data.TestRepository.CheckInRepository(this.Context);

            checkInRepository.RecordCheckIn(userId, DateTime.Now, nextCheckIn);
        }

    }
}
