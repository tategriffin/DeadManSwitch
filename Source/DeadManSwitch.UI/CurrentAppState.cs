using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeadManSwitch.Configuration;
using Microsoft.Practices.Unity;

namespace DeadManSwitch.UI
{
    public static class CurrentAppState
    {
        public static IUnityContainer IoCContainer { get; internal set; }
        
        //shortcut methods
        public static string ApplicationName
        {
            get { return IoCContainer.Resolve<IHostSettingsReader>().GetSetting(ConfigurationKeys.ApplicationName); }
        }

    }
}
