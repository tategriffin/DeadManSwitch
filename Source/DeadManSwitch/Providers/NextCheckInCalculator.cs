using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NLog;

namespace DeadManSwitch.Providers
{
    internal class NextCheckInCalculator
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public DateTime? RecalculateNextCheckIn(UserPreferences userPrefs, List<ISchedule> userSchedules)
        {
            logger.Trace("enter");

            if (userPrefs == null) throw new ArgumentNullException("userPrefs");
            if (userSchedules == null) throw new ArgumentNullException("userSchedules");

            DateTime? nextCheckIn = GetNextCheckInDateTime(userPrefs, userSchedules);

            logger.Debug("UserID: {0}; Next checkin: {1}", userPrefs.UserId, (nextCheckIn.HasValue ? nextCheckIn.Value.ToShortTimeString() : "null"));
            logger.Trace("exit");
            return nextCheckIn;
        }

        private DateTime? GetNextCheckInDateTime(UserPreferences userPrefs, List<ISchedule> userSchedules)
        {
            DateTime? nextCheckInDateTime = null;

            List<DateTime> possibleCheckIns = BuildNextCheckInPossibilities(userPrefs, userSchedules);
            if (possibleCheckIns.Count > 0)
            {
                possibleCheckIns.Sort();
                nextCheckInDateTime = possibleCheckIns.First().ToMinutePrecision();
            }

            return nextCheckInDateTime;
        }

        /// <summary>
        /// Builds the next check in possibilities.
        /// </summary>
        /// <param name="userPrefs">The user prefs.</param>
        /// <param name="userSchedules">The user schedules.</param>
        /// <returns></returns>
        /// <remarks>
        /// Returns UTC times based on the user's local time
        /// </remarks>
        private List<DateTime> BuildNextCheckInPossibilities(UserPreferences userPrefs, List<ISchedule> userSchedules)
        {
            List<DateTime> possibleNextCheckIns = new List<DateTime>();

            TimeZoneInfo tzInfo = userPrefs.TzInfo;

            foreach (ISchedule schedule in userSchedules)
            {
                DateTime? userLocalNextCheckInForSchedule = schedule.CalculateNextCheckIn(tzInfo);
                if (userLocalNextCheckInForSchedule.HasValue)
                {
                    DateTime utcNextCheckIn = TimeZoneInfo.ConvertTimeToUtc(userLocalNextCheckInForSchedule.Value, tzInfo);
                    possibleNextCheckIns.Add(utcNextCheckIn);
                    logger.Debug("UserID: {0}; ScheduleID: {1}; Calculated checkin: {2}", userPrefs.UserId, schedule.Id, utcNextCheckIn.ToShortTimeString());
                }
            }

            return possibleNextCheckIns;
        }

    }
}
