using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeadManSwitch.Data.SqlRepository.EntityMappers;

namespace DeadManSwitch.Data.SqlRepository
{
    public class DailyScheduleRepository : IDailyScheduleRepository
    {
        public List<Schedule.DailySchedule> GetAllDailySchedulesForUser(int userId)
        {
            DeadManSwitchEntities context = new DeadManSwitchEntities();
            try
            {
                return GetAllDailySchedulesForUser(context, userId);
            }
            finally
            {
                context.Dispose();
            }
        }

        internal List<Schedule.DailySchedule> GetAllDailySchedulesForUser(DeadManSwitchEntities context, int userId)
        {
            return
                context.ScheduleDailies
                .Where(s => s.UserId == userId)
                .ToList()
                .ToDomain();
        }

        public Schedule.DailySchedule FindDailyScheduleById(int scheduleId)
        {
            DeadManSwitchEntities context = new DeadManSwitchEntities();
            try
            {
                var existingSchedule =
                    context.ScheduleDailies
                    .Where(s => s.ScheduleDailyId == scheduleId)
                    .SingleOrDefault();

                return (existingSchedule == null ? null : existingSchedule.ToDomain());
            }
            finally
            {
                context.Dispose();
            }
        }

        public void UpsertDailySchedule(Schedule.DailySchedule schedule, DateTime? nextCheckInDateTime)
        {
            DeadManSwitchEntities context = new DeadManSwitchEntities();
            try
            {
                var existingSchedule =
                    context.ScheduleDailies
                    .Where(s => s.ScheduleDailyId == schedule.Id)
                    .SingleOrDefault();
                if (existingSchedule == null)
                {
                    AddDailySchedule(context, schedule);
                }
                else
                {
                    UpdateDailySchedule(context, schedule, existingSchedule);
                }
                RecordUserCheckIn(context, schedule.UserId, nextCheckInDateTime);
                context.SaveChanges();
            }
            finally
            {
                context.Dispose();
            }
        }

        public void DeleteDailySchedule(Schedule.DailySchedule schedule, DateTime? nextCheckInDateTime)
        {
            DeadManSwitchEntities context = new DeadManSwitchEntities();
            try
            {
                var existingSchedule =
                    context.ScheduleDailies
                    .Where(s => s.ScheduleDailyId == schedule.Id)
                    .SingleOrDefault();
                if (existingSchedule != null)
                {
                    context.ScheduleDailies.Remove(existingSchedule);
                }
                RecordUserCheckIn(context, schedule.UserId, nextCheckInDateTime);
                context.SaveChanges();
            }
            finally
            {
                context.Dispose();
            }
        }

        private void AddDailySchedule(DeadManSwitchEntities context, DeadManSwitch.Schedule.DailySchedule schedule)
        {
            DeadManSwitch.Data.SqlRepository.ScheduleDaily data = new ScheduleDaily();
            data.ScheduleDailyId = 0;
            data.CreateDate = DateTime.UtcNow;

            ScheduleDailyMapper.MapDomainToData(schedule, data);

            context.ScheduleDailies.Add(data);
        }

        private void UpdateDailySchedule(DeadManSwitchEntities context, DeadManSwitch.Schedule.DailySchedule schedule, DeadManSwitch.Data.SqlRepository.ScheduleDaily data)
        {
            ScheduleDailyMapper.MapDomainToData(schedule, data);
        }

        private void RecordUserCheckIn(DeadManSwitchEntities context, int userId, DateTime? nextCheckIn)
        {
            CheckInRepository checkInRepository = new DeadManSwitch.Data.SqlRepository.CheckInRepository();

            checkInRepository.RecordCheckIn(context, userId, DateTime.UtcNow, nextCheckIn);
        }

    }
}
