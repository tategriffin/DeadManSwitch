using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeadManSwitch.Service;

namespace DeadManSwitch.UI.Web.AspNet.Schedule
{
    public abstract class EditSchedulePresenter : DMSPagePresenter
    {
        public enum EditScheduleAction
        {
            Add,
            Change
        }

        protected EditScheduleAction Action { get; set; }
        protected int ScheduleId { get; set; }

        protected IScheduleService ScheduleSvc;
        protected IAccountService AccountSvc;

        public EditSchedulePresenter(CurrentUser user, System.Collections.Specialized.NameValueCollection qryString)
            : base(user)
        {
            if (qryString == null) throw new ArgumentNullException("qryString");

            CaptureQueryStringValues(qryString);
            this.ScheduleSvc = GetService<IScheduleService>();
            this.AccountSvc = GetService<IAccountService>();
        }

        protected virtual void CaptureQueryStringValues(System.Collections.Specialized.NameValueCollection qryString)
        {
            int schedIdPos = 0;

            int numOfParms = qryString.AllKeys.Length;
            switch (numOfParms)
            {
                case 0:
                    this.Action = EditScheduleAction.Add;
                    break;
                case 1:
                    int editScheduleId;
                    if (int.TryParse(qryString[schedIdPos], out editScheduleId) && editScheduleId != 0)
                    {
                        this.Action = EditScheduleAction.Change;
                        this.ScheduleId = editScheduleId;
                    }
                    break;
                default:
                    string msg = "Unrecognized query string. ";
                    string values = string.Join("; ", qryString.AllKeys.Select(key => key + ": " + qryString[key]).ToArray());

                    throw new Exception(msg + values);
            }

        }
    }
}
