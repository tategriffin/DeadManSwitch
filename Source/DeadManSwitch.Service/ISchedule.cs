using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Service
{
    public interface ISchedule
    {
        int Id { get; set; }
        Guid UniqueId { get; set; }
        string Name { get; set; }
        string Description { get; }
        bool Enabled { get; set; }

        RecurrenceInterval Interval { get; }
    }
}
