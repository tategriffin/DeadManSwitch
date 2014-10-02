using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.UI
{
    public class EditUserActionModel
    {
        public EditUserActionModel()
        {
            this.Id = 0;
            this.Step = 0;
            this.WaitMinutes = 0;
            this.ActionType = ActionType.None;
            this.Recipient = string.Empty;
        }

        public int Id { get; set; }
        public int Step { get; set; }
        public int WaitMinutes { get; set; }
        public ActionType ActionType { get; set; }

        private string RecipientValue;
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

        public static EditUserActionModel CreateNewStep(int stepNumber)
        {
            if (stepNumber < 1) throw new ArgumentException("stepNumber must be greater than 0.");

            EditUserActionModel action = new EditUserActionModel();

            action.Step = stepNumber;

            return action;
        }
    }
}
