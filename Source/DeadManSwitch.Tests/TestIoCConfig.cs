using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeadManSwitch.Action;
using DeadManSwitch.Data;
using DeadManSwitch.Data.TestRepository;
using ExternalServiceAdapters;
using Microsoft.Practices.Unity;

namespace DeadManSwitch.Tests
{
    internal static class TestIoCConfig
    {
        public static IUnityContainer BuildContainer(RepositoryContext context)
        {
            var container = new UnityContainer();

            RegisterExternalServices(container);
            RegisterRepositories(container, context);

            return container;
        }

        private static void RegisterExternalServices(IUnityContainer container)
        {
            container.RegisterType<ISendEmailAdapter, Fakes.SendEmailAdapterFake>();
            container.RegisterType<ISendSMSAdapter, Fakes.SendSMSAdapterFake>();

        }

        private static void RegisterRepositories(IUnityContainer container, RepositoryContext context)
        {
            container.RegisterType<IApplicationSettingsRepository, Data.TestRepository.ApplicationSettingsRepository>(new InjectionConstructor(context));
            container.RegisterType<ICheckInRepository, Data.TestRepository.CheckInRepository>(new InjectionConstructor(context));
            container.RegisterType<IAccountRepository, Data.TestRepository.AccountRepository>(new InjectionConstructor(context));
            container.RegisterType<IEscalationRepository, Data.TestRepository.EscalationRepository>(new InjectionConstructor(context));
            container.RegisterType<IUserEscalationProcedureRepository, Data.TestRepository.UserEscalationProcedureRepository>(new InjectionConstructor(context));
            container.RegisterType<IScheduleRepository, Data.TestRepository.ScheduleRepository>(new InjectionConstructor(context));
            container.RegisterType<IDailyScheduleRepository, Data.TestRepository.DailyScheduleRepository>(new InjectionConstructor(context));
            container.RegisterType<IUserPreferenceRepository, Data.TestRepository.UserPreferenceRepository>(new InjectionConstructor(context));
            container.RegisterType<IReferenceDataRepository, Data.TestRepository.ReferenceDataRepository>(new InjectionConstructor(context));
            container.RegisterType<IKillSwitchRepository, Data.TestRepository.KillSwitchRepository>(new InjectionConstructor(context));

        }

    }
}
