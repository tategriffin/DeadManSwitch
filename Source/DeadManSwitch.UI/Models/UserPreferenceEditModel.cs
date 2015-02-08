using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.UI
{
    public class UserPreferenceEditModel
    {

        public UserPreferenceEditModel()
        {
            this.TimeZoneId = TimeZoneInfo.Utc.Id;
            this.EarlyCheckInMinutes = 60;

            this.TimeZoneOptions = new Dictionary<string, string>();
            this.EarlyCheckInOptions = new Dictionary<string, string>();
        }

        [DisplayName("Time Zone")]
        public string TimeZoneId { get; set; }
        [DisplayName("Check in window start")]
        public int EarlyCheckInMinutes { get; set; }

        public Dictionary<string, string> TimeZoneOptions { get; set; }
        public Dictionary<string, string> EarlyCheckInOptions { get; set; }

    }
}
