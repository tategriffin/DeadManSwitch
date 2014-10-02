using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DeadManSwitch.Service;

namespace DeadManSwitch.UI.Web.AspNet.Tasks
{
    /// <summary>
    /// Promotes missed checkins and runs escalation tasks
    /// </summary>
    /// <remarks>
    /// All threading/locking code should really not be here,
    /// but it is because I'm curerntly allowing anyone on
    /// the internet to access pages in 
    /// DeadManSwitch.UI.Web.AspNet.Tasks
    /// </remarks>
    public class EscalationTaskPresenter : DMSPagePresenter
    {
        private static readonly object padlock = new object();
        private static NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        private IEscalationService EscalationSvc;

        public EscalationTaskPresenter(CurrentUser user)
            : base(user)
        {
            this.EscalationSvc = GetService<IEscalationService>();
        }

        public System.Net.HttpStatusCode Run()
        {
            Log.Info(CurrentUser.ToString());

            System.Net.HttpStatusCode? statusCode = GetFakeStatusCode();
            if (!statusCode.HasValue)
            {
                statusCode = GetRealStatusCode();
            }

            LogRunResult(statusCode.Value);
            return statusCode.Value;
        }

        private static System.Net.HttpStatusCode? GetFakeStatusCode()
        {
            return EscalationTaskStatusCodeFactory.GetHttpStatusCode();
        }

        private System.Net.HttpStatusCode GetRealStatusCode()
        {
            System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.InternalServerError;
            try
            {
                bool successful = this.EscalationSvc.Run();
                if (successful)
                {
                    statusCode = System.Net.HttpStatusCode.OK;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }

            return statusCode;
        }

        private void LogRunResult(HttpStatusCode code)
        {
            if (code.IsInformational() || code.IsSuccess())
            {
                Log.Info(string.Format("Escalate returned: {0}", code));
            }
            else if (code.IsClientError() || code.IsServerError())
            {
                Log.Error(string.Format("Escalate returned error: {0}", code));
            }
            else
            {
                Log.Warn(string.Format("Escalate returned unexpected status: {0}", code));
            }
        }
    }
}
