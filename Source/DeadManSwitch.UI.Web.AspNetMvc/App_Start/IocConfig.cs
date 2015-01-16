using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeadManSwitch.Configuration;
using Microsoft.Practices.Unity;

namespace DeadManSwitch.UI.Web.AspNetMvc
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
            //DeadManSwitch.UI.BootStrapper.Configure(container, config); //TODO: Move this class and CurrentAppState to AspNet project
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

    }
}