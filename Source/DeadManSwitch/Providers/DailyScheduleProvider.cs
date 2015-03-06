using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeadManSwitch.Data;
using DeadManSwitch.Schedule;
using Microsoft.Practices.Unity;
using NLog;

namespace DeadManSwitch.Providers
{
    public class DailyScheduleProvider : ScheduleProvider
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();

        private IDailyScheduleRepository DailyScheduleRepository;

        public DailyScheduleProvider(IUnityContainer container)
            : base(container)
        {
            this.DailyScheduleRepository = container.Resolve<IDailyScheduleRepository>();
        }

        public DailySchedule FindDailySchedule(User user, int scheduleId)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (user.UserId == 0) throw new ArgumentException("userId is not valid.");
            if (scheduleId == 0) throw new ArgumentException("scheduleId is not valid.");

            DeadManSwitch.Schedule.DailySchedule schedule = this.DailyScheduleRepository.FindDailyScheduleById(scheduleId);
            if (schedule != null && schedule.UserId != user.UserId)
            {
                //Return null schedule if it's not the user's schedule
                schedule = null;
            }

            return schedule;
        }

        public void SaveDailySchedule(User user, DailySchedule schedule)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (user.UserId == 0) throw new ArgumentException("userId is not valid.");
            if (schedule == null) throw new ArgumentNullException("schedule");
            if (IsUserAuthorized(user, schedule) == false) throw new Exception("You're not authorized to save this schedule.");

            schedule.UserId = user.UserId;

//            if (schedule.Id == 0 && schedule.UserId == 0)   //this is a new schedule
//            {
//                schedule.UserId = user.UserId;
//            }

            ThrowExceptionValidationMessages(schedule);

            DateTime? nextCheckIn = RecalculateNextCheckInOnSave(user, schedule);
            this.DailyScheduleRepository.UpsertDailySchedule(schedule, nextCheckIn);
        }

        private void ThrowExceptionValidationMessages(DailySchedule schedule)
        {
            List<string> validationMessages = ValidateDailySchedule(schedule);
            if (validationMessages.Count != 0)
            {
                string errMsg;
                if (validationMessages.Count == 1)
                {
                    errMsg = validationMessages.First();
                }
                else
                {
                    errMsg = "The account could not be created because " + string.Join(" and ", validationMessages);
                }

                throw new Exception(errMsg);
            }
        }

        public void DeleteSchedule(User user, int scheduleId)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (user.UserId == 0) throw new ArgumentException("userId is not valid.");
            if (scheduleId == 0) throw new ArgumentException("scheduleId is not valid.");

            DailySchedule schedule = this.DailyScheduleRepository.FindDailyScheduleById(scheduleId);
            if (schedule == null)
            {
                Log.Info(string.Format("User ID: {0} attempted to delete non-existent schedule ID: {1}", user.UserId, scheduleId));
            }
            else if (schedule.UserId != user.UserId)
            {
                throw new Exception(string.Format("User ID: {0} is not authorized to delete schedule ID: {1}", user.UserId, scheduleId));
            }
            else
            {
                DateTime? nextCheckIn = RecalculateNextCheckInOnDelete(user, schedule);
                this.DailyScheduleRepository.DeleteDailySchedule(schedule, nextCheckIn);
            }
        }

        private bool IsUserAuthorized(User user, DailySchedule schedule)
        {
            bool isAuthorized = false;
            if (schedule.Id == 0)   //It's a new schedule, let the user add it
            {
                if (schedule.UserId == 0 || schedule.UserId == user.UserId)
                {
                    isAuthorized = true;
                }
            }
            else
            {
                //Don't trust schedule.UserId to be correct. Ask the DB instead.
                DailySchedule existingSchedule = FindDailySchedule(user, schedule.Id);
                if (existingSchedule != null)
                {
                    isAuthorized = true;
                }
            }

            return isAuthorized;
        }

        public List<string> ValidateDailySchedule(DailySchedule schedule)
        {
            List<string> validationMessages = schedule.Validate();
            if (validationMessages.Count == 0)
            {
                validationMessages.AddIfNonEmpty(ValidateDailyScheduleOverlap(schedule));
            }

            return validationMessages;
        }

        private string ValidateDailyScheduleOverlap(DailySchedule schedule)
        {
            string msg = string.Empty;

            List<DailySchedule> otherExistingSchedules =
                this.DailyScheduleRepository.GetAllDailySchedulesForUser(schedule.UserId)
                .Where(s => s.Id != schedule.Id)
                .ToList();

            foreach (var item in otherExistingSchedules)
            {
                if (item.EqualDaysAndTime(schedule))
                {
                    msg = string.Format("Schedule '{0}' is this same as existing schedule '{1}'", schedule.Name, item.Name);
                    break;
                }
            }

            return msg;
        }

    }
}
