using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DeadManSwitch.Configuration;
using Microsoft.Practices.Unity;

namespace DeadManSwitch.UI.Web.AspNet
{
    public static class IocConfig
    {
        public static void Register()
        {
            IUnityContainer container = new UnityContainer();
            
            IHostSettingsReader config = new AppSettingReader();
            container.RegisterInstance<IHostSettingsReader>(config);

            InternalServicesConfig.Configure(container, config);

            //Do this last
            CurrentAppState.IoCContainer = container;
        }

    }
}