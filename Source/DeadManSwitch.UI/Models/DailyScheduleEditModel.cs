using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.UI
{
    public class DailyScheduleEditModel
    {
        public DailyScheduleEditModel()
            : this(false, false) { }

        public DailyScheduleEditModel(bool setAllDays, bool isEnabled)
        {
            Id = 0;
            ScheduleName = string.Empty;
            IsEnabled = isEnabled;

            Sunday = setAllDays;
            Monday = setAllDays;
            Tuesday = setAllDays;
            Wednesday = setAllDays;
            Thursday = setAllDays;
            Friday = setAllDays;
            Saturday = setAllDays;

            this.CheckIn = new TimeModel();
            this.EarlyCheckIn = new TimeModel();
        }

        public int Id { get; set; }
        
        [Required(ErrorMessage = "Schedule Name is required")]
        public string ScheduleName { get; set; }
        public bool IsEnabled { get; set; }
        public bool Sunday { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }

        public TimeModel CheckIn
        {
            get { return new TimeModel(CheckInHour, CheckInMinute, CheckInAmPm); }
            set
            {
                CheckInHour = value.Hour;
                CheckInMinute = value.Minute;
                CheckInAmPm = value.AMPM;
            }
        }
        public int CheckInHour { get; set; }
        public int CheckInMinute { get; set; }
        public string CheckInAmPm { get; set; }


        public TimeModel EarlyCheckIn
        {
            get { return new TimeModel(EarlyCheckInHour, EarlyCheckInMinute, EarlyCheckInAmPm);}
            set
            {
                EarlyCheckInHour = value.Hour;
                EarlyCheckInMinute = value.Minute;
                EarlyCheckInAmPm = value.AMPM;
            }
        }
        public int EarlyCheckInHour { get; set; }
        public int EarlyCheckInMinute { get; set; }
        public string EarlyCheckInAmPm { get; set; }

        public string UserTimeZone { get; set; }
        public Dictionary<string, string> CheckInHourOptions { get; set; }
        public Dictionary<string, string> CheckInMinuteOptions { get; set; }
        public Dictionary<string, string> CheckInAmPmOptions { get; set; }

        public string SubmitActionText { get; set; }
    }

}
