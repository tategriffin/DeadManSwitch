using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Service
{
    public class CheckInInfo
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public DateTime? CheckInTimeUtc { get; set; }
        public DateTime? CheckInTimeUserLocal { get { return this.ConvertToUserLocal(this.CheckInTimeUtc); } }
        public DateTime? NextCheckInTimeUtc { get; set; }
        public DateTime? NextCheckInTimeUserLocal { get { return this.ConvertToUserLocal(this.NextCheckInTimeUtc); } }
        public TimeZoneInfo UserTimeZone { get; set; }

        private DateTime? ConvertToUserLocal(DateTime? utcDateTime)
        {
            DateTime? userLocal = null;

            if (utcDateTime.HasValue)
            {
                userLocal = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime.Value, this.UserTimeZone);
            }

            return userLocal;
        }

        public string GetNextCheckInText()
        {
            string txt = "Unknown";
            if (this.NextCheckInTimeUserLocal.HasValue)
            {
                DateTime userLocalTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, this.UserTimeZone);
                if (userLocalTime < this.NextCheckInTimeUserLocal.Value)
                {
                    txt = BuildNextCheckInTextFromDate(userLocalTime, this.NextCheckInTimeUserLocal.Value);
                }
            }

            return txt;
        }

        private string BuildNextCheckInTextFromDate(DateTime userLocalTime, DateTime userLocalNextCheckIn)
        {
            if (userLocalNextCheckIn <= userLocalTime) return "Unknown";

            string txt = userLocalNextCheckIn.ToString();

            if (userLocalNextCheckIn.IsSameDay(userLocalTime))      //Same day
            {
                TimeSpan ts = new TimeSpan(userLocalNextCheckIn.Hour, userLocalNextCheckIn.Minute, 0);
                txt = "today " + ts.ToTimeOfDay("at ");
            }
            else
            {
                var dow = userLocalNextCheckIn.DayOfWeek.ToString();
                TimeSpan ts = new TimeSpan(userLocalNextCheckIn.Hour, userLocalNextCheckIn.Minute, 0);

                var diff = userLocalNextCheckIn.Date.Subtract(userLocalTime.Date);
                if (diff.Days < 7)
                {
                    txt = dow + " " + ts.ToTimeOfDay("at ");
                }
                else if (diff.Days == 7)
                {
                    txt = "next " + dow + " " + ts.ToTimeOfDay("at ");
                }
            }

            return txt;
        }

    }
}
