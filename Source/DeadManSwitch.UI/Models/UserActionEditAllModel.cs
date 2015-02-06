using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.UI
{
    public class UserActionEditAllModel
    {
        public UserActionEditAllModel()
        {
            this.UserActions = new List<UserActionEditModel>();

            this.ActionTypeOptions = new Dictionary<string, string>();
            this.WaitMinuteOptions = new Dictionary<string, string>();
        }

        public List<UserActionEditModel> UserActions { get; set; }

        public Dictionary<string, string> ActionTypeOptions { get; set; }
        public Dictionary<string, string> WaitMinuteOptions { get; set; }
    }
}
