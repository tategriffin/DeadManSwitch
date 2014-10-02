using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch
{
    public interface ISchedule
    {
        int Id { get; set; }
        int UserId { get; set; }
        string Name { get; set; }
        string Description { get; }
        bool Enabled { get; set; }

        DateTime? CalculateNextCheckIn(TimeZoneInfo userTimeZone);
        RecurrenceInterval Recurrence { get; }
    }
}
