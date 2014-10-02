using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch
{
    public class CheckInInfo
    {
        public CheckInInfo(User user, TimeZoneInfo tzInfo)
        {
            this.User = user;
            this.CheckInTimeUtc = null;
            this.NextCheckInTimeUtc = null;
            this.UserTimeZone = tzInfo;
        }

        public User User { get; set; }
        public DateTime? CheckInTimeUtc { get; set; }
        public DateTime? NextCheckInTimeUtc { get; set; }
        public TimeZoneInfo UserTimeZone { get; internal set; }

    }
}
