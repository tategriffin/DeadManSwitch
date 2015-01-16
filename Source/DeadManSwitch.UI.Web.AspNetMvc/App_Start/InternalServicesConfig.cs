using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DeadManSwitch.Configuration;
using Microsoft.Practices.Unity;

namespace DeadManSwitch.UI.Web.AspNetMvc
{
    public class InternalServicesConfig
    {
        public static void Configure(IUnityContainer container, IHostSettingsReader config)
        {
            bool useInProcessServices = config.GetSetting<bool>(ConfigurationKeys.InProcessServicesEnabled);
            if (useInProcessServices)
            {
                RegisterInProcessServices(container, config);
            }
            else
            {
                RegisterOutOfProcessServices(container, config);
            }
        }

        private static void RegisterInProcessServices(IUnityContainer container, IHostSettingsReader config)
        {
            DeadManSwitch.Service.BootStrapper.Configure(container, config);

            container.RegisterType<DeadManSwitch.Service.IAccountService, DeadManSwitch.Service.AccountService>();
            container.RegisterType<DeadManSwitch.Service.IActionService, DeadManSwitch.Service.ActionService>();
            container.RegisterType<DeadManSwitch.Service.ICheckInService, DeadManSwitch.Service.CheckInService>();
            container.RegisterType<DeadManSwitch.Service.IScheduleService, DeadManSwitch.Service.ScheduleService>();

            container.RegisterType<DeadManSwitch.Service.IEscalationService, DeadManSwitch.Service.EscalationService>();
        }

        private static void RegisterOutOfProcessServices(IUnityContainer container, IHostSettingsReader config)
        {
            var serviceAdapterAssembly = LoadAdapterAssembly(config);

            RegisterInterfaceImplementation(container, serviceAdapterAssembly, typeof(DeadManSwitch.Service.IAccountService));
            RegisterInterfaceImplementation(container, serviceAdapterAssembly, typeof(DeadManSwitch.Service.IActionService));
            RegisterInterfaceImplementation(container, serviceAdapterAssembly, typeof(DeadManSwitch.Service.ICheckInService));
            RegisterInterfaceImplementation(container, serviceAdapterAssembly, typeof(DeadManSwitch.Service.IScheduleService));

            RegisterInterfaceImplementation(container, serviceAdapterAssembly, typeof(DeadManSwitch.Service.IEscalationService));
        }

        private static Assembly LoadAdapterAssembly(IHostSettingsReader config)
        {
            string servicesAdapterAssemblyName = config.GetSetting(ConfigurationKeys.OutOfProcessServicesAdaptersAssembly);
            string fullPath = Path.Combine(AppDomain.CurrentDomain.RelativeSearchPath, servicesAdapterAssemblyName);

            var adapterAssembly = Assembly.LoadFrom(fullPath);
            return adapterAssembly;
        }

        private static void RegisterInterfaceImplementation(IUnityContainer container, Assembly serviceAdapterAssembly, Type interfaceType)
        {
            Type concreteType = FindImplementationType(serviceAdapterAssembly, interfaceType);
            container.RegisterType(interfaceType, concreteType);
        }

        private static Type FindImplementationType(Assembly serviceAdapterAssembly, Type interfaceType)
        {
            var concreteTypeList =
                serviceAdapterAssembly.GetExportedTypes().Where(t => interfaceType.IsAssignableFrom(t)).ToList();
            if (concreteTypeList.Count == 1)
            {
                return concreteTypeList[0];
            }
            else
            {
                if (concreteTypeList.Count == 0)
                {
                    throw new Exception(string.Format("Assembly {0} does not contain any types implementing {1}", serviceAdapterAssembly.FullName, interfaceType.FullName));
                }
                else
                {
                    throw new Exception(string.Format("Assembly {0} contains multiple types implementing {1}", serviceAdapterAssembly.FullName, interfaceType.FullName));
                }
            }
        }

    }
}
