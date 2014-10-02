using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Data.SqlRepository.EntityMappers
{
    internal static class UserPreferenceMapper
    {
        internal static DeadManSwitch.UserPreferences ToDomain(this SqlRepository.UserPreference preferenceData)
        {
            DeadManSwitch.UserPreferences domainPreferences = new UserPreferences();

            domainPreferences.UserId = preferenceData.UserId;
            domainPreferences.TzInfo = TimeZoneInfo.FindSystemTimeZoneById(preferenceData.TimeZoneId);
            domainPreferences.EarlyCheckInOffset = new TimeSpan(0, preferenceData.EarlyCheckInMinutes, 0);

            return domainPreferences;
        }

        internal static void MapDomainToData(DeadManSwitch.UserPreferences domain, SqlRepository.UserPreference data)
        {
            data.UserId = domain.UserId;
            data.TimeZoneId = domain.TzInfo.Id;
            data.EarlyCheckInMinutes = (int)domain.EarlyCheckInOffset.TotalMinutes;
        }
    }
}
