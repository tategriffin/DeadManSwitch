using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Configuration
{
    public interface IHostSettingsReader
    {
        string GetSetting(string key);
        string GetSetting(string key, string defaultValue);

        T GetSetting<T>(string key);
        T GetSetting<T>(string key, string defaultValue);

    }
}
