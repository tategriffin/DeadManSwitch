using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Data.SqlRepository
{
    public class ApplicationSettingsRepository : IApplicationSettingsRepository
    {
        public string GetSetting(string key)
        {
            DeadManSwitchEntities context = new DeadManSwitchEntities();
            try
            {
                var setting =
                    context.ApplicationSettings
                    .Where(s => string.Compare(s.Name, key, true) == 0)     //ignore case
                    .Select(s => new { s.Name, s.Value })
                    .SingleOrDefault();

                return (setting == null ? null : setting.Value);
            }
            finally
            {
                context.Dispose();
            }
        }
    }
}
