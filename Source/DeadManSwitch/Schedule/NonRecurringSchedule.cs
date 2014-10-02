using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Schedule
{
    public class NonRecurringSchedule
    {

        private NonRecurringSchedule(int userId)
        {
            if (userId == 0) throw new ArgumentException("userId is not valid.");

            this.UserId = userId;
        }

        public NonRecurringSchedule(int userId, TimeSpan offset)
            : this(userId)
        {
            if (offset.TotalMilliseconds < 0) throw new ArgumentException("offset cannot be negative.");

            this.NextCheckIn = DateTime.UtcNow.Add(offset);
        }

        public NonRecurringSchedule(int userId, DateTime utcCheckInDateTime)
            : this(userId)
        {
            if (utcCheckInDateTime.Kind != DateTimeKind.Utc) throw new ArgumentException("utcCheckInDateTime.Kind must be Utc");
            if (utcCheckInDateTime <= DateTime.UtcNow) throw new ArgumentException("utcCheckInDateTime must be after DateTime.UtcNow.");

            this.NextCheckIn = utcCheckInDateTime;
        }

        public NonRecurringSchedule(int userId, DateTime localCheckInDateTime, DateTime currentLocalDateTime)
            : this(userId)
        {
            if (localCheckInDateTime <= currentLocalDateTime) throw new ArgumentException("localCheckInDateTime must be after currentLocalDateTime.");

            this.NextCheckIn = DateTime.UtcNow.Add(DateTimeCalculator.CalculateOffset(localCheckInDateTime, currentLocalDateTime));
        }

        public int UserId { get; private set; }

        private DateTime utcNextCheckIn;
        public DateTime NextCheckIn 
        {
            get { return utcNextCheckIn; }
            set
            {
                if (value.Kind != DateTimeKind.Utc) throw new ArgumentException("value.Kind must be Utc");
                utcNextCheckIn = value;
            }
        }

    }
}
