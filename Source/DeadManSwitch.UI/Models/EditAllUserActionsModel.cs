using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.UI
{
    public class EditAllUserActionsModel
    {
        public EditAllUserActionsModel()
        {
            this.UserActions = new List<EditUserActionModel>();

            this.ActionTypeOptions = new Dictionary<string, string>();
            this.WaitMinuteOptions = new Dictionary<string, string>();
        }

        public List<EditUserActionModel> UserActions { get; set; }

        public Dictionary<string, string> ActionTypeOptions { get; set; }
        public Dictionary<string, string> WaitMinuteOptions { get; set; }
    }
}
