using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.UI
{
    public class UserActionEditModel
    {
        public UserActionEditModel()
        {
            this.Id = 0;
            this.Step = 0;
            this.WaitMinutes = 0;
            this.ActionType = ActionType.None;
            this.Recipient = string.Empty;
        }

        public int Id { get; set; }
        public int Step { get; set; }
        [Display(Name = "Wait")]
        public int WaitMinutes { get; set; }
        [Display(Name = "and then")]
        public ActionType ActionType { get; set; }

        public int ActionTypeId
        {
            get { return (int) this.ActionType; }
            set
            {
                if (Enum.IsDefined(typeof (ActionType), value))
                {
                    this.ActionType = (ActionType)value;
                }
            }
        }

        private string RecipientValue;
        [Display(Name = "to")]
        public string Recipient 
        {
            get
            {
                string formattedValue;
                switch (this.ActionType)
                {
                    case ActionType.TextMessage:
                        formattedValue = this.RecipientValue.ToPhoneNumber();
                        break;
                    default:
                        formattedValue = this.RecipientValue;
                        break;
                }

                return formattedValue;
            }
            set { this.RecipientValue = value; } 
        }

        public Dictionary<string, string> WaitMinuteOptions { get; set; }
        public Dictionary<string, string> ActionTypeOptions { get; set; }

        public string SubmitActionText { get; set; }
    }
}
