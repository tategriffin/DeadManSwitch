using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Schedule
{
    internal class DailyScheduleCheckInWindow
    {
        public DailyScheduleCheckInWindow(DateTime checkInDate, TimeSpan checkInTime, TimeSpan earlyCheckInTime)
        {
            SetCheckInWindowValues(checkInDate, checkInTime, earlyCheckInTime);
        }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public bool IsInWindow(DateTime userLocalDateTime)
        {
            return (this.Start <= userLocalDateTime && userLocalDateTime <= this.End);
        }

        private void SetCheckInWindowValues(DateTime baseDate, TimeSpan checkInTime, TimeSpan earlyCheckInTime)
        {
            DateTime checkInStartDate;

            DateTime checkInEndDate = new DateTime(baseDate.Year, baseDate.Month, baseDate.Day);
            if (earlyCheckInTime > checkInTime)
            {
                //must be previous day
                checkInStartDate = checkInEndDate.AddDays(-1);
            }
            else
            {
                checkInStartDate = checkInEndDate;
            }

            this.Start = checkInStartDate.Add(earlyCheckInTime);
            this.End = checkInEndDate.Add(checkInTime);
        }
    }
}
