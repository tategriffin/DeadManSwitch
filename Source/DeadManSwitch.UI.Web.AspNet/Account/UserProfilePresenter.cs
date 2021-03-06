﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeadManSwitch.Service;

namespace DeadManSwitch.UI.Web.AspNet.Account
{
    public class UserProfilePresenter : DMSPagePresenter
    {
        private IAccountService AccountSvc;

        public UserProfilePresenter(CurrentUser user)
            : base(user)
        {
            this.AccountSvc = GetService<IAccountService>();

            this.PopulateModel();
        }

        public UserProfileEditModel Model { get; set; }

        private void PopulateModel()
        {
            var existingUser = this.AccountSvc.FindUser(this.CurrentUser.UserName);

            this.Model = existingUser.ToUiEditModel();
        }

        public void SaveProfile()
        {
            this.AccountSvc.UpdateProfile(this.CurrentUser.UserName, this.Model.ToServiceModel());
        }

    }
}
