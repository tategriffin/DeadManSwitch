using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.UI.Web.AspNet
{
    public class HomePresenter : DMSPagePresenter
    {
        public HomePresenter(CurrentUser user)
            : base(user)
        {
            this.SetState();
        }

        public string FeaturedMessage { get; private set; }
        public bool IsCheckInNowVisible { get; private set; }

        private void SetState()
        {
            this.FeaturedMessage = BuildFeaturedMessage(this.CurrentUser, CurrentAppState.ApplicationName);
            this.IsCheckInNowVisible = this.CurrentUser.IsAuthenticated;
        }

        private string BuildFeaturedMessage(CurrentUser user, string applicationName)
        {
            string message;
            if (user.IsAuthenticated && string.IsNullOrWhiteSpace(user.UserName) == false)
            {
                message = string.Format("Welcome back to {0}, {1}", applicationName, user.UserName);
            }
            else
            {
                message = string.Format("Welcome to {0}", applicationName);
            }

            return message;
        }

    }
}
