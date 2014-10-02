using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch
{
    public class MissedCheckIn
    {
        public int UserId { get; set; }
        public DateTime ExpectedCheckIn { get; set; }
        public DateTime? LastCheckIn { get; set; }
    }
}
