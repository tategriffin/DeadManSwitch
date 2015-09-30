using System;
using DeadManSwitch.Configuration;
using Microsoft.Practices.Unity;

namespace DeadManSwitch.Service.WebApi.Host
{
    public class InternalServicesConfig
    {
        public static void Configure(IUnityContainer container, IHostSettingsReader config)
        {
            if(container == null) throw new ArgumentNullException(nameof(container));

            RegisterServices(container, config);
        }

        private static void RegisterServices(IUnityContainer container, IHostSettingsReader config)
        {
            BootStrapper.Configure(container, config);

            container.RegisterType<IAccountService, AccountService>();
            container.RegisterType<IActionService, ActionService>();
            container.RegisterType<ICheckInService, CheckInService>();
            container.RegisterType<IScheduleService, ScheduleService>();
            container.RegisterType<IEscalationService, EscalationService>();
        }

    }
}
