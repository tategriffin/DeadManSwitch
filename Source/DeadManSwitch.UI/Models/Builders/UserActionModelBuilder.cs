using System;
using System.Collections.Generic;
using System.Linq;
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

        public UserActionEditModel BuildCreateModel(string userName)
        {
            int stepNumber = 1;
            var existingSteps = ActionSvc.FindUserEscalationSteps(userName);
            if (existingSteps.Any())
            {
                stepNumber = existingSteps.Last().Number + 1;
            }

            UserActionEditModel model = new UserActionEditModel() {Step = stepNumber};
            model.SubmitActionText = "Create step";

            PopulateModelNonPersistentInfo(model);

            return model;
        }

        public UserActionEditModel BuildEditModel(DeadManSwitch.Service.EscalationStep step)
        {
            UserActionEditModel model = step.ToUiEditModel();
            model.SubmitActionText = "Save changes";

            PopulateModelNonPersistentInfo(model);

            return model;
        }

        public void PopulateModelNonPersistentInfo(UserActionEditModel model)
        {
            model.ActionTypeOptions = BuildActionTypeOptions();
            model.WaitMinuteOptions = BuildWaitMinuteOptions();
        }

        private Dictionary<string, string> BuildActionTypeOptions()
        {
            return ActionSvc.GetAllEscalationActionTypes()
                .ToDictionary(i => i.Key.ToString(), i => i.Value);
        }

        private Dictionary<string, string> BuildWaitMinuteOptions()
        {
            return ActionSvc.GetAllEscalationWaitMinutes()
                .ToDictionary(i => i.Key.ToString(), i => i.Value);
        }

    }
}
