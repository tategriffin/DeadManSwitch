using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.UI
{
    public class ScheduleListModel
    {
        public ScheduleListModel()
            : this (new List<ScheduleViewModel>()) { }
        public ScheduleListModel(IEnumerable<ScheduleViewModel> schedules)
        {
            Schedules = new List<ScheduleViewModel>(schedules);
            NextCheckInText = string.Empty;
        }

        public List<ScheduleViewModel> Schedules { get; private set; }
        public string NextCheckInText { get; set; }
    }
}
