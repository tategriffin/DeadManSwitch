using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Service
{
    public class DailySchedule : Schedule
    {
        public TimeSpan CheckInWindowStartTime { get; set; }
        public TimeSpan CheckInTime { get; set; }

        public bool Sunday { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }

        public override RecurrenceInterval Interval { get { return RecurrenceInterval.Daily; } }
        public static int IntervalId { get { return (int)RecurrenceInterval.Daily; } }

    }
}
