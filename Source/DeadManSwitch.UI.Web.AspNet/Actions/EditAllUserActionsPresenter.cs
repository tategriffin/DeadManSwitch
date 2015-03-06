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

        public UserActionEditAllModel Model { get; private set; }

        public void Save(List<UserActionEditModel> userActions)
        {
            if (userActions == null) throw new ArgumentNullException("userActions");

            List<EscalationStep> allSteps = userActions.ToServiceEntityList();
            this.ActionSvc.SaveEscalationSteps(this.CurrentUser.UserName, allSteps);
        }

        private void PopulateModel()
        {
            UserActionEditAllModel model = new UserActionEditAllModel();

            model.UserActions = BuildUserActions();

            model.ActionTypeOptions = BuildActionTypeOptions();
            model.WaitMinuteOptions = BuildWaitMinuteOptions();

            this.Model = model;
        }

        private List<UserActionEditModel> BuildUserActions()
        {
            List<UserActionEditModel> actions = new List<UserActionEditModel>();

            if (this.CurrentUser.IsAuthenticated)
            {
                var allSteps = this.ActionSvc.FindAllEscalationStepsByUserName(this.CurrentUser.UserName);
                actions.AddRange(allSteps.ToUiEditModelList());

                actions.AddRange(this.BuildEmptyUserActions(actions.Count));
            }

            return actions;
        }

        private List<UserActionEditModel> BuildEmptyUserActions(int existingActionCount)
        {
            List<UserActionEditModel> unusedSteps = new List<UserActionEditModel>();

            for (int step = existingActionCount + 1; step <= MaxEscalationActions; step++)
            {
                var emptyStep = new UserActionEditModel() {Step = step};
                unusedSteps.Add(emptyStep);
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
