using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeadManSwitch.Data;
using Microsoft.Practices.Unity;

namespace DeadManSwitch.Providers
{
    public class CheckInProvider
    {
        private ScheduleProvider SchedulePvdr;
        private UserPreferenceProvider UserPreferencePvdr;
        private ICheckInRepository CheckInRepository;

        public CheckInProvider(IUnityContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");

            this.CheckInRepository = container.Resolve<ICheckInRepository>();
            this.SchedulePvdr = new ScheduleProvider(container);
            this.UserPreferencePvdr = new UserPreferenceProvider(container);
        }

        public CheckInInfo FindLastCheckIn(User user)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (user.UserId == 0) throw new ArgumentException("userId is not valid.");

            CheckInInfo utcCheckInInfo = CheckInRepository.FindLastCheckInByUser(user.UserId);

            return utcCheckInInfo;
        }

        public CheckInInfo RecordCheckIn(User user)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (user.UserId == 0) throw new ArgumentException("userId is not valid.");

            DateTime? nextCheckInDateTime = this.RecalculateNextCheckInForUser(user);

            this.RecordCheckIn(user.UserId, nextCheckInDateTime);

            return this.FindLastCheckIn(user);
        }

        private void RecordCheckIn(int userId, DateTime? nextCheckInDateTime)
        {
            if (nextCheckInDateTime.HasValue && nextCheckInDateTime.Value.Kind != DateTimeKind.Utc) throw new ArgumentException("nextCheckInDateTime.Kind must be UTC");
            CheckInRepository.RecordCheckIn(userId, DateTime.UtcNow, nextCheckInDateTime);
        }

        public DateTime? RecalculateNextCheckInForUser(User user)
        {
            UserPreferences userPrefs = this.UserPreferencePvdr.Find(user);
            List<ISchedule> userSchedules = this.SchedulePvdr.GetAllUserSchedules(user);

            NextCheckInCalculator calc = new NextCheckInCalculator();
            DateTime? nextCheckInDateTime = calc.RecalculateNextCheckIn(userPrefs, userSchedules);

            return nextCheckInDateTime;
        }

    }
}
