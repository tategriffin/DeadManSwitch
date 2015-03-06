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
        private readonly ReferenceDataProvider RefDataProvider;
        private readonly UserProvider UserProvider;
        private readonly UserEscalationProcedureProvider UserEscalationProvider;

        public ActionService(IUnityContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");

            this.RefDataProvider = new ReferenceDataProvider(container);
            this.UserProvider = new UserProvider(container);
            this.UserEscalationProvider = new UserEscalationProcedureProvider(container);
        }

        public Dictionary<int, string> GetAllEscalationActionTypes()
        {
            return this.RefDataProvider.EscalationActionTypes();
        }

        public Dictionary<int, string> GetAllEscalationWaitMinutes()
        {
            return this.RefDataProvider.EscalationWaitMinuteOptions();
        }

        public EscalationStep FindEscalationStepById(string userName, int stepId)
        {
            DeadManSwitch.User user = UserProvider.FindByUserName(userName);
            var task = UserEscalationProvider.FindTaskById(user.UserId, stepId);

            return task.ToEscalationStep();
        }

        public List<EscalationStep> FindAllEscalationStepsByUserName(string userName)
        {
            DeadManSwitch.User user = UserProvider.FindByUserName(userName);
            EscalationProcedures procedures = this.UserEscalationProvider.FindProceduresByUserId(user.UserId);

            return procedures.EscalationList.ToEscalationSteps();
        }

        public void SaveEscalationStep(string userName, EscalationStep step)
        {
            DeadManSwitch.User user = UserProvider.FindByUserName(userName);
            UserEscalationProvider.Save(user, step.ToUserEscalationTask(user.UserId));
        }

        public void SaveEscalationSteps(string userName, IEnumerable<EscalationStep> allSteps)
        {
            DeadManSwitch.User user = UserProvider.FindByUserName(userName);
            EscalationProcedures procedures = new EscalationProcedures(user.UserId, allSteps.ToUserEscalationTasks(user.UserId));
            
            this.UserEscalationProvider.Save(user, procedures);
        }

        public List<EscalationStep> ReorderEscalationSteps(string userName, IEnumerable<int> orderedStepIds)
        {
            DeadManSwitch.User user = UserProvider.FindByUserName(userName);
            UserEscalationProvider.ReorderSteps(user, orderedStepIds);

            return FindAllEscalationStepsByUserName(userName);
        }

        public void DeleteEscalationStep(string userName, int stepId)
        {
            DeadManSwitch.User user = UserProvider.FindByUserName(userName);
            UserEscalationProvider.Delete(user, stepId);
        }
    }
}
