using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeadManSwitch.Service;

namespace DeadManSwitch.UI.Web.AspNet.Account
{
    public class LoginPresenter : DMSPagePresenter
    {
        private static NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        private IAccountService authenticationSvc;

        public LoginPresenter(string ipAddress)
            : base(CurrentUser.Anonymous(ipAddress))
        {
            this.authenticationSvc = GetService<IAccountService>();

        }

        public bool TryLogin(string userName, string password, out string failedLoginMessage)
        {
            bool loginSuccessful = false;
            failedLoginMessage = string.Empty;

            if (String.IsNullOrWhiteSpace(userName))
            {
                failedLoginMessage = "User name cannot be null or empty.";
            }
            else if (String.IsNullOrWhiteSpace(password))
            {
                failedLoginMessage = "Password cannot be null or empty.";
            }
            else
            {
                try
                {
                    User user = authenticationSvc.Login(userName, password);
                    if (user != null)
                    {
                        loginSuccessful = true;
                    }
                    else
                    {
                        failedLoginMessage = "The user name, password, or both are incorrect.";
                    }
                }
                catch (Exception ex)
                {
                    failedLoginMessage = "Login failed because the required service is not available.";
                    Log.Error(ex.ToString());
                }
            }

            return loginSuccessful;
        }

    }
}
