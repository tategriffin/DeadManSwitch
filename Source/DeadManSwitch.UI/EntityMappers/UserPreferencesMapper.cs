using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.UI
{
    public static class UserPreferencesMapper
    {
        public static DeadManSwitch.UI.UserPreferenceEditModel ToUiEditModel(this DeadManSwitch.Service.UserPreferences source)
        {
            var target = new DeadManSwitch.UI.UserPreferenceEditModel();

            target.EarlyCheckInMinutes = (int)source.EarlyCheckInOffset.TotalMinutes;
            target.TimeZoneId = source.TzInfo.Id;

            return target;
        }

        public static DeadManSwitch.Service.UserPreferences ToServiceEntity(this DeadManSwitch.UI.UserPreferenceEditModel source)
        {
            var target = new DeadManSwitch.Service.UserPreferences();

            target.EarlyCheckInOffset = new TimeSpan(0, source.EarlyCheckInMinutes, 0);
            target.TzInfo = TimeZoneInfo.FindSystemTimeZoneById(source.TimeZoneId);

            return target;
        }
    }
}
