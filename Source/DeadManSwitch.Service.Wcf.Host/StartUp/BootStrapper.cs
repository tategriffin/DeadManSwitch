using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.WebSockets;
using DeadManSwitch.Configuration;
using Microsoft.Practices.Unity;

namespace DeadManSwitch.Service.Wcf.Host
{
    public static class BootStrapper
    {
        private static readonly object Padlock = new object();
        private static bool IsConfigured = false;

        public static void Configure()
        {
            if (IsConfigured) return;
            lock (Padlock)
            {
                IUnityContainer container = new UnityContainer();
                IHostSettingsReader config = new AppSettingReader();

                InternalServicesConfig.Configure(container, config);

                CurrentAppState.IoCContainer = container;

                IsConfigured = true;
            }
        }
    }

}
