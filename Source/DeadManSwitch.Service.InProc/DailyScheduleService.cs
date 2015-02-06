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
        private UserProvider UserProvider;
        private DailyScheduleProvider DailyScheduleProvider;

        public DailyScheduleService(IUnityContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");

            this.UserProvider = new UserProvider(container);
            this.DailyScheduleProvider = new DailyScheduleProvider(container);
        }

        public DailySchedule FindByScheduleId(string userName, int scheduleId)
        {
            if (String.IsNullOrWhiteSpace(userName)) throw new ArgumentNullException("userName", "userName cannot be null or empty.");

            var existingUser = UserProvider.FindByUserName(userName);
            return this.DailyScheduleProvider.FindDailySchedule(existingUser, scheduleId).ToServiceEntity();
        }

        public void Save(string userName, DailySchedule schedule)
        {
            if (String.IsNullOrWhiteSpace(userName)) throw new ArgumentNullException("userName", "userName cannot be null or empty.");
            if (schedule == null) throw new ArgumentNullException("schedule");

            var existingUser = UserProvider.FindByUserName(userName);
            this.DailyScheduleProvider.SaveDailySchedule(existingUser, schedule.ToDomainEntity());
        }

        public void Delete(string userName, int scheduleId)
        {
            if (String.IsNullOrWhiteSpace(userName)) throw new ArgumentNullException("userName", "userName cannot be null or empty.");
            if (scheduleId == 0) throw new ArgumentException("scheduleId is not valid.");

            var existingUser = UserProvider.FindByUserName(userName);
            this.DailyScheduleProvider.DeleteSchedule(existingUser, scheduleId);
        }

    }
}
