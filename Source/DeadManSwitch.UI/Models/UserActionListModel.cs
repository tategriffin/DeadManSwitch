using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.UI.Models
{
    public class UserActionListModel
    {
        public UserActionListModel()
            : this (new List<UserActionViewModel>()) { }
        public UserActionListModel(IEnumerable<UserActionViewModel> actions)
        {
            UserActions = new List<UserActionViewModel>(actions);
        }

        public List<UserActionViewModel> UserActions { get; private set; }
    }
}
