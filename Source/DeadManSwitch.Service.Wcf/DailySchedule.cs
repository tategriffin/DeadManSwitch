using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Service.Wcf
{
    [Serializable]
    [DataContract]
    public class DailySchedule : Schedule
    {
        private TimeSpan CheckInWindowStartTimeValue { get; set; }
        [DataMember]
        public string CheckInWindowStartTime
        {
            get { return CheckInWindowStartTimeValue.ToString(); }
            set { CheckInWindowStartTimeValue = TimeSpan.Parse(value); }
        }

        private TimeSpan CheckInTimeValue { get; set; }
        [DataMember]
        public string CheckInTime
        {
            get { return CheckInTimeValue.ToString(); }
            set { CheckInTimeValue = TimeSpan.Parse(value); }
        }


        [DataMember]
        public bool Sunday { get; set; }
        [DataMember]
        public bool Monday { get; set; }
        [DataMember]
        public bool Tuesday { get; set; }
        [DataMember]
        public bool Wednesday { get; set; }
        [DataMember]
        public bool Thursday { get; set; }
        [DataMember]
        public bool Friday { get; set; }
        [DataMember]
        public bool Saturday { get; set; }

        public override int Interval
        {
            get { return Service.DailySchedule.IntervalId; }
            set { int x = value; } //noop
        }
    }
}
