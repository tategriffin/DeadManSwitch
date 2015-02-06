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
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        private static void ReferenceTypesForDeploy()
        {
            //This is simply a hack to make sure certain files are included in /bin during build and deploy.
            var ref1 = typeof(DeadManSwitch.Service.Wcf.Proxy.AccountServiceProxy);
            var ref2 = typeof(System.Data.Entity.SqlServer.SqlProviderServices);
        }

    }
}