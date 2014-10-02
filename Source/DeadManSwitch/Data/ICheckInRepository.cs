using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Data
{
    public interface ICheckInRepository
    {
        void RecordCheckIn(int userId, DateTime checkInDateTime, DateTime? nextCheckInDateTime);

        CheckInInfo FindLastCheckInByUser(int userId);

        IList<MissedCheckIn> FindMissedCheckInsNeedingEscalation(int limit, TimeSpan retryLockTimeout, int maxRetries);

    }
}
