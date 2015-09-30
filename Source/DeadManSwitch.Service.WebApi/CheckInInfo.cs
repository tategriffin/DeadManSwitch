using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Service.WebApi
{
    public class CheckInInfo
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public DateTime? CheckInTimeUtc { get; set; }
        public DateTime? NextCheckInTimeUtc { get; set; }
        public string UserTimeZoneId { get; set; }
    }
}
