using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Data.TestRepository
{
    /// <summary>
    /// Makes it look like user checked in earlier than they did
    /// </summary>
    public class PriorDateTimeCheckInRepository : CheckInRepository
    {
        public PriorDateTimeCheckInRepository(RepositoryContext context) : base(context)
        {
        }

        public override void RecordCheckIn(int userId, DateTime checkInDateTime, DateTime? nextCheckInDateTime)
        {
            DateTime modifiedCheckIn = checkInDateTime.AddDays(-1);
            DateTime? modifiedNextCheckIn = null;
            if (nextCheckInDateTime.HasValue)
            {
                modifiedNextCheckIn = nextCheckInDateTime.Value.AddDays(-1);
            }

            base.RecordCheckIn(userId, modifiedCheckIn, modifiedNextCheckIn);
        }
    }
}
