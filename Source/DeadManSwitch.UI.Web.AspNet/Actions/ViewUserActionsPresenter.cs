using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeadManSwitch;
using DeadManSwitch.Service;

namespace DeadManSwitch.UI.Web.AspNet.Actions
{
    public class ViewUserActionsPresenter : DMSPagePresenter
    {
        private IActionService ActionSvc;

        public ViewUserActionsPresenter(CurrentUser user)
            : base(user)
        {
            this.ActionSvc = GetService<IActionService>();

            this.Model = BuildUserActions();
        }

        public List<ViewUserActionModel> Model { get; private set; }

        private List<ViewUserActionModel> BuildUserActions()
        {
            List<ViewUserActionModel> actions = new List<ViewUserActionModel>();

            if (this.CurrentUser.IsAuthenticated)
            {
                var allSteps = this.ActionSvc.FindUserEscalationSteps(this.CurrentUser.UserName);
                actions.AddRange(allSteps.ToUiViewModelList());
            }

            return actions;
        }

    }
}
