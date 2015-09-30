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
            if (container == null) throw new ArgumentNullException(nameof(container));

            this.RefDataProvider = new ReferenceDataProvider(container);
            this.UserProvider = new UserProvider(container);
            this.UserEscalationProvider = new UserEscalationProcedureProvider(container);
        }

        public Task<Dictionary<int, string>> GetAllEscalationActionTypesAsync()
        {
            return Task.FromResult(GetAllEscalationActionTypes());
        }

        public Dictionary<int, string> GetAllEscalationActionTypes()
        {
            return this.RefDataProvider.EscalationActionTypes();
        }

        public Task<Dictionary<int, string>> GetAllEscalationWaitMinutesAsync()
        {
            return Task.FromResult(GetAllEscalationWaitMinutes());
        }

        public Dictionary<int, string> GetAllEscalationWaitMinutes()
        {
            return this.RefDataProvider.EscalationWaitMinuteOptions();
        }

        public Task<EscalationStep> FindEscalationStepByIdAsync(string userName, int stepId)
        {
            return Task.FromResult(FindEscalationStepById(userName, stepId));
        }

        public EscalationStep FindEscalationStepById(string userName, int stepId)
        {
            DeadManSwitch.User user = UserProvider.FindByUserName(userName);
            var task = UserEscalationProvider.FindTaskById(user.UserId, stepId);

            return task.ToEscalationStep();
        }

        public Task<List<EscalationStep>> FindAllEscalationStepsByUserNameAsync(string userName)
        {
            return Task.FromResult(FindAllEscalationStepsByUserName(userName));
        }

        public List<EscalationStep> FindAllEscalationStepsByUserName(string userName)
        {
            DeadManSwitch.User user = UserProvider.FindByUserName(userName);
            EscalationProcedures procedures = this.UserEscalationProvider.FindProceduresByUserId(user.UserId);

            return procedures.EscalationList.ToEscalationSteps();
        }

        public Task SaveEscalationStepAsync(string userName, EscalationStep step)
        {
            SaveEscalationStep(userName, step);

            return Task.CompletedTask;
        }

        public void SaveEscalationStep(string userName, EscalationStep step)
        {
            DeadManSwitch.User user = UserProvider.FindByUserName(userName);
            UserEscalationProvider.Save(user, step.ToUserEscalationTask(user.UserId));
        }

        public Task SaveEscalationStepsAsync(string userName, IEnumerable<EscalationStep> allSteps)
        {
            SaveEscalationSteps(userName, allSteps);

            return Task.CompletedTask;
        }

        public void SaveEscalationSteps(string userName, IEnumerable<EscalationStep> allSteps)
        {
            DeadManSwitch.User user = UserProvider.FindByUserName(userName);
            EscalationProcedures procedures = new EscalationProcedures(user.UserId, allSteps.ToUserEscalationTasks(user.UserId));
            
            this.UserEscalationProvider.Save(user, procedures);
        }

        public Task<List<EscalationStep>> ReorderEscalationStepsAsync(string userName, IEnumerable<int> orderedStepIds)
        {
            return Task.FromResult(ReorderEscalationSteps(userName, orderedStepIds));
        }

        public List<EscalationStep> ReorderEscalationSteps(string userName, IEnumerable<int> orderedStepIds)
        {
            DeadManSwitch.User user = UserProvider.FindByUserName(userName);
            UserEscalationProvider.ReorderSteps(user, orderedStepIds);

            return FindAllEscalationStepsByUserName(userName);
        }

        public Task DeleteEscalationStepAsync(string userName, int stepId)
        {
            DeleteEscalationStep(userName, stepId);

            return Task.CompletedTask;
        }

        public void DeleteEscalationStep(string userName, int stepId)
        {
            DeadManSwitch.User user = UserProvider.FindByUserName(userName);
            UserEscalationProvider.Delete(user, stepId);
        }

    }
}
