using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeadManSwitch.Data.TestRepository;

namespace DeadManSwitch.Data.TestRepository
{
    public class ApplicationSettingsRepository : RepositoryWithContext, IApplicationSettingsRepository
    {
        public ApplicationSettingsRepository(RepositoryContext context)
            :base(context) { }

        public string GetSetting(string key)
        {
            if (Context.ApplicationSettings.ContainsKey(key))
            {
                return Context.ApplicationSettings[key];
            }
            else
            {
                return null;
            }
        }

        public void SetSetting(string key, string value)
        {
            if (Context.ApplicationSettings.ContainsKey(key))
            {
                Context.ApplicationSettings[key] = value;
            }
        }

    }
}
