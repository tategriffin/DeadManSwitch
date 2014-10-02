using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeadManSwitch.Configuration;
using Microsoft.Practices.Unity;

namespace DeadManSwitch.Service
{
    public class InternalServicesConfig
    {
        public static void Configure(IUnityContainer container, IHostSettingsReader config)
        {
            DeadManSwitch.Service.BootStrapper.Configure(container, config);

            RegisterServices(container, config);
        }

        private static void RegisterServices(IUnityContainer container, IHostSettingsReader config)
        {
            container.RegisterType<DeadManSwitch.Service.IAccountService, DeadManSwitch.Service.AccountService>();
            container.RegisterType<DeadManSwitch.Service.IActionService, DeadManSwitch.Service.ActionService>();
            container.RegisterType<DeadManSwitch.Service.ICheckInService, DeadManSwitch.Service.CheckInService>();
            container.RegisterType<DeadManSwitch.Service.IEscalationService, DeadManSwitch.Service.EscalationService>();
            container.RegisterType<DeadManSwitch.Service.IScheduleService, DeadManSwitch.Service.ScheduleService>();
        }

    }
}
