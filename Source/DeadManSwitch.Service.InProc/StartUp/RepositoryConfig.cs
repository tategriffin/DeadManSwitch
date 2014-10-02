using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeadManSwitch.Configuration;
using DeadManSwitch.Data;
using Microsoft.Practices.Unity;

namespace DeadManSwitch.Service
{
    internal class RepositoryConfig
    {
        public static void Configure(IUnityContainer container, IHostSettingsReader config)
        {
            RegisterSqlRepositories(container);
        }

        private static void RegisterSqlRepositories(IUnityContainer container)
        {
            container.RegisterType<IApplicationSettingsRepository, Data.SqlRepository.ApplicationSettingsRepository>();
            container.RegisterType<ICheckInRepository, Data.SqlRepository.CheckInRepository>();
            container.RegisterType<IAccountRepository, Data.SqlRepository.AccountRepository>();
            container.RegisterType<IEscalationRepository, Data.SqlRepository.EscalationRepository>();
            container.RegisterType<IUserEscalationProcedureRepository, Data.SqlRepository.UserEscalationProcedureRepository>();
            container.RegisterType<IUserPreferenceRepository, Data.SqlRepository.UserPreferenceRepository>();
            container.RegisterType<IReferenceDataRepository, Data.SqlRepository.ReferenceDataRepository>();
            container.RegisterType<IKillSwitchRepository, Data.SqlRepository.KillSwitchRepository>();

            container.RegisterType<IScheduleRepository, Data.SqlRepository.ScheduleRepository>();
            container.RegisterType<IDailyScheduleRepository, Data.SqlRepository.DailyScheduleRepository>();
        }


    }
}
