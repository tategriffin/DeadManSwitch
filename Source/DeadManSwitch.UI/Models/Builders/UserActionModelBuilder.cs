using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using DeadManSwitch.Service;

namespace DeadManSwitch.UI.Models.Builders
{
    public class UserActionModelBuilder
    {
        private readonly IActionService ActionSvc;

        public UserActionModelBuilder(IActionService actionService)
        {
            ActionSvc = actionService;
        }

        public async Task<UserActionEditModel> BuildCreateModelAsync(string userName)
        {
            int stepNumber = 1;
            var existingSteps = await ActionSvc.FindAllEscalationStepsByUserNameAsync(userName);
            if (existingSteps.Any())
            {
                stepNumber = existingSteps.Last().Number + 1;
            }

            UserActionEditModel model = new UserActionEditModel() {Step = stepNumber};
            model.SubmitActionText = "Create step";

            await PopulateModelNonPersistentInfoAsync(model);

            return model;
        }

        public async Task<UserActionEditModel> BuildEditModelAsync(DeadManSwitch.Service.EscalationStep step)
        {
            UserActionEditModel model = step.ToUiEditModel();
            model.SubmitActionText = "Save changes";

            await PopulateModelNonPersistentInfoAsync(model);

            return model;
        }

        public async Task PopulateModelNonPersistentInfoAsync(UserActionEditModel model)
        {
            model.ActionTypeOptions = await BuildActionTypeOptionsAsync();
            model.WaitMinuteOptions = await BuildWaitMinuteOptionsAsync();
        }

        private async Task<Dictionary<string, string>> BuildActionTypeOptionsAsync()
        {
            var actionTypes = await ActionSvc.GetAllEscalationActionTypesAsync();
            return actionTypes.ToDictionary(i => i.Key.ToString(), i => i.Value);
        }

        private async Task<Dictionary<string, string>> BuildWaitMinuteOptionsAsync()
        {
            var waitMinutes = await ActionSvc.GetAllEscalationWaitMinutesAsync();
            return waitMinutes.ToDictionary(i => i.Key.ToString(), i => i.Value);
        }

    }
}
