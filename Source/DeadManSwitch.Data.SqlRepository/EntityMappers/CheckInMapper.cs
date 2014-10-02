using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Data.SqlRepository.EntityMappers
{
    internal static class CheckInMapper
    {
        public static DeadManSwitch.CheckInInfo ToDomain(this DeadManSwitch.Data.SqlRepository.CheckIn dataItem)
        {
            var domainUser = dataItem.UserAccount.ToDomain();
            var domainUserPrefs = dataItem.UserAccount.UserPreference.ToDomain();

            DeadManSwitch.CheckInInfo domainItem = new CheckInInfo(domainUser, domainUserPrefs.TzInfo);
            domainItem.CheckInTimeUtc = dataItem.LastCheckIn;
            domainItem.NextCheckInTimeUtc = dataItem.NextCheckIn;

            return domainItem;
        }

    }
}
