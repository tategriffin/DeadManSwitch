using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.UI
{
    public class TimeModel
    {
        private const int Noon = 12;
        private const int Midnight = 12;
        private const string Morning = "AM";
        private const string Afternoon = "PM";

        public TimeModel()
        {
            SetTime(new TimeSpan());
        }

        public TimeModel(TimeSpan ts)
        {
            SetTime(ts);
        }

        public TimeModel(string hour, string minute, string ampm)
        {
            int hr = int.Parse(hour);
            int min = int.Parse(minute);

            SetTime(hr, min, ampm);
        }

        public TimeModel(int hour, int minute, string ampm)
        {
            SetTime(hour, minute, ampm);
        }

        private TimeSpan Time;
        public TimeSpan ToTimeSpan() { return new TimeSpan(this.Time.Ticks); }
        
        public string Hour
        {
            get
            {
                if (this.Time.Hours == 0) return Midnight.ToString();
                if (this.Time.Hours > Noon) return (this.Time.Hours - Noon).ToString();

                return this.Time.Hours.ToString();
            }
        }
        public string Minute { get { return Time.Minutes.ToString(); } }
        public string AMPM { get { return (Time.Hours < Noon ? Morning : Afternoon); } }

        private void SetTime(int hour, int minute, string ampm)
        {
            int militaryHour = ConvertHourToMilitary(hour, ampm);

            SetTime(militaryHour, minute);
        }

        private void SetTime(int hour, int minute)
        {
            if (hour < 0 || hour > 23) throw new ArgumentOutOfRangeException("hour", "value must be between 0 and 23.");
            if (minute < 0 || minute > 59) throw new ArgumentOutOfRangeException("minute", "value must be between 0 and 59.");

            SetTime(new TimeSpan(hour, minute, 0));
        }

        private void SetTime(TimeSpan ts)
        {
            if (ts == null) throw new ArgumentNullException("ts");

            this.Time = ts;
        }

        private int ConvertHourToMilitary(int hour, string ampm)
        {
            if (!ampm.Equals(Morning, StringComparison.InvariantCultureIgnoreCase)
                && !ampm.Equals(Afternoon, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ArgumentException(string.Format("parameter must be either {0} or {1}.", Morning, Afternoon));
            }

            int militaryHour;
            switch (hour)
            {
                case 12:
                    militaryHour = (ampm.Equals(Morning, StringComparison.InvariantCultureIgnoreCase) ? 0 : 12);
                    break;
                default:
                    militaryHour = (ampm.Equals(Morning, StringComparison.InvariantCultureIgnoreCase) ? hour : hour + 12);
                    break;
            }

            return militaryHour;
        }
    }
}
