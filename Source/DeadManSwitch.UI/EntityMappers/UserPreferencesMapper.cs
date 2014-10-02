using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.UI
{
    public static class UserPreferencesMapper
    {
        public static DeadManSwitch.Service.UserPreferences ToServiceEntity(this DeadManSwitch.UI.UserPreferencesModel source)
        {
            var target = new DeadManSwitch.Service.UserPreferences();

            target.EarlyCheckInOffset = new TimeSpan(0, source.EarlyCheckInMinutes, 0);
            target.TzInfo = TimeZoneInfo.FindSystemTimeZoneById(source.TimeZoneId);

            return target;
        }
    }
}
