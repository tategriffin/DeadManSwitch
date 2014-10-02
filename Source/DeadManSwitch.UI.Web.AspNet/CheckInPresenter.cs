using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeadManSwitch.Service;
using NLog;

namespace DeadManSwitch.UI.Web.AspNet
{
    public class CheckInPresenter : DMSPagePresenter
    {
        private static NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        private ICheckInService CheckInSvc;

        public CheckInPresenter(CurrentUser user)
            : base(user)
        {
            this.CheckInSvc = GetService<ICheckInService>();
        }

        public CheckInResultModel CheckIn()
        {
            CheckInResultModel model = new CheckInResultModel();

            if (this.CurrentUser.IsAuthenticated == false)
            {
                PopulateModelForUnauthenticatedUser(model);
                return model;
            }

            try
            {
                CheckInInfo lastCheckInInfo = this.CheckInSvc.CheckInUser(this.CurrentUser.UserName);
                if (lastCheckInInfo == null)
                {
                    PopulateModelForFailure(model);
                }
                else
                {
                    PopulateModelForSuccess(model, lastCheckInInfo);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                PopulateModelForFailure(model);
            }

            return model;
        }

        private void PopulateModelForSuccess(CheckInResultModel model, CheckInInfo lastCheckInInfo)
        {
            model.CheckInStatusMessage = string.Format("Thanks for checking in {0}.", lastCheckInInfo.DisplayName);
            model.NextCheckInText = lastCheckInInfo.GetNextCheckInText();
            model.NextCheckIn = lastCheckInInfo.NextCheckInTimeUserLocal;
        }

        private void PopulateModelForFailure(CheckInResultModel model)
        {
            model.CheckInStatusMessage = "Your check in failed. Please try again in a few minutes.";
            model.NextCheckInText = "Unknown";
        }

        private void PopulateModelForUnauthenticatedUser(CheckInResultModel model)
        {
            model.CheckInStatusMessage = "You must be logged in before checking in.";
            model.NextCheckInText = "Unknown";
        }

    }
}
