using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.UI
{
    public class ScheduleViewModel
    {
        public int Id { get; set; }
        public bool Enabled { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DeadManSwitch.RecurrenceInterval Interval { get; set; }

        public string DisplayName
        {
            get { return string.Format("{0} ({1})", Name, Description); }
        }
    }
}
