using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Service
{
    public class UserPreferences
    {
        public TimeZoneInfo TzInfo { get; set; }
        public TimeSpan EarlyCheckInOffset { get; set; }
    }
}
