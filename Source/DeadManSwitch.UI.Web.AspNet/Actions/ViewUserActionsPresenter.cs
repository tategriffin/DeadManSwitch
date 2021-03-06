﻿using System;
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

        public List<UserActionViewModel> Model { get; private set; }

        private List<UserActionViewModel> BuildUserActions()
        {
            List<UserActionViewModel> actions = new List<UserActionViewModel>();

            if (this.CurrentUser.IsAuthenticated)
            {
                var allSteps = this.ActionSvc.FindAllEscalationStepsByUserName(this.CurrentUser.UserName);
                actions.AddRange(allSteps.ToUiViewModelList());
            }

            return actions;
        }

    }
}
