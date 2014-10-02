using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeadManSwitch.Providers;
using DeadManSwitch.Service.EntityMappers;
using Microsoft.Practices.Unity;

namespace DeadManSwitch.Service
{
    public class ActionService : IActionService
    {
        IUnityContainer Container;

        private ReferenceDataProvider RefDataProvider;
        private UserProvider UserProvider;
        private UserEscalationProcedureProvider UserEscalationProvider;

        public ActionService(IUnityContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");

            this.Container = container;
            this.RefDataProvider = new ReferenceDataProvider(this.Container);
            this.UserProvider = new UserProvider(this.Container);
            this.UserEscalationProvider = new UserEscalationProcedureProvider(this.Container);
        }

        public Dictionary<int, string> GetAllEscalationActionTypes()
        {
            return this.RefDataProvider.EscalationActionTypes();
        }

        public Dictionary<int, string> GetAllEscalationWaitMinutes()
        {
            return this.RefDataProvider.EscalationWaitMinuteOptions();
        }

        public List<EscalationStep> FindUserEscalationSteps(string userName)
        {
            DeadManSwitch.User user = UserProvider.FindByUserName(userName);
            EscalationProcedures procedures = this.UserEscalationProvider.FindByUserId(user.UserId);

            return procedures.EscalationList.ToEscalationSteps();
        }

        public void SaveUserEscalationSteps(string userName, IEnumerable<EscalationStep> allSteps)
        {
            DeadManSwitch.User user = UserProvider.FindByUserName(userName);
            EscalationProcedures procedures = new EscalationProcedures(user.UserId, allSteps.ToUserEscalationTasks(user.UserId));
            
            this.UserEscalationProvider.Save(user, procedures);
        }

    }
}
