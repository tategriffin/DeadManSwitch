using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeadManSwitch.Service;

namespace DeadManSwitch.UI.Web.AspNet.Actions
{
    public class EditAllUserActionsPresenter : DMSPagePresenter
    {
        private const int MaxEscalationActions = 10;

        private IActionService ActionSvc;

        public EditAllUserActionsPresenter(CurrentUser user)
            : base(user)
        {
            this.ActionSvc = GetService<IActionService>();

            this.PopulateModel();
        }

        public EditAllUserActionsModel Model { get; private set; }

        public void Save(List<EditUserActionModel> userActions)
        {
            if (userActions == null) throw new ArgumentNullException("userActions");

            List<EscalationStep> allSteps = userActions.ToServiceEntityList();
            this.ActionSvc.SaveUserEscalationSteps(this.CurrentUser.UserName, allSteps);
        }

        private void PopulateModel()
        {
            EditAllUserActionsModel model = new EditAllUserActionsModel();

            model.UserActions = BuildUserActions();

            model.ActionTypeOptions = BuildActionTypeOptions();
            model.WaitMinuteOptions = BuildWaitMinuteOptions();

            this.Model = model;
        }

        private List<EditUserActionModel> BuildUserActions()
        {
            List<EditUserActionModel> actions = new List<EditUserActionModel>();

            if (this.CurrentUser.IsAuthenticated)
            {
                var allSteps = this.ActionSvc.FindUserEscalationSteps(this.CurrentUser.UserName);
                actions.AddRange(allSteps.ToUiEditModelList());

                actions.AddRange(this.BuildEmptyUserActions(actions.Count));
            }

            return actions;
        }

        private List<EditUserActionModel> BuildEmptyUserActions(int existingActionCount)
        {
            List<EditUserActionModel> unusedSteps = new List<EditUserActionModel>();

            for (int step = existingActionCount + 1; step <= MaxEscalationActions; step++)
            {
                unusedSteps.Add(EditUserActionModel.CreateNewStep(step));
            }

            return unusedSteps;
        }

        private Dictionary<string, string> BuildWaitMinuteOptions()
        {
            return this.ActionSvc.GetAllEscalationWaitMinutes().ToStringKeysAndValues();
        }

        private Dictionary<string, string> BuildActionTypeOptions()
        {
            return this.ActionSvc.GetAllEscalationActionTypes().ToStringKeysAndValues();
        }

    }
}
