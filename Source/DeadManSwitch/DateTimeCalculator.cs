using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch
{
    public static class DateTimeCalculator
    {
        public static TimeSpan CalculateOffset(DateTime futureTime, DateTime currentTime)
        {
            if (currentTime.Kind != futureTime.Kind) throw new ArgumentException("currentTime.Kind must be the same as futureTime.Kind");
            if (!(currentTime < futureTime)) throw new ArgumentException("currentTime must be prior to futureTime.");

            return futureTime.Subtract(currentTime);
        }

        /// <summary>
        /// Gets the user local date time.
        /// </summary>
        /// <param name="userTimeZone">The user time zone.</param>
        /// <returns></returns>
        /// <remarks>
        /// Set a breakpoint in this method to test various dates and times
        /// </remarks>
        public static DateTime GetUserLocalDateTime(TimeZoneInfo userTimeZone)
        {
            DateTime userLocalDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, userTimeZone);

            return userLocalDateTime;
        }

        public static DateTime ConvertSystemLocalToUserLocal(DateTime systemLocalDateTime, TimeZoneInfo userTimeZone)
        {
            DateTime utcDateTime = systemLocalDateTime.ToUniversalTime();

            return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, userTimeZone);
        }

    }
}
