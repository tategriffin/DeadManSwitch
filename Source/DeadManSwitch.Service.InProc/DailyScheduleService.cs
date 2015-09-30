using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeadManSwitch.Providers;
using DeadManSwitch.Service.EntityMappers;
using Microsoft.Practices.Unity;

namespace DeadManSwitch.Service
{
    public class DailyScheduleService : IDailyScheduleService
    {
        private readonly UserProvider UserProvider;
        private readonly DailyScheduleProvider DailyScheduleProvider;
        private readonly ReferenceDataProvider RefDataProvider;

        public DailyScheduleService(IUnityContainer container)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));

            this.UserProvider = new UserProvider(container);
            this.DailyScheduleProvider = new DailyScheduleProvider(container);
            this.RefDataProvider = new ReferenceDataProvider(container);
        }

        public Task<DailySchedule> FindByScheduleIdAsync(string userName, int scheduleId)
        {
            return Task.FromResult(FindByScheduleId(userName, scheduleId));
        }

        public DailySchedule FindByScheduleId(string userName, int scheduleId)
        {
            if (String.IsNullOrWhiteSpace(userName)) throw new ArgumentNullException(nameof(userName), "userName cannot be null or empty.");

            var existingUser = UserProvider.FindByUserName(userName);
            return this.DailyScheduleProvider.FindDailySchedule(existingUser, scheduleId).ToServiceEntity();
        }

        public Task SaveAsync(string userName, DailySchedule schedule)
        {
            Save(userName, schedule);

            return Task.CompletedTask;
        }

        public void Save(string userName, DailySchedule schedule)
        {
            if (String.IsNullOrWhiteSpace(userName)) throw new ArgumentNullException(nameof(userName), "userName cannot be null or empty.");
            if (schedule == null) throw new ArgumentNullException(nameof(schedule));

            var existingUser = UserProvider.FindByUserName(userName);
            this.DailyScheduleProvider.SaveDailySchedule(existingUser, schedule.ToDomainEntity());
        }

        public Task DeleteAsync(string userName, int scheduleId)
        {
            Delete(userName, scheduleId);

            return Task.CompletedTask;
        }

        public void Delete(string userName, int scheduleId)
        {
            if (String.IsNullOrWhiteSpace(userName)) throw new ArgumentNullException(nameof(userName), "userName cannot be null or empty.");
            if (scheduleId == 0) throw new ArgumentException("scheduleId is not valid.");

            var existingUser = UserProvider.FindByUserName(userName);
            this.DailyScheduleProvider.DeleteSchedule(existingUser, scheduleId);
        }

        public Task<Dictionary<int, string>> CheckInHourOptionsAsync()
        {
            return Task.FromResult(CheckInHourOptions());
        }

        public Dictionary<int, string> CheckInHourOptions()
        {
            return RefDataProvider.HourOptions();
        }

        public Task<Dictionary<int, string>> CheckInMinuteOptionsAsync()
        {
            return Task.FromResult(CheckInMinuteOptions());
        }

        public Dictionary<int, string> CheckInMinuteOptions()
        {
            return RefDataProvider.MinuteOptions();
        }

        public Task<Dictionary<string, string>> CheckInAmPmOptionsAsync()
        {
            return Task.FromResult(CheckInAmPmOptions());
        }
        public Dictionary<string, string> CheckInAmPmOptions()
        {
            return RefDataProvider.AmPmOptions();
        }

    }
}
