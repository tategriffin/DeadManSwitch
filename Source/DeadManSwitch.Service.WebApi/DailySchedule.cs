using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Service.WebApi
{
    public class DailySchedule : Schedule
    {
        private TimeSpan CheckInWindowStartTimeValue { get; set; }
        public string CheckInWindowStartTime
        {
            get { return CheckInWindowStartTimeValue.ToString(); }
            set { CheckInWindowStartTimeValue = TimeSpan.Parse(value); }
        }

        private TimeSpan CheckInTimeValue { get; set; }
        public string CheckInTime
        {
            get { return CheckInTimeValue.ToString(); }
            set { CheckInTimeValue = TimeSpan.Parse(value); }
        }


        public bool Sunday { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }

        public override int Interval
        {
            get { return Service.DailySchedule.IntervalId; }
            set { int x = value; } //noop
        }
    }
}
