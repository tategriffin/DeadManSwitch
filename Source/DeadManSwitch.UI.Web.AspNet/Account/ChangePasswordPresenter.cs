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
            ChangePasswordResultModel result = new ChangePasswordResultModel();
            
            result.PasswordWasChanged = this.AccountSvc.ChangePassword(userName, currentPassword, password);
            if (result.PasswordWasChanged)
            {
                result.ResultMessage = "Your password has been changed.";
            }
            else
            {
                result.ResultMessage = "Your password could not be changed.";
            }

            return result;
        }

    }
}
