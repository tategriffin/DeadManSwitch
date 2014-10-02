using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.UI
{
    public class CheckInResultModel
    {
        public string CheckInStatusMessage { get; set; }
        public string NextCheckInText { get; set; }
        public DateTime? NextCheckIn { get; set; }
    }
}
