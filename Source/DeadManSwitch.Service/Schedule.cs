using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Service
{
    public abstract class Schedule : ISchedule
    {
        protected Schedule()
        {
            UniqueId = Guid.Empty;
        }

        public int Id { get; set; }
        public Guid UniqueId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }

        public abstract RecurrenceInterval Interval { get; }
    }
}
