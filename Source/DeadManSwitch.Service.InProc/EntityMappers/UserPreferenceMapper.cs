using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Service.EntityMappers
{
    internal static class UserPreferenceMapper
    {
        public static DeadManSwitch.Service.UserPreferences ToServiceEntity(this DeadManSwitch.UserPreferences domainPrefs)
        {
            if (domainPrefs == null) return null;

            var svcPrefs = new DeadManSwitch.Service.UserPreferences();

//            svcPrefs.UserId = domainPrefs.UserId;
            svcPrefs.EarlyCheckInOffset = domainPrefs.EarlyCheckInOffset;
            svcPrefs.TzInfo = domainPrefs.TzInfo;

            return svcPrefs;
        }

        public static DeadManSwitch.UserPreferences ToDomainEntity(this DeadManSwitch.Service.UserPreferences svcPrefs)
        {
            if (svcPrefs == null) return null;

            var domainPrefs = new DeadManSwitch.UserPreferences();

//            domainPrefs.UserId = svcPrefs.UserId;
            domainPrefs.EarlyCheckInOffset = svcPrefs.EarlyCheckInOffset;
            domainPrefs.TzInfo = svcPrefs.TzInfo;

            return domainPrefs;
        }

    }
}
