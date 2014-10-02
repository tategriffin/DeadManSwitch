using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch
{
    public class UserPreferences
    {
        public int UserId { get; set; }

        /// <summary>
        /// The user's time zone information. This should be used to
        /// calculate differences between the user's local time zone
        /// and UTC.
        /// </summary>
        public TimeZoneInfo TzInfo { get; set; }

        /// <summary>
        /// Allows the user to specify an amount of time prior to
        /// the expected checkin time that will reset the next
        /// checkin time.
        /// </summary>
        /// <remarks>
        /// For example, 
        /// assume the user is expected to check in at 10am 
        /// and the EarlyCheckInOffset is set to 2 hours.
        /// If the user checks in at 8:00am, the user's escalation
        /// procedures will not be started at 10am that day.
        ///
        /// If the user checks in at 7:00am, the user's escalation
        /// procedures will be started at 10am that day, assuming 
        /// they don't check in again between 8am and 10am.
        /// </remarks>
        private TimeSpan EarlyCheckInOffsetValue;
        public TimeSpan EarlyCheckInOffset
        {
            get { return this.EarlyCheckInOffsetValue; }
            set
            {
                if (value.TotalMinutes < 15)
                {
                    this.EarlyCheckInOffsetValue = new TimeSpan(0, 15, 0);
                }
                else
                {
                    this.EarlyCheckInOffsetValue = value;
                }
            }
        }


        public static UserPreferences GetDefaultPreferences()
        {
            return GetDefaultPreferences(0);
        }

        public static UserPreferences GetDefaultPreferences(int userId)
        {
            UserPreferences preferences = new UserPreferences();

            preferences.UserId = userId;
            preferences.EarlyCheckInOffset = new TimeSpan(0, 60, 0);
            preferences.TzInfo = TimeZoneInfo.Local;

            return preferences;
        }

    }
}
