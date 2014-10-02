using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Configuration;
using DeadManSwitch.Configuration;

namespace DeadManSwitch.UI.Web.AspNet
{
    public class AppSettingReader : IHostSettingsReader
    {
        public string GetSetting(string key)
        {
            if(ConfigurationManager.AppSettings.AllKeys.Contains(key) == false) throw new Exception(string.Format("Application Setting {0} does not exist.", key));

            return MappedSettingValue(key, ConfigurationManager.AppSettings[key]);
        }

        public string GetSetting(string key, string defaultValue)
        {
            string value = defaultValue;
            if(ConfigurationManager.AppSettings.AllKeys.Contains(key))
            {
                value = ConfigurationManager.AppSettings[key];
            }

            return MappedSettingValue(key, value);
        }

        public T GetSetting<T>(string key)
        {
            string value = this.GetSetting(key);

            return (T)Convert.ChangeType(value, typeof(T));
        }

        public T GetSetting<T>(string key, string defaultValue)
        {
            string value = this.GetSetting(key, defaultValue);

            return (T)Convert.ChangeType(value, typeof(T));
        }

        /// <summary>
        /// Maps the setting value for "special case" configuration items.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <remarks>
        /// Use this method to map/pre-process configuration setting values.
        /// </remarks>
        private string MappedSettingValue(string key, string value)
        {
            string mappedValue;

            switch(key)
            {
                case ConfigurationKeys.ExternalServiceFileOutputRedirect:
                    mappedValue = HttpContext.Current.Server.MapPath(value);
                    break;

                default:
                    mappedValue = value;
                    break;
            }

            return mappedValue;
        }

    }
}