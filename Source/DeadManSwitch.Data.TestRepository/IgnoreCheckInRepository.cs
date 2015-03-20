using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Data.TestRepository
{
    public class IgnoreCheckInRepository : RepositoryWithContext, ICheckInRepository
    {
        public IgnoreCheckInRepository(RepositoryContext context) : base(context)
        {
        }

        public void RecordCheckIn(int userId, DateTime checkInDateTime, DateTime? nextCheckInDateTime)
        {
            //Do nothing
        }

        public CheckInInfo FindLastCheckInByUser(int userId)
        {
            return null;
        }

        public IList<MissedCheckIn> FindMissedCheckInsNeedingEscalation(int limit, TimeSpan retryLockTimeout, int maxRetries)
        {
            throw new NotImplementedException("IgnoreCheckInRepository never has check-ins.");
        }
    }
}
