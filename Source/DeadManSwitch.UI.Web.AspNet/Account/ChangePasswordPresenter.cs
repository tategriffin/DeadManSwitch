using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeadManSwitch.Service;

namespace DeadManSwitch.UI.Web.AspNet.Account
{
    public class ChangePasswordPresenter : DMSPagePresenter
    {
        private IAccountService AccountSvc;

        public ChangePasswordPresenter(CurrentUser user)
            : base(user)
        {
            this.AccountSvc = GetService<IAccountService>();

        }

        public ChangePasswordResultModel ChangePassword(string userName, string currentPassword, string password)
        {
            ChangePasswordResultModel result;
            
            var wasPwdChanged = this.AccountSvc.ChangePassword(userName, currentPassword, password);
            if (wasPwdChanged)
            {
                result = new ChangePasswordResultModel("Your password has been changed.");
            }
            else
            {
                result = new ChangePasswordResultModel("Your password could not be changed.", false);
            }

            return result;
        }

    }
}
