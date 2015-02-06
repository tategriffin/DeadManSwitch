using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeadManSwitch.Configuration;

namespace DeadManSwitch.UI.Web.AspNetMvc
{
    public static class CurrentAppState
    {
        private static IHostSettingsReader AppSettings
        {
            get { return (IHostSettingsReader) DependencyResolver.Current.GetService(typeof (IHostSettingsReader)); }
        }
        //shortcut methods
        public static string ApplicationName
        {
            get { return AppSettings.GetSetting(ConfigurationKeys.ApplicationName); }
        }

        public static string BuildPageTitle(string title)
        {
            if (!string.IsNullOrWhiteSpace(title))
            {
                return ApplicationName + " - " + title;
            }
            else
            {
                return ApplicationName;
            }
        }

    }
}