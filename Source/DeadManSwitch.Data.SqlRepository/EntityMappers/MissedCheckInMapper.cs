using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Data.SqlRepository.EntityMappers
{
    internal static class MissedCheckInMapper
    {
        internal static List<DeadManSwitch.MissedCheckIn> ToDomain(this IEnumerable<DeadManSwitch.Data.SqlRepository.vwMissedCheckIn> data)
        {
            List<DeadManSwitch.MissedCheckIn> domain = new List<DeadManSwitch.MissedCheckIn>();
            foreach (var item in data)
            {
                domain.Add(item.ToDomain());
            }

            return domain;
        }

        internal static DeadManSwitch.MissedCheckIn ToDomain(this DeadManSwitch.Data.SqlRepository.vwMissedCheckIn data)
        {
            if (data.NextCheckIn.HasValue == false) throw new ArgumentException(string.Format("MissedCheckIn parameter 'data' with CheckInId: {0} is not valid because data.NextCheckIn is null.", data.CheckInId));

            DeadManSwitch.MissedCheckIn domain = new DeadManSwitch.MissedCheckIn();
            domain.UserId = data.UserId;
            domain.LastCheckIn = data.LastCheckIn;
            domain.ExpectedCheckIn = data.NextCheckIn.Value;

            return domain;
        }

    }
}
