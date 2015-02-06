using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.UI
{
    public class UserActionViewModel
    {
        public UserActionViewModel()
        {
            Id = 0;
            StepNumber = 0;
            WaitMinutes = 0;
            Recipient = string.Empty;
            ActionType = ActionType.None;
        }

        public int Id { get; set; }
        public int StepNumber { get; set; }
        public int WaitMinutes { get; set; }
        public ActionType ActionType { get; set; }
        public string Recipient { get; set; }
        public string StepDescription { get { return BuildDescription(); } }

        private string BuildDescription()
        {
            if (StepNumber < 1) return string.Empty;

            if (StepNumber == 1)
            {
                return string.Format("If I miss a check in, {0}", BuildActionText());
            }
            else
            {
                return string.Format("then {0}", BuildActionText());
            }
        }

        private string BuildActionText()
        {
            string desc;
            switch (this.ActionType)
            {
                case ActionType.EmailMessage:
                    desc = BuildWaitText() + "send an email to " + Recipient;
                    break;

                case ActionType.TextMessage:
                    desc = BuildWaitText() + "send a text to " + Recipient.ToPhoneNumber();
                    break;

                default:
                    desc = "do nothing";
                    break;
            }

            return desc;
        }

        private string BuildWaitText()
        {
            if (WaitMinutes <= 0) return string.Empty;

            return string.Format("wait {0} minutes, and ", WaitMinutes);
        }

    }
}
