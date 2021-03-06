﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeadManSwitch.Configuration;
using Microsoft.Practices.Unity;

namespace DeadManSwitch.Service
{
    public static class BootStrapper
    {
        private static readonly object Padlock = new object();
        private static bool IsConfigured = false;

        public static void Configure(IUnityContainer container, IHostSettingsReader config)
        {
            if (container == null) throw new ArgumentNullException("container");
            if (IsConfigured) return;

            lock (Padlock)
            {
                RepositoryConfig.Configure(container, config);
                ExternalServicesConfig.Configure(container,config);
                EscalationDaemon.Start(container, config);

                IsConfigured = true;
            }
        }
    }

}
