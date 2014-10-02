using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.UI
{
    public class EditDailyScheduleModel
    {
        public EditDailyScheduleModel()
            : this(false, false) { }

        public EditDailyScheduleModel(bool setAllDays, bool isEnabled)
        {
            Id = 0;
            UserId = 0;
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
        public int UserId { get; set; }
        public string ScheduleName { get; set; }
        public bool IsEnabled { get; set; }
        public bool Sunday { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }

        public TimeModel CheckIn { get; set; }
        public TimeModel EarlyCheckIn { get; set; }

        public string UserTimeZone { get; set; }
        public Dictionary<string, string> CheckInHourOptions { get; set; }
        public Dictionary<string, string> CheckInMinuteOptions { get; set; }
        public Dictionary<string, string> CheckInAmPmOptions { get; set; }

    }

}
