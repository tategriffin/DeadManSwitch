using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeadManSwitch.Data;
using Microsoft.Practices.Unity;
using NLog;

namespace DeadManSwitch.Providers
{
    public class ScheduleProvider
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private IScheduleRepository ScheduleRepository;
        private UserPreferenceProvider UserPreferencePvdr;

        public ScheduleProvider(IUnityContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");

            this.ScheduleRepository = container.Resolve<IScheduleRepository>();
            this.UserPreferencePvdr = new UserPreferenceProvider(container);
        }

        public List<ISchedule> GetAllUserSchedules(User user)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (user.UserId == 0) throw new ArgumentException("userId is not valid.");

            List<ISchedule> schedules = this.ScheduleRepository.SearchByUserId(user.UserId);

            return schedules;
        }

        protected DateTime? RecalculateNextCheckInOnSave(User user, ISchedule scheduleBeingSaved)
        {
            List<ISchedule> allSchedules = this.GetAllUserSchedules(user);
            ISchedule existingSchedule = allSchedules.SingleOrDefault(s => s.Id == scheduleBeingSaved.Id);
            if (existingSchedule != null)
            {
                //If we're changing a schedule, remove the existing schedule from recalc
                allSchedules.Remove(existingSchedule);
            }

            //Include new or changing schedule in recalc
            allSchedules.Add(scheduleBeingSaved);

            return this.RecalculateNextCheckIn(user, allSchedules);
        }

        protected DateTime? RecalculateNextCheckInOnDelete(User user, ISchedule scheduleBeingDeleted)
        {
            List<ISchedule> allSchedules = this.GetAllUserSchedules(user);
            ISchedule existingSchedule = allSchedules.SingleOrDefault(s => s.Id == scheduleBeingDeleted.Id);
            if (existingSchedule != null)
            {
                allSchedules.Remove(existingSchedule);
            }

            return this.RecalculateNextCheckIn(user, allSchedules);
        }

        private DateTime? RecalculateNextCheckIn(User user, List<ISchedule> allSchedules)
        {
            UserPreferences userPrefs = this.UserPreferencePvdr.Find(user);

            NextCheckInCalculator calc = new NextCheckInCalculator();
            DateTime? nextCheckIn = calc.RecalculateNextCheckIn(userPrefs, allSchedules);

            return nextCheckIn;
        }

    }
}
