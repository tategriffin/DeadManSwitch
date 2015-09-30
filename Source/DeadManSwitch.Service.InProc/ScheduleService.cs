using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeadManSwitch;
using DeadManSwitch.Data;
using DeadManSwitch.Providers;
using DeadManSwitch.Service.EntityMappers;
using Microsoft.Practices.Unity;

namespace DeadManSwitch.Service
{
    public class ScheduleService : IScheduleService
    {
        private readonly UserProvider UserProvider;
        private readonly ScheduleProvider ScheduleProvider;

        private readonly IDailyScheduleService DailyScheduleSvc;

        public ScheduleService(IUnityContainer container)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));

            this.UserProvider = new UserProvider(container);
            this.ScheduleProvider = new ScheduleProvider(container);

            //Setup specific schedule services
            this.DailyScheduleSvc = new DailyScheduleService(container);
        }

        public IDailyScheduleService DailyScheduleService { get { return this.DailyScheduleSvc; } }


        public Task<List<ISchedule>> SearchAllSchedulesByUserAsync(string userName)
        {
            return Task.FromResult(SearchAllSchedulesByUser(userName));
        }

        public List<ISchedule> SearchAllSchedulesByUser(string userName)
        {
            if (String.IsNullOrWhiteSpace(userName)) throw new ArgumentNullException(nameof(userName), "userName cannot be null or empty.");

            var existingUser = UserProvider.FindByUserName(userName);
            return this.ScheduleProvider.GetAllUserSchedules(existingUser).ToServiceInterfaceList();
        }

        public Task DeleteScheduleAsync(string userName, int scheduleTypeId, int scheduleId)
        {
            DeleteSchedule(userName, scheduleTypeId, scheduleId);

            return Task.CompletedTask;
        }

        public void DeleteSchedule(string userName, int scheduleTypeId, int scheduleId)
        {
            string exceptionMsg = $"The specified schedule recurrence interval {scheduleTypeId} is not supported.";

            //TODO: Invert this functionality to ask known types if they can handle the specified schedule type
            try
            {
                RecurrenceInterval interval = (RecurrenceInterval)scheduleTypeId;
                switch (interval)
                {
                    case RecurrenceInterval.Daily:
                        this.DailyScheduleSvc.DeleteAsync(userName, scheduleId);
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(exceptionMsg, ex);
            }
        }

    }
}
