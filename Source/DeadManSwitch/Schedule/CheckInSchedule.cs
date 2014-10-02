using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Schedule
{
    public abstract class CheckInSchedule : ISchedule
    {
        protected CheckInSchedule()
	    {
            this.Id = 0;
            this.UserId = 0;
            this.Name = string.Empty;
            this.Enabled = true;
	    }

        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Description { get { return BuildScheduleDescription(); } }
        public bool Enabled { get; set; }

        public abstract RecurrenceInterval Recurrence { get; }
        public abstract DateTime? CalculateNextCheckIn(TimeZoneInfo userTimeZone);
        protected abstract string BuildScheduleDescription();
    }
}
